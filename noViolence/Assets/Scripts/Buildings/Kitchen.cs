using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kitchen : MonoBehaviour
{
    public List<PlayerUnit> workingUnits;

    public int level;
    public float totalProdRate;

    IEnumerator ProduceFood()
    {
        if (workingUnits.Count > 0)
        {
            for (int i = 0; i < workingUnits.Count; i++)
            {
                PlayerManager.instance.playerWarehouse.foodAmount += totalProdRate;
                workingUnits[i].AddExperience(Random.Range(3, 13));

            }
        }

        yield return new WaitForSeconds(10);
        StartCoroutine(ProduceFood());
    }

}
