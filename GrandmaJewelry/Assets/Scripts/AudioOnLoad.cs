using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioOnLoad : MonoBehaviour
{
    public AudioClip audioOnActive;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioOnActive);
    }

    public void PlayAudio()
    {
        //if (audioSource.isPlaying)
        //{
        //    audioSource.Stop();
        //}
        GameObject.Find("Audio Source").GetComponent<AudioSource>().PlayOneShot(audioOnActive);
    }
}
