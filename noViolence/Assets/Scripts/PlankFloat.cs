using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankFloat : MonoBehaviour
{
    public LeanTweenType floatEase;

    void OnEnable()
    {
        LeanTween.moveLocalY(gameObject, transform.position.y+0.2f, 2.2f).setLoopPingPong().setEase(floatEase);
    }
}
