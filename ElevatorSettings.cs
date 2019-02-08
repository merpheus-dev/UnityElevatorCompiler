using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace ElevatorCompiler
{
    public static class ElevatorSettings
    {
        private const string UseWaitMusicKey = "EDITOR_UseWaitMusic";
        private const string PlayMethodKey = "EDITOR_MusicPlayMethod";
        private const string ShuffleKey = "EDITOR_ShouldShuffle";
        private const string DefaultTrackKey = "EDITOR_DefaultTrack";
        private const string LightMappingKey = "EDITOR_LightMap";

        public static bool IsNativeMode
        {
            get
            {
                if (!EditorPrefs.HasKey(PlayMethodKey))
                    EditorPrefs.SetInt(PlayMethodKey, 1);

                return EditorPrefs.GetInt(PlayMethodKey) == 0;
            }
            set
            {
                EditorPrefs.SetInt(PlayMethodKey, value ? 0 : 1);
            }
        }
        public static bool UseWaitMusic
        {
            get
            {
                if (!EditorPrefs.HasKey(UseWaitMusicKey))
                    EditorPrefs.SetBool(UseWaitMusicKey, true);
                return EditorPrefs.GetBool(UseWaitMusicKey);
            }
            set
            {
                EditorPrefs.SetBool(UseWaitMusicKey, value);
            }
        }

        public static bool Shuffle
        {
            get
            {
                if (!EditorPrefs.HasKey(ShuffleKey))
                    EditorPrefs.SetBool(ShuffleKey, true);
                return EditorPrefs.GetBool(ShuffleKey);
            }
            set
            {
                EditorPrefs.SetBool(ShuffleKey, value);
            }
        }

        public static int DefaultTrackIndex
        {
            get
            {
                if (!EditorPrefs.HasKey(DefaultTrackKey))
                    EditorPrefs.SetInt(DefaultTrackKey, 0);
                return EditorPrefs.GetInt(DefaultTrackKey);
            }
            set
            {
                EditorPrefs.SetInt(DefaultTrackKey, value);
            }
        }

        public static bool PlayOnLightMapping
        {
            get
            {
                if (!EditorPrefs.HasKey(LightMappingKey))
                    EditorPrefs.SetBool(LightMappingKey, false);
                return EditorPrefs.GetBool(LightMappingKey);
            }
            set
            {
                EditorPrefs.SetBool(LightMappingKey, value);
            }
        }
    }
}