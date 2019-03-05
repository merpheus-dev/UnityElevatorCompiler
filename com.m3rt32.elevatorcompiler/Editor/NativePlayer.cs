using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Media;
using System;
using UnityEditor;
namespace ElevatorCompiler
{
    public class NativePlayer : AbstractPlayer, IPlayer
    {
        private static Thread _thread;
        private static SoundPlayer _player;
        public void Play()
        {
            SoundLibrary.GetSoundName();
            _thread = new Thread(PlaySound);
            _thread.Start();
        }

        public void PlaySound()
        {
            using (var player = new SoundPlayer(SoundLibrary.GetSoundName()))
            {
                player.PlaySync();
            }
            _thread.Join();
        }

        public void CompileFinished()
        {
            _player = new SoundPlayer(string.Format("{0}/{1}/ding.wav",Environment.CurrentDirectory,SoundLibrary.SoundBankLocation));
            _player.Play();
            _player.StreamChanged += _player_StreamChanged;


        }

        private void _player_StreamChanged(object sender, EventArgs e)
        {
            _player.StreamChanged -= _player_StreamChanged;
            scheduledTime = (float)EditorApplication.timeSinceStartup + _player.Stream.Length;
        }

        public override void CleanUp()
        {
            _player.Dispose();
            scheduledTime = 0f;
        }
    }

}