using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuild : MonoBehaviour
{
    Warehouse whouse;
    public GameObject mainTentPrefab;

    public int mTentTime;


    public int mTentWprice, mTentSprice;
    public int kitchenWprice, kitchenSprice;


    public void BuildMainTent()
    {
        whouse = PlayerManager.instance.playerWarehouse;

        if(whouse.woodAmount >= mTentWprice && whouse.stoneAmount >= mTentSprice)
            SpawnPrefab(mainTentPrefab, mTentTime);
        else {print ("Not enough resources...");}
    }

    public void SpawnPrefab(GameObject prefab, int time)
    {
        uiManager.instance.OpenBuildScroll();
        Instantiate(prefab, transform);
        PlayerManager.instance.playerBuilding = true;
        prefab.GetComponent<PlayerBuildPrefab>().buildTime = time;
    }


}
