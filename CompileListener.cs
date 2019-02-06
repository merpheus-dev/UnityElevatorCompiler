using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Compilation;
using System;
using System.Reflection;
using System.Media;
using System.Threading;
namespace ElevatorCompiler
{
    public class CompileListener : EditorWindow
    {
        private static Thread _thread;

        private const string MenuItemName = "Tools/Elevator Compiler";
        private const string MenuMethodSwitchItemNameNative = "Tools/CompileMethod/Native";
        private const string MenuMethodSwitchItemNameEditor = "Tools/CompileMethod/Editor";
        private const string UseWaitMusicKey = "EDITOR_UseWaitMusic";
        private const string PlayMethodKey = "EDITOR_MusicPlayMethod";
        private static bool _useWaitMusic;

        [InitializeOnLoadMethod]
        public static void Loaded()
        {
            _useWaitMusic = EditorPrefs.GetBool(UseWaitMusicKey, true);
            UpdateToggle();
        }

        #region CallBack Listeners
        private static void CompilationPipeline_assemblyCompilationStarted(string obj)
        {
            TriggerSoundPlay();
        }

        private static void Lightmapping_started()
        {
            TriggerSoundPlay();
        }


        private static void CompilationPipeline_assemblyCompilationFinished(string arg1, CompilerMessage[] arg2)
        {
            TriggerCompileFinished();
        }
        #endregion

        private static void TriggerSoundPlay()
        {
            PlayerFactory.GetPlayer().Play();
        }

        public static void TriggerCompileFinished()
        {
            PlayerFactory.GetPlayer().CompileFinished();
        }

        #region Quick Menu Controls
        [MenuItem(MenuItemName)]
        public static void ToggleWaitMusic()
        {
            _useWaitMusic = !_useWaitMusic;
            UpdateToggle();
        }

        [MenuItem(MenuMethodSwitchItemNameEditor)]
        public static void ChooseEditorPlayer()
        {
            EditorPrefs.SetInt(PlayMethodKey, 1);
            Menu.SetChecked(MenuMethodSwitchItemNameEditor, true);
            Menu.SetChecked(MenuMethodSwitchItemNameNative, false);
        }

        [MenuItem(MenuMethodSwitchItemNameNative)]
        public static void ChooseNativePlayer()
        {
            EditorPrefs.SetInt(PlayMethodKey, 0);
            Menu.SetChecked(MenuMethodSwitchItemNameEditor, false);
            Menu.SetChecked(MenuMethodSwitchItemNameNative, true);
        }

        //Never call this on listeners. Will cause overlapping
        private static void UpdateToggle()
        {
            EditorPrefs.SetBool(UseWaitMusicKey, _useWaitMusic);
            Menu.SetChecked(MenuItemName, _useWaitMusic);

            if (!EditorPrefs.HasKey(PlayMethodKey))
                EditorPrefs.SetInt(PlayMethodKey, 0);

            bool isEditorPlayer = EditorPrefs.GetInt(PlayMethodKey) == 1;
            Menu.SetChecked(MenuMethodSwitchItemNameEditor, isEditorPlayer);
            Menu.SetChecked(MenuMethodSwitchItemNameNative, !isEditorPlayer);

            if (_useWaitMusic)
            {
                CompilationPipeline.assemblyCompilationStarted += CompilationPipeline_assemblyCompilationStarted;
                CompilationPipeline.assemblyCompilationFinished += CompilationPipeline_assemblyCompilationFinished;
                Lightmapping.started += Lightmapping_started;
            }
            else
            {
                CompilationPipeline.assemblyCompilationStarted -= CompilationPipeline_assemblyCompilationStarted;
                Lightmapping.started -= Lightmapping_started;
            }

        }

        #endregion

    }

}