using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceManager : MonoBehaviour
{
    AudioSource ambienceSource;
    
    public AudioClip forrestFrogs;

    void Awake()
    {
        ambienceSource = GetComponent<AudioSource>();
        ambienceSource.clip = forrestFrogs;
        ambienceSource.Play();
    }

    


}
