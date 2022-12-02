using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sfxManager : MonoBehaviour
{
    public static sfxManager instance;

    [Header("Clips")]
    public AudioClip clickSound;
    public AudioClip alertErrorSound;

    public AudioClip timeStopSound;
    public AudioClip timePlaySound;
    public AudioClip timeFasterSound;
    public AudioClip timeFastestSound;

    public AudioClip buildRotation;
    public AudioClip buildPlacement;
    public AudioClip buildComplete;
    public AudioClip[] treeChopings;


    void Awake()
    {
        instance = this;
    }

    public void PlayClickSound()
    {
        LeanAudio.play(clickSound);
    }

    public void PlayTimeStopSound()
    {
        LeanAudio.play(timeStopSound);
    }

    public void PlayTimePlaySound()
    {
        LeanAudio.play(timePlaySound);
    }

    public void PlayTimeFasterSound()
    {
        LeanAudio.play(timeFasterSound);
    }

    public void PlayTimeFastestSound()
    {
        LeanAudio.play(timeFastestSound);
    }

    public void PlayBuildRotateSound()
    {
        LeanAudio.play(buildRotation);
    }

    public void PlayBuildCompleteSound()
    {
        LeanAudio.play(buildComplete);
        
    }

    public void PlayErrorSound()
    {
        LeanAudio.play(alertErrorSound);
    }


}
