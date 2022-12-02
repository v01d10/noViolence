using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public enum ResType
    {
        Wood, Stone,
        Berry, Mushrooms, Wheat
    }
    public ResType rType;

    public float TotalAmount;
    public float MaxGatherers;
    public int placIndex;
    public List<UnitGathering> Gatherers;

    public void RemoveResource(float amount)
    {
        TotalAmount -= amount;
    }
}
