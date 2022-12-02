using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildingDetection : MonoBehaviour
{
    PlayerUnit thisUnit;
    float distanceToSelBuilding;

    public Kitchen _selKitchen;
    public Church _selChurch;

    void Start()
    {
        thisUnit = GetComponentInParent<PlayerUnit>();
    }

    public void StartBuildingDetection()
    {
        StartCoroutine("BuildingDetection");
    }

    IEnumerator BuildingDetection()
    {
        if (_selKitchen != null)
        {
            distanceToSelBuilding = Vector3.Distance(transform.position, _selKitchen.transform.position);

            if (distanceToSelBuilding < 1f)
            {
                thisUnit.unitModel.SetActive(false);
                yield return null;
            }
            else
            {
                yield return new WaitForSecondsRealtime(0.3f);
                StartCoroutine("BuildingDetection");

            }
        }
    }
}
