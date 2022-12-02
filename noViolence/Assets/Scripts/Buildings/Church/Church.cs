using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ChurchUI;

public class Church : Building
{
    void Start()
    {
        buildingLevelMultiplier = buildingLevel * 1.3f;
        StartCoroutine("HandleChurchProduction");
    }

    void Update()
    {
        if (producing && uiManager.instance.ChurchScroll.activeInHierarchy)
        {
            if (productionTimer < productionTime)
                productionTimer += Time.deltaTime;
            else
                productionTimer = 0;

            churchUI.churchProgressBar.fillAmount = productionTimer / 10;
        }
    }

    void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (churchUI.WorkerScroll.activeInHierarchy)
                churchUI.WorkerScroll.SetActive(false);
            uiManager.instance.OpenChurchScroll();
            LoadChurchText();
            OpenProductionSroll();
        }
    }

    IEnumerator HandleChurchProduction()
    {
        if (workingUnits.Count > 0)
        {
            totalProdRate = 0;
            foreach (PlayerUnit u in workingUnits) { totalProdRate += 1.5f + ((1 * u.intLevel) / 10); print(totalProdRate); }

            totalProdRate *= buildingLevelMultiplier;
            if (totalProdRate > 0) producing = true;

            for (int i = 0; i < workingUnits.Count; i++)
            {
                PlayerManager.instance.WritingProgress += totalProdRate / 100;
                if (workingUnits[i].Health < workingUnits[i].MaxHealth)
                {
                    workingUnits[i].Health += totalProdRate / 3;
                }
            }
        }
        else
        {
            print("No units in Kitchen: " + bID);
            producing = false;
        }

        yield return new WaitForSeconds(productionTime);
        StartCoroutine("HandleChurchProduction");
    }

    public void LoadChurchText()
    {
        LoadTopPanel();
        churchUI.openUpgradePanel.onClick.RemoveAllListeners();
        churchUI.openUpgradePanel.onClick.AddListener(OpenChurchUpgradePanel);


        churchUI.churchTabSwitchButton.onClick.RemoveAllListeners();

        if (!churchUI.WorkerScroll.activeInHierarchy)
        {
            churchUI.churchTabSwitchButton.onClick.AddListener(OpenWorkerScroll);

        }
        else
            churchUI.churchTabSwitchButton.onClick.AddListener(OpenProductionSroll);
    }

    void LoadTopPanel()
    {
        churchUI.workerTxt.text = workingUnits.Count.ToString() + " / " + maxWorkingUnits;
        churchUI.upgradeLvlTxt.text = buildingLevel.ToString();
        churchUI.totalProdTxt.text = (totalProdRate / 100).ToString("F2");
        churchUI.totalHealText.text = (totalProdRate / 3).ToString("F2");
    }


    public void OpenChurchUpgradePanel()
    {
        if (!churchUI.upgradePanel.activeInHierarchy)
        {
            churchUI.upgradePanel.SetActive(true);
            churchUI.upgradeWoodPriceTxt.text = ((int)upgradeWoodPrice).ToString();
            churchUI.upgradeStonePriceTxt.text = ((int)upgradeStonePrice).ToString();
            churchUI.increaseBuildingLevelButton.onClick.RemoveAllListeners();
            churchUI.increaseBuildingLevelButton.onClick.AddListener(IncreaseBuildingLevel);

            LeanTween.scale(churchUI.upgradePanel, new Vector3(0, 0, 0), 0f);
            LeanTween.scale(churchUI.upgradePanel, new Vector3(1, 1, 1), 0.2f);
        }
        else
            LeanTween.scale(churchUI.upgradePanel, new Vector3(0, 0, 0), 0.2f).setOnComplete(() => churchUI.upgradePanel.SetActive(false));
    }

    public void OpenWorkerScroll()
    {
        if (!churchUI.WorkerScroll.activeInHierarchy)
        {
            churchUI.WorkerScroll.SetActive(true);
            churchUI.ProdScroll.SetActive(false);
            LoadChurchText();
            LoadWorkerPanels();
        }
    }

    public void OpenProductionSroll()
    {
        if (!churchUI.ProdScroll.activeInHierarchy)
        {
            churchUI.ProdScroll.SetActive(true);
            churchUI.WorkerScroll.SetActive(false);
            DestroyWorkerPanels();

            LoadChurchText();
        }
    }

    public void LoadWorkerPanels()
    {
        DestroyWorkerPanels();

        for (int i = 0; i < workingUnits.Count; i++)
        {
            WorkerPanel wPanel = Instantiate(churchUI.workerPanelPrefab, churchUI.WorkerPanel).GetComponent<WorkerPanel>();
            churchUI.WorkerPanels.Add(wPanel.gameObject);
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
        foreach (var workerPanel in churchUI.WorkerPanels)
        {
            Destroy(workerPanel.gameObject);
        }
    }



}
