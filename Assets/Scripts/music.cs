using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class music : MonoBehaviour
{
    public static AudioSource audioSource;
    public static AudioClip startAudio;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        startAudio = Resources.Load<AudioClip>("Start");
    }

    public void StartMusic()
    {
        audioSource.Play();
    }

    public void CloseMusic()
    {
        audioSource.Stop();
    }

}
