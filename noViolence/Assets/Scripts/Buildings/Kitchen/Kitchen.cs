using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static KitchenUI;

public class Kitchen : Building
{
    Warehouse pWarehouse;

    void Start()
    {
        buildingLevelMultiplier = buildingLevel * 1.3f;
        pWarehouse = PlayerManager.instance.playerWarehouse;
        StartCoroutine("ProduceFood");
    }

    void Update()
    {
        if (producing && uiManager.instance.KitchenScroll.activeInHierarchy)
        {
            if (productionTimer < productionTime)
                productionTimer += Time.deltaTime;
            else
                productionTimer = 0;

            kitchenUI.kitchenProgressBar.fillAmount = productionTimer / 10;
        }
    }

    void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (kitchenUI.WorkerScroll.activeInHierarchy)
                kitchenUI.WorkerScroll.SetActive(false);
            uiManager.instance.OpenKitchenScroll();
            LoadKitchenText();
            OpenProductionSroll();
        }
    }

    IEnumerator ProduceFood()
    {
        if (workingUnits.Count > 0)
        {
            totalProdRate = 0;
            foreach (PlayerUnit u in workingUnits) { totalProdRate += 1.3f + ((1 * u.skillLevel) / 10); print(totalProdRate);}

            totalProdRate *= buildingLevelMultiplier;
            
            for (int i = 0; i < workingUnits.Count; i++)
            {
                if (pWarehouse.shroomAmount > totalProdRate * (totalProdRate * 1.3f))
                {
                    if(!producing && totalProdRate > 0) producing = true;
                    print("Cooking from shrooms: " + bID + "Working units: " + workingUnits.Count);
                    pWarehouse.shroomAmount -= (totalProdRate * 1.3f);
                    pWarehouse.foodAmount += totalProdRate;

                    workingUnits[i].AddExperience((int)workingUnits[i].expNeeded / 666);
                }
                else
                {
                    if (pWarehouse.potatoAmount > totalProdRate * (totalProdRate * 1.3f))
                    {
                        if(!producing && totalProdRate > 0) producing = true;
                        print("Cooking from potato: " + bID + "Working units: " + workingUnits.Count);
                        pWarehouse.potatoAmount -= (totalProdRate * 1.3f);
                        pWarehouse.foodAmount += totalProdRate;

                        workingUnits[i].AddExperience((int)workingUnits[i].expNeeded / 666);
                    }
                    else
                    {
                        if (pWarehouse.wheatAmount > totalProdRate * (totalProdRate * 1.3f))
                        {
                            if(!producing && totalProdRate > 0) producing = true;
                            print("Cooking from wheat: " + bID + "Working units: " + workingUnits.Count);
                            pWarehouse.wheatAmount -= (totalProdRate * 1.3f);
                            pWarehouse.foodAmount += totalProdRate;

                            workingUnits[i].AddExperience((int)workingUnits[i].expNeeded / 666);
                        }
                    }
                }
            }
        }
        else
        {
            print("No units in Kitchen: " + bID);
            producing = false;
        }


        yield return new WaitForSeconds(productionTime);
        StartCoroutine("ProduceFood");
    }

    public void LoadKitchenText()
    {
        LoadTopPanel();
        kitchenUI.openUpgradePanel.onClick.RemoveAllListeners();
        kitchenUI.openUpgradePanel.onClick.AddListener(OpenKitchenUpgradePanel);


        kitchenUI.KitchenTabSwitchButton.onClick.RemoveAllListeners();

        if (!kitchenUI.WorkerScroll.activeInHierarchy)
        {
            kitchenUI.KitchenTabSwitchButton.onClick.AddListener(OpenWorkerScroll);

        }
        else
            kitchenUI.KitchenTabSwitchButton.onClick.AddListener(OpenProductionSroll);
    }

    void LoadTopPanel()
    {
        kitchenUI.workerTxt.text = workingUnits.Count.ToString() + " / " + maxWorkingUnits;
        kitchenUI.upgradeLvlTxt.text = buildingLevel.ToString();
        kitchenUI.totalProdTxt.text = ((int)totalProdRate).ToString();
    }


    public void OpenKitchenUpgradePanel()
    {
        if (!kitchenUI.upgradePanel.activeInHierarchy)
        {
            kitchenUI.upgradePanel.SetActive(true);
            kitchenUI.upgradeWoodPriceTxt.text = ((int)upgradeWoodPrice).ToString();
            kitchenUI.upgradeStonePriceTxt.text = ((int)upgradeStonePrice).ToString();
            kitchenUI.increaseBuildingLevelButton.onClick.RemoveAllListeners();
            kitchenUI.increaseBuildingLevelButton.onClick.AddListener(IncreaseBuildingLevel);

            LeanTween.scale(kitchenUI.upgradePanel, new Vector3(0, 0, 0), 0f);
            LeanTween.scale(kitchenUI.upgradePanel, new Vector3(1, 1, 1), 0.3f);
        }
        else
            LeanTween.scale(kitchenUI.upgradePanel, new Vector3(0, 0, 0), 0.3f).setOnComplete(() => kitchenUI.upgradePanel.SetActive(false));
    }

    public void OpenWorkerScroll()
    {
        if (!kitchenUI.WorkerScroll.activeInHierarchy)
        {
            kitchenUI.WorkerScroll.SetActive(true);
            kitchenUI.ProdScroll.SetActive(false);
            LoadKitchenText();
            LoadWorkerPanels();
        }
    }

    public void OpenProductionSroll()
    {
        if (!kitchenUI.ProdScroll.activeInHierarchy)
        {
            kitchenUI.ProdScroll.SetActive(true);
            kitchenUI.WorkerScroll.SetActive(false);
            DestroyWorkerPanels();

            LoadKitchenText();
        }
    }

    public void LoadWorkerPanels()
    {
        DestroyWorkerPanels();

        for (int i = 0; i < workingUnits.Count; i++)
        {
            WorkerPanel wPanel = Instantiate(kitchenUI.workerPanelPrefab, kitchenUI.WorkerPanel).GetComponent<WorkerPanel>();
            kitchenUI.WorkerPanels.Add(wPanel.gameObject);
            wPanel.workerUsingThisButton = workingUnits[i].gameObject;

            wPanel.gameObject.SetActive(true);
            wPanel.NameTxt.text = workingUnits[i].Name;
            wPanel.IntTxt.text = workingUnits[i].intLevel.ToString();
            wPanel.StrTxt.text = workingUnits[i].strLevel.ToString();
            wPanel.SkllTxt.text = workingUnits[i].skillLevel.ToString();

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
        foreach (var workerPanel in kitchenUI.WorkerPanels)
        {
            Destroy(workerPanel.gameObject);
        }
    }


}
