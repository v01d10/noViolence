using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CombatProjectile : MonoBehaviour
{
    public void InvokeDisabling()
    {
        Invoke("DisableObject", 0.7f);
    }

    public void DisableObject()
    {
        gameObject.SetActive(false);
        print("Disabling projectile");
    }


}
