using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    public bool callOnStart;
    public float timeToDestroy;

    void Start()
    {
        if(callOnStart == true)
        {
            DestroyIn(timeToDestroy);
        }
    }

    public void DestroyIn(float timeToDestroy)
    {
        StartCoroutine(CountDown(timeToDestroy));
    }

    IEnumerator CountDown(float value)
    {
        float normalizedTIme = 0;
        while(normalizedTIme <= 1)
        {
            normalizedTIme += Time.deltaTime / value;
            yield return null;
        }
        Destroy(gameObject);
    }
}
