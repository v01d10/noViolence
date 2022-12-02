using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    AudioSource musicSource;
    int clipNumber;
    public int nexSongWaitTime;

    AudioClip tmpClip;
    public List<AudioClip> musicClips;


    void Start()
    {
        musicSource = GetComponent<AudioSource>();
        StartCoroutine("PlayMusic");
    }

    IEnumerator PlayMusic()
    {
        yield return new WaitForSecondsRealtime(nexSongWaitTime);
        clipNumber = Random.Range(2, musicClips.Count);
        musicSource.clip = musicClips[clipNumber];
        musicSource.Play();

        tmpClip = musicClips[clipNumber];
        musicClips.Remove(tmpClip);
        musicClips.Insert(0, tmpClip);
        StartCoroutine("PlayMusic");
    }
}
