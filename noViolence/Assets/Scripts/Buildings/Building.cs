using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static KitchenUI;

public class Building : MonoBehaviour
{
    public int bID;
    public float productionTime;
    public float productionTimer;

    public float buildingLevel;
    public float buildingLevelMultiplier;
    public int maxWorkingUnits;

    [Header("Upgrade")]
    public float upgradeWoodPrice;
    public float upgradeStonePrice;

    [Header("Rates")]
    public float totalProdRate;
    public float totalProdRatePerc;

    // public float prod1Rate;
    // public float prod1Perc;
    // public float prod2Rate;
    // public float prod2Perc;
    // public float prod3Rate;
    // public float prod3Perc;

    public Vector3 position;
    public bool producing;

    public List<PlayerUnit> workingUnits;
    public pBuildingSaveData pBuildingData;


    public void IncreaseBuildingLevel()
    {
        if (PlayerManager.instance.playerWarehouse.woodAmount >= upgradeWoodPrice && PlayerManager.instance.playerWarehouse.stoneAmount >= upgradeStonePrice)
        {
            PlayerManager.instance.playerWarehouse.woodAmount -= upgradeWoodPrice;
            PlayerManager.instance.playerWarehouse.stoneAmount -= upgradeStonePrice;

            buildingLevel++;
            buildingLevelMultiplier = buildingLevel * 1.3f;
            if (buildingLevel == 10) maxWorkingUnits++;
            if (buildingLevel == 15) maxWorkingUnits++;

            upgradeStonePrice *= 1.6f;
            upgradeWoodPrice *= 1.6f;

            LeanTween.scale(kitchenUI.upgradePanel, new Vector3(0, 0, 0), 0.3f).setOnComplete(() => kitchenUI.upgradePanel.SetActive(false));
            print("Kitchen upgraded");
            sfxManager.instance.PlayBuildCompleteSound();

        }
        else
        {
            if (PlayerManager.instance.playerWarehouse.stoneAmount < upgradeWoodPrice)
            {
                print("Not enough wood");
                Notifier.instance.Notify("Not enough wood!");
                sfxManager.instance.PlayErrorSound();
            }
            if (PlayerManager.instance.playerWarehouse.stoneAmount < upgradeStonePrice)
            {
                print("Not enough stone");
                Notifier.instance.Notify("Not enough stone!");
                sfxManager.instance.PlayErrorSound();
            }

        }
    }

    public void SaveState()
    {
        if (GetComponentInParent<PlayerBuildingHolder>().pBuildingData.Contains(pBuildingData))
        {

            int indexOf = GetComponentInParent<PlayerBuildingHolder>().pBuildingData.IndexOf(pBuildingData);

            if (GetComponentInParent<PlayerBuildingHolder>().pBuildingData[indexOf] != null)
            {
                pBuildingSaveData pbDta = PlayerBuildingHolder.instance.pBuildingData[indexOf];

                pbDta._bID = this.bID;
                pbDta._productionTime = this.productionTime;
                pbDta._productionTimer = this.productionTimer;
                pbDta._buildingLevel = this.buildingLevel;
                pbDta._buildingLevelMultiplier = this.buildingLevelMultiplier;
                pbDta._maxWorkingUnits = this.maxWorkingUnits;
                pbDta._upgradeWoodPrice = this.upgradeWoodPrice;
                pbDta._upgradeStonePrice = this.upgradeStonePrice;
                pbDta._totalProdRate = this.totalProdRate;
                pbDta._totalProdRatePerc = this.totalProdRatePerc;
                pbDta._producing = this.producing;
                pbDta._workingUnits = this.workingUnits;

                pbDta._position = this.transform.position;
            }
        }
        else
        {
            pBuildingData = new pBuildingSaveData();

            pBuildingData._bID = this.bID;
            pBuildingData._productionTime = this.productionTime;
            pBuildingData._productionTimer = this.productionTimer;
            pBuildingData._buildingLevel = this.buildingLevel;
            pBuildingData._buildingLevelMultiplier = this.buildingLevelMultiplier;
            pBuildingData._maxWorkingUnits = this.maxWorkingUnits;
            pBuildingData._upgradeWoodPrice = this.upgradeWoodPrice;
            pBuildingData._upgradeStonePrice = this.upgradeStonePrice;
            pBuildingData._totalProdRate = this.totalProdRate;
            pBuildingData._totalProdRatePerc = this.totalProdRatePerc;
            pBuildingData._producing = this.producing;
            pBuildingData._workingUnits = this.workingUnits;

            pBuildingData._position = this.transform.position;

            PlayerBuildingHolder.instance.pBuildingData.Add(pBuildingData);
            print(GetComponentInParent<PlayerBuildingHolder>().pBuildingData.Count);

        }

    }

    public void LoadState(pBuildingSaveData _pBuildingData)
    {
        Building kitchen = Instantiate(PlayerBuildManager.instance.kitchenPrefab, PlayerBuildingHolder.instance.transform).GetComponent<Building>();
        kitchen.bID = _pBuildingData._bID;
        kitchen.productionTime = _pBuildingData._productionTime;
        kitchen.productionTimer = _pBuildingData._productionTimer;
        kitchen.buildingLevel = _pBuildingData._buildingLevel;
        kitchen.buildingLevelMultiplier = _pBuildingData._buildingLevelMultiplier;
        kitchen.maxWorkingUnits = _pBuildingData._maxWorkingUnits;
        kitchen.upgradeWoodPrice = _pBuildingData._upgradeWoodPrice;
        kitchen.upgradeStonePrice = _pBuildingData._upgradeStonePrice;
        kitchen.totalProdRate = _pBuildingData._totalProdRate;
        kitchen.totalProdRatePerc = _pBuildingData._totalProdRatePerc;
        kitchen.producing = _pBuildingData._producing;
        kitchen.workingUnits = _pBuildingData._workingUnits;

        kitchen.transform.position = _pBuildingData._position;


    }

}

[System.Serializable]
public class pBuildingSaveData
{
    public int _bID;
    public float _productionTime;
    public float _productionTimer;

    public float _buildingLevel;
    public float _buildingLevelMultiplier;
    public int _maxWorkingUnits;

    public float _upgradeWoodPrice;
    public float _upgradeStonePrice;

    public float _totalProdRate;
    public float _totalProdRatePerc;

    public bool _producing;

    public Vector3 _position;

    public List<PlayerUnit> _workingUnits;

}

