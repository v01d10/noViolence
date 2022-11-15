using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sfxManager : MonoBehaviour
{
    public static sfxManager instance;

    public AudioSource sfxSource;

    public AudioClip buildPlacement;
    public AudioClip buildComplete;
    public AudioClip treeChoping;

    void Awake()
    {
        instance = this;
        sfxSource = GetComponent<AudioSource>();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.clip = clip;
        sfxSource.Play();
    }
}
