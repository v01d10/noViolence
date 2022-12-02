using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static MineUI;

public class Mine : Building
{
    Warehouse pWarehouse;

    void Start()
    {
        buildingLevelMultiplier = buildingLevel * 1.3f;
        StartCoroutine("HandleMineProduction");
    }

    void Update()
    {
        if (producing && uiManager.instance.MineScroll.activeInHierarchy)
        {
            if (productionTimer < productionTime)
                productionTimer += Time.deltaTime;
            else
                productionTimer = 0;

            mineUI.MineProgressBar.fillAmount = productionTimer / 10;
        }
    }

    void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (mineUI.WorkerScroll.activeInHierarchy)
                mineUI.WorkerScroll.SetActive(false);
            uiManager.instance.OpenMineScroll();
            LoadMineText();
            OpenProductionSroll();
        }
    }

    IEnumerator HandleMineProduction()
    {
        if (workingUnits.Count > 0)
        {
            totalProdRate = 0;
            foreach (PlayerUnit u in workingUnits) { totalProdRate += 1.5f + ((1 * u.intLevel) / 10); print(totalProdRate); }

            totalProdRate *= buildingLevelMultiplier;
            if (totalProdRate > 0) producing = true;

            for (int i = 0; i < workingUnits.Count; i++)
            {
                if (pWarehouse.stoneAmount < pWarehouse.stoneMaxAmount)
                {
                    if (!producing && totalProdRate > 0) producing = true;
                    print("Mining stone: " + bID + "Working units: " + workingUnits.Count);

                    pWarehouse.stoneAmount += (totalProdRate * 0.7f);

                    workingUnits[i].AddExperience((int)workingUnits[i].expNeeded / 666);
                }
            }
        }
        else
        {
            print("No units in Mine: " + bID);
            producing = false;
        }

        yield return new WaitForSeconds(productionTime);
        StartCoroutine("HandleMineProduction");
    }

    public void LoadMineText()
    {
        LoadTopPanel();
        mineUI.openUpgradePanel.onClick.RemoveAllListeners();
        mineUI.openUpgradePanel.onClick.AddListener(OpenMineUpgradePanel);


        mineUI.MineTabSwitchButton.onClick.RemoveAllListeners();

        if (!mineUI.WorkerScroll.activeInHierarchy)
        {
            mineUI.MineTabSwitchButton.onClick.AddListener(OpenWorkerScroll);

        }
        else
            mineUI.MineTabSwitchButton.onClick.AddListener(OpenProductionSroll);
    }

    void LoadTopPanel()
    {
        mineUI.workerTxt.text = workingUnits.Count.ToString() + " / " + maxWorkingUnits;
        mineUI.upgradeLvlTxt.text = buildingLevel.ToString();
        mineUI.totalProdTxt.text = (totalProdRate / 100).ToString("F2");
    }


    public void OpenMineUpgradePanel()
    {
        if (!mineUI.upgradePanel.activeInHierarchy)
        {
            mineUI.upgradePanel.SetActive(true);
            mineUI.upgradeWoodPriceTxt.text = ((int)upgradeWoodPrice).ToString();
            mineUI.upgradeStonePriceTxt.text = ((int)upgradeStonePrice).ToString();
            mineUI.increaseBuildingLevelButton.onClick.RemoveAllListeners();
            mineUI.increaseBuildingLevelButton.onClick.AddListener(IncreaseBuildingLevel);

            LeanTween.scale(mineUI.upgradePanel, new Vector3(0, 0, 0), 0f);
            LeanTween.scale(mineUI.upgradePanel, new Vector3(1, 1, 1), 0.2f);
        }
        else
            LeanTween.scale(mineUI.upgradePanel, new Vector3(0, 0, 0), 0.2f).setOnComplete(() => mineUI.upgradePanel.SetActive(false));
    }

    public void OpenWorkerScroll()
    {
        if (!mineUI.WorkerScroll.activeInHierarchy)
        {
            mineUI.WorkerScroll.SetActive(true);
            mineUI.ProdScroll.SetActive(false);
            LoadMineText();
            LoadWorkerPanels();
        }
    }

    public void OpenProductionSroll()
    {
        if (!mineUI.ProdScroll.activeInHierarchy)
        {
            mineUI.ProdScroll.SetActive(true);
            mineUI.WorkerScroll.SetActive(false);
            DestroyWorkerPanels();

            LoadMineText();
        }
    }

    public void LoadWorkerPanels()
    {
        DestroyWorkerPanels();

        for (int i = 0; i < workingUnits.Count; i++)
        {
            WorkerPanel wPanel = Instantiate(mineUI.workerPanelPrefab, mineUI.WorkerPanel).GetComponent<WorkerPanel>();
            mineUI.WorkerPanels.Add(wPanel.gameObject);
            wPanel.workerUsingThisButton = workingUnits[i].gameObject;

            wPanel.gameObject.SetActive(true);
            wPanel.NameTxt.text = workingUnits[i].Name;
            wPanel.IntTxt.text = workingUnits[i].intLevel.ToString();
            wPanel.StrTxt.text = workingUnits[i].strLevel.ToString();
            wPanel.SkllTxt.text = workingUnits[i].intLevel.ToString();

            wPanel.GetComponent<Button>().onClick.RemoveAllListeners();
            wPanel.GetComponent<Button>().onClick.AddListener(() =>
            {
                wPanel.workerUsingThisButton.GetComponent<PlayerUnit>().unitModel.SetActive(true);
                wPanel.workerUsingThisButton.GetComponent<PlayerUnit>().CancelAllActions();
                Destroy(wPanel.gameObject);
                print("Worker panel destroyed");
            });
        }
    }

    public void DestroyWorkerPanels()
    {
        foreach (var workerPanel in mineUI.WorkerPanels)
        {
            Destroy(workerPanel.gameObject);
        }
    }



}
