using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildingDetection : MonoBehaviour
{
    public Kitchen _selKitchen;
    PlayerUnit thisUnit;

    void Start()
    {
        thisUnit = GetComponentInParent<PlayerUnit>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Kitchen" && _selKitchen != null && _selKitchen.workingUnits.Contains(thisUnit))
        {
            thisUnit.unitModel.SetActive(false);
        }
    }
}
