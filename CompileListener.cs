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
    static Thread _t;
    [InitializeOnLoadMethod]
    public static void Loaded()
    {
        CompilationPipeline.assemblyCompilationStarted += CompilationPipeline_assemblyCompilationStarted;
    }

    private static void CompilationPipeline_assemblyCompilationStarted(string obj)
    {
        _t = new Thread(PlaySound)
        {
            IsBackground = true
        };
        _t.Start();
    }

    public static void PlaySound()
    {
        using (SoundPlayer player = new SoundPlayer(Environment.CurrentDirectory + "/Assets/Editor/elevator.wav"))
        {
            player.PlaySync();
        }
        _t.Join();
    }
}

