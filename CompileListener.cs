using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Compilation;
using System;
using System.Reflection;
using System.Media;
using System.Threading;
using UnityEditor.Callbacks;
using UnityEditorInternal;
using System.Linq;
using NUnit.Framework;
namespace ElevatorCompiler
{
    public class CompileListener : EditorWindow
    {
        private static Thread _thread;

        private const string MenuItemName = "Tools/Elevator Compiler/Active";
        private const string MenuItemEditor = "Tools/Elevator Compiler/Settings";
        private const string MenuMethodSwitchItemNameNative = "Tools/Elevator Compiler/CompileMethod/Native";
        private const string MenuMethodSwitchItemNameEditor = "Tools/Elevator Compiler/CompileMethod/Editor";
        private const string NativeModeInfoText = "Native mode uses windows libraries and may slow down your compile time a bit. But doesnt add anything to your scene.";
        private const string EditorModeInfoText = "Editor player mode is active, this will add a temporary object to your scene to play it,doesnt effect performance though, its very cool unless you find something getting changed in your scene as annoying.";
        private ReorderableList _list;

        [InitializeOnLoadMethod]
        public static void Loaded()
        {
            UpdateToggle();
        }

        [DidReloadScripts]
        public static void DidCompileFinished()
        {
            if (ElevatorSettings.UseWaitMusic)
                PlayerFactory.GetPlayer().CompileFinished();
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
        #endregion

        private static void TriggerSoundPlay()
        {
            PlayerFactory.GetPlayer().Play();
        }

        #region Quick Menu Controls
        [MenuItem(MenuItemName)]
        public static void ToggleWaitMusic()
        {
            ElevatorSettings.UseWaitMusic ^= true;
            UpdateToggle();
        }

        [MenuItem(MenuMethodSwitchItemNameEditor)]
        public static void ChooseEditorPlayer()
        {
            ElevatorSettings.IsNativeMode = false;
            Menu.SetChecked(MenuMethodSwitchItemNameEditor, true);
            Menu.SetChecked(MenuMethodSwitchItemNameNative, false);
        }

        [MenuItem(MenuMethodSwitchItemNameNative)]
        public static void ChooseNativePlayer()
        {
            ElevatorSettings.IsNativeMode = true;
            Menu.SetChecked(MenuMethodSwitchItemNameEditor, false);
            Menu.SetChecked(MenuMethodSwitchItemNameNative, true);
        }

        //Never call this on listeners. Will cause overlapping
        private static void UpdateToggle()
        {
            Menu.SetChecked(MenuItemName, ElevatorSettings.UseWaitMusic);


            Menu.SetChecked(MenuMethodSwitchItemNameEditor, !ElevatorSettings.IsNativeMode);
            Menu.SetChecked(MenuMethodSwitchItemNameNative, ElevatorSettings.IsNativeMode);

            if (ElevatorSettings.UseWaitMusic)
            {
                CompilationPipeline.assemblyCompilationStarted += CompilationPipeline_assemblyCompilationStarted;
                if (ElevatorSettings.PlayOnLightMapping)
                {
                    Lightmapping.started += Lightmapping_started;
                    Lightmapping.completed += DidCompileFinished;
                }
            }
            else
            {
                CompilationPipeline.assemblyCompilationStarted -= CompilationPipeline_assemblyCompilationStarted;
                Lightmapping.started -= Lightmapping_started;
                Lightmapping.completed -= DidCompileFinished;
            }

        }

        #endregion


        #region Editor Drawers
        [MenuItem(MenuItemEditor)]
        public static void DisplayEditorWindow()
        {
            EditorWindow _window = GetWindow<CompileListener>("Elevator Compiler", true);
            _window.Show();
        }
        private void OnEnable()
        {
            _list = new ReorderableList(SoundLibrary.GetAllSoundFileNames().ToList(), typeof(string), false, true, false, false);
            _list.drawHeaderCallback += (Rect r) =>
              {
                  EditorGUI.LabelField(r, "Sound Files");
              };
            _list.drawElementCallback = (rect, index, isActive, isFocus) =>
            {
                if (isFocus)
                    ElevatorSettings.DefaultTrackIndex = index;
                EditorGUI.LabelField(rect, SoundLibrary.GetAllSoundFileNames()[index]);
            };
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUIStyle _style = new GUIStyle();
            _style.fontSize = 30;
            _style.contentOffset = new Vector2(20f, 0f);
            EditorGUILayout.LabelField("Elevator Compiler Plugin", _style);
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("super useful edition", EditorStyles.centeredGreyMiniLabel);

            EditorGUILayout.EndVertical();

            ElevatorSettings.UseWaitMusic = EditorGUILayout.Toggle("Active:", ElevatorSettings.UseWaitMusic);
            if (ElevatorSettings.IsNativeMode)
            {
                EditorGUILayout.HelpBox(NativeModeInfoText, MessageType.Warning, true);
            }
            else
            {
                EditorGUILayout.HelpBox(EditorModeInfoText, MessageType.Info, true);
            }
            ElevatorSettings.IsNativeMode = EditorGUILayout.Toggle("Native Mode:", ElevatorSettings.IsNativeMode);

            if (ElevatorSettings.Shuffle)
                GUI.color = Color.green;
            if (GUILayout.Button(ElevatorSettings.Shuffle ? "Shuffling is ON" : "Shuffle is OFF"))
                ElevatorSettings.Shuffle ^= true;
            GUI.color = Color.white;

            if (!ElevatorSettings.Shuffle)
                EditorGUILayout.LabelField("You can choose a default soundtrack");
            if (_list != null)
                _list.DoLayoutList();

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("A tool by Mert Kirimgeri", EditorStyles.centeredGreyMiniLabel);
            EditorGUILayout.LabelField("Contributors", EditorStyles.centeredGreyMiniLabel);
            EditorGUILayout.LabelField("Liam Knightley(Ding sound and player prefs implementation PR)", EditorStyles.centeredGreyMiniLabel);
            EditorGUILayout.LabelField("Moe_Baker(Editor Player solution)", EditorStyles.centeredGreyMiniLabel);
            EditorGUILayout.LabelField("Nicholas Hutchind(sound suggestion)", EditorStyles.centeredGreyMiniLabel);
            EditorGUILayout.EndVertical();
        }
        #endregion

    }

}