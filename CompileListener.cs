using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Compilation;
using System;
using System.Reflection;
using System.Media;
using System.Threading;
public class CompileListener : EditorWindow
{
    private static Thread _thread;
    private static int _activeCompilers;

    private const string MenuItemName = "Tools/Elevator Compiler";
    private const string UseWaitMusicKey = "EDITOR_UseWaitMusic";
    private static bool _useWaitMusic;

    [InitializeOnLoadMethod]
    public static void Loaded()
    {
        _useWaitMusic = EditorPrefs.GetBool(UseWaitMusicKey, true);
        EditorApplication.delayCall += UpdateToggle;
    }

    private static void CompilationPipeline_assemblyCompilationStarted(string obj)
    {
        _activeCompilers++;
        _thread = new Thread(PlaySound)
        {
            IsBackground = true
        };
        _thread.Start();
    }

    public static void PlaySound()
    {
        using (var player = new SoundPlayer($"{Environment.CurrentDirectory}/Assets/Editor/elevator.wav"))
        {
            player.PlaySync();
        }
        _thread.Join();
    }

    private static void CompilationFinished(string assembly, CompilerMessage[] messages)
    {
        _activeCompilers--;
        if(_activeCompilers == 0)
            AllCompilersDone();
    }

    private static void AllCompilersDone()
    {
        using (var player = new SoundPlayer($"{Environment.CurrentDirectory}/Assets/Editor/elevator_ding.wav"))
        {
            Debug.Log("All compilers done");
            player.PlaySync();
        }
    }

    [MenuItem(MenuItemName)]
    public static void ToggleWaitMusic()
    {
        _useWaitMusic = !_useWaitMusic;
        UpdateToggle();
    }

    private static void UpdateToggle()
    {
        EditorPrefs.SetBool(UseWaitMusicKey, _useWaitMusic);
        Menu.SetChecked(MenuItemName, _useWaitMusic);

        if (_useWaitMusic)
        {
            CompilationPipeline.assemblyCompilationStarted += CompilationPipeline_assemblyCompilationStarted;
            CompilationPipeline.assemblyCompilationFinished += CompilationFinished;
        }
        else
        {
            CompilationPipeline.assemblyCompilationStarted -= CompilationPipeline_assemblyCompilationStarted;
            CompilationPipeline.assemblyCompilationFinished -= CompilationFinished;
        }
    }
}
