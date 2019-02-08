using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
namespace ElevatorCompiler
{
    public static class SoundLibrary
    {
        private static List<AudioClip> soundClips;
        private static string[] soundNames;
        public const string SoundBankLocation = "Assets/Editor/Resources/Elevator/Playlist";
        static SoundLibrary()
        {
            //For Editor mode
            AudioClip[] clips = Resources.LoadAll<AudioClip>("Elevator/Playlist");
            if (clips.Length > 0)
                soundClips = clips.ToList();
            else
                throw new System.NullReferenceException("No sound file detected for Elevator Compiler");

            //For native mode
            if (!Directory.Exists(string.Format("{0}/{1}", System.Environment.CurrentDirectory, SoundBankLocation)))
                throw new System.NullReferenceException("PlayList folder not found!", new System.Exception("Please make sure you have" + SoundBankLocation));

            soundNames = Directory.GetFiles(SoundBankLocation, "*.wav");
            if (soundNames.Length == 0)
                throw new System.NullReferenceException("No sound file detected for Elevator Compiler");
        }

        public static string[] GetAllSoundFileNames()
        {
            return soundNames;
        }

        public static AudioClip GetSoundClip()
        {
            if (!ElevatorSettings.Shuffle)
            {
                if (ElevatorSettings.DefaultTrackIndex >= soundNames.Length)
                    throw new System.Exception("Elevator Compiler Error,your playlist changes, if you wanna continue using a single track, assign your default sound again");
                return soundClips[ElevatorSettings.DefaultTrackIndex];
            }
            return soundClips[Random.Range(0, soundClips.Count)];
        }

        public static string GetSoundName()
        {
            if (!ElevatorSettings.Shuffle)
            {
                if (ElevatorSettings.DefaultTrackIndex >= soundNames.Length)
                    throw new System.Exception("Elevator Compiler Error,your playlist changes, if you wanna continue using a single track, assign your default sound again");
                return soundNames[ElevatorSettings.DefaultTrackIndex];
            }
            System.Random rnd = new System.Random();
            return soundNames[rnd.Next(0, soundNames.Length)];
        }

    }
}