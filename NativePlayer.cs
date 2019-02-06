using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Media;
using System;

public class NativePlayer : AbstractPlayer, IPlayer
{
    private Thread _thread;
    public void Play()
    {
         _thread = new Thread(PlaySound)
        {
            IsBackground = true
        };
        _thread.Start();
    }

    public void PlaySound()
    {
        using (var player = new SoundPlayer($"{Environment.CurrentDirectory}/Assets/Editor/elevator.wav"))
        {
            player.PlaySync();
        }
        _thread.Join();
    }

    public void CompileFinished()
    {
    }

    public override void CleanUp()
    {
        
    }
}
