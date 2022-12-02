using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FirstGearGames.SmoothCameraShaker;

public class uiManager : MonoBehaviour
{
    public static uiManager instance;
    void Awake() { instance = this; }

    [Header("MainPanel")]
    public GameObject MainPanel;
    public GameObject BuildScroll;
    public GameObject WarehouseScroll;
    public GameObject MainBuildingScroll;
    public GameObject KitchenScroll;
    public GameObject ChurchScroll;
    public GameObject MineScroll;

    [Header("MainBuilding")]
    public TextMeshProUGUI apostleAmount;
    public TextMeshProUGUI apostleMaxAmount;

    public Animator mainPanelAnimator;
    bool mainPanelOpened;
    GameObject ob;

    [Header("Shakes")]
    public ShakeData buildShake;

    public void OpenMainPanel()
    {
        DisableScrolls();

        if (!mainPanelOpened)
        {
            LeanTween.moveLocalY(MainPanel, -255, 0.7f).setEaseInOutBounce().setOnComplete(() =>
            { mainPanelOpened = true; });
        }
        else
        {
            CloseMainPanel();
        }
    }

    void CloseMainPanel()
    {
        LeanTween.moveLocalY(MainPanel, -850, 0.7f).setEaseOutBounce().setOnComplete(() => { mainPanelOpened = false; DisableScrolls(); });
    }

    public void OpenBuildScroll()
    {
        if (!BuildScroll.activeInHierarchy)
        {
            if (!mainPanelOpened)
                OpenMainPanel();

            DisableScrolls();
            BuildScroll.SetActive(true);
        }
        else
        {
            CloseMainPanel();
        }
    }

    public void OpenMainBuildingScroll()
    {
        if (!MainBuildingScroll.activeInHierarchy)
        {
            if (!mainPanelOpened)
                OpenMainPanel();

            DisableScrolls();
            MainBuildingScroll.SetActive(true);
        }
        else
        {
            CloseMainPanel();
        }
    }

    public void OpenWarehouseScroll()
    {
        if (!WarehouseScroll.activeInHierarchy)
        {
            if (!mainPanelOpened)
                OpenMainPanel();

            DisableScrolls();
            WarehouseScroll.SetActive(true);
            PlayerManager.instance.playerWarehouse.LoadResourceText();
        }
        else
        {
            CloseMainPanel();
        }
    }

    public void OpenKitchenScroll()
    {
        if (!KitchenScroll.activeInHierarchy)
        {
            if (!mainPanelOpened)
                OpenMainPanel();

            DisableScrolls();
            KitchenScroll.SetActive(true);
        }
        else
        {
            CloseMainPanel();
        }
    }

    public void OpenChurchScroll()
    {
        if (!ChurchScroll.activeInHierarchy)
        {
            if (!mainPanelOpened)
                OpenMainPanel();

            DisableScrolls();
            ChurchScroll.SetActive(true);
        }
        else
        {
            CloseMainPanel();
        }
    }

    public void OpenMineScroll()
    {
        if (!MineScroll.activeInHierarchy)
        {
            if (!mainPanelOpened)
                OpenMainPanel();

            DisableScrolls();
            MineScroll.SetActive(true);
        }
        else
        {
            CloseMainPanel();
        }
    }

    void DisableScrolls()
    {
        if (BuildScroll.activeInHierarchy)
            BuildScroll.SetActive(false);

        if (WarehouseScroll.activeInHierarchy)
            WarehouseScroll.SetActive(false);

        if (MainBuildingScroll.activeInHierarchy)
            MainBuildingScroll.SetActive(false);

        if (KitchenScroll.activeInHierarchy)
            KitchenScroll.SetActive(false);

        if (ChurchScroll.activeInHierarchy)
            ChurchScroll.SetActive(false);

        if (MineScroll.activeInHierarchy)
            MineScroll.SetActive(false);
    }


    void DisableObject(GameObject obj, float time)
    {
        ob = obj;
        Invoke("DelayDisabling", time);
    }

    void DelayDisabling()
    {
        if (ob.activeInHierarchy)
            ob.SetActive(false);
        else
            return;
    }
}
