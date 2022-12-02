using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarTween : MonoBehaviour
{
    public LeanTweenType altarBounce;

    void Start()
    {
        LeanTween.moveLocalY(gameObject, transform.position.y+0.5f, 2).setEase(altarBounce).setLoopPingPong();
    }

}
