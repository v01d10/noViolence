using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kitchen : MonoBehaviour
{
    public List<PlayerUnit> workingUnits;

    public int bID;
    public int maxWorkingUnits;
    public int level;
    public float maxTotalProdRate;
    public float totalProdRate;
    public float food1Rate;
    public float food2Rate;
    public float food3Rate;

    void Start()
    {
        StartCoroutine("ProduceFood");
    }

    IEnumerator ProduceFood()
    {
        if (workingUnits.Count > 0)
        {
            print("Producing food! Kitchen : " + bID + "Working units: " + workingUnits.Count);
            for (int i = 0; i < workingUnits.Count; i++)
            {
                PlayerManager.instance.playerWarehouse.foodAmount += totalProdRate;
                workingUnits[i].AddExperience(Random.Range(3, 13));
            }
        }
        else
        {
            print("No units in Kitchen: " + bID);
        }

        yield return new WaitForSeconds(10);
        StartCoroutine(ProduceFood());
    }

}
