using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildManager : MonoBehaviour
{
    public static PlayerBuildManager instance;
    void Awake() { instance = this; }

    Warehouse whouse;

    [Header("Prefabs")]
    public GameObject mainTentPrefab;
    public GameObject kitchenPrefab;

    public int mTentTime, kitchenTime;

    [Header("Prices")]
    public int mTentWprice, mTentSprice;
    public int kitchenWprice, kitchenSprice;

    public bool playerBuilding;


    public void BuildMainTent()
    {
        whouse = PlayerManager.instance.playerWarehouse;

        if (whouse.woodAmount >= mTentWprice && whouse.stoneAmount >= mTentSprice)
            SpawnPrefab(mainTentPrefab, mTentTime);
        else
        {
            print("Not enough resources...");
            Notifier.instance.Notify("Not enough resources...");
            sfxManager.instance.PlayErrorSound();
        }
    }

    public void BuildKitchen()
    {
        whouse = PlayerManager.instance.playerWarehouse;

        if (whouse.woodAmount >= kitchenWprice && whouse.stoneAmount >= kitchenSprice)
            SpawnPrefab(mainTentPrefab, kitchenTime);
        else
        {
            print("Not enough resources...");
            Notifier.instance.Notify("Not enough resources...");
            sfxManager.instance.PlayErrorSound();
        }
    }

    public void SpawnPrefab(GameObject prefab, int time)
    {
        playerBuilding = true;
        uiManager.instance.OpenBuildScroll();
        prefab = Instantiate(prefab, transform, worldPositionStays: true);
        prefab.GetComponentInChildren<PlayerBuildPrefab>().buildTime = time;
    }


}
