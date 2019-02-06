using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
public class EnginePlayer : IPlayer
{
    private AudioSource audioSource;
    private Scene scene; 
    public void Play()
    {
        AudioClip clip = Resources.Load<AudioClip>("elevator.wav");
        scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
        if (audioSource != null)
            MonoBehaviour.DestroyImmediate(audioSource.gameObject);
        audioSource = new GameObject("ElevatorMusicPlayer").AddComponent<AudioSource>();
        EditorSceneManager.MoveGameObjectToScene(audioSource.gameObject, scene);
        if (!audioSource.isPlaying)
        {
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void CompileFinished()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
            MonoBehaviour.DestroyImmediate(audioSource.gameObject);
        }
    }
}
