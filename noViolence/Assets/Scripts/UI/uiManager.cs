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

    [Header("MainBuilding")]
    public TextMeshProUGUI apostleAmount;
    public TextMeshProUGUI apostleMaxAmount;

    [Header("Warehouse")]
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI stoneText;
    public TextMeshProUGUI berriesText;
    public TextMeshProUGUI MushroomsText;
    public TextMeshProUGUI WheatText;

    public Animator mainPanelAnimator;
    bool mainPanelOpened;
    GameObject ob;

    [Header("Shakes")]
    public ShakeData buildShake;
    
    void Start()
    {
        mainPanelAnimator = GetComponent<Animator>();
    }
    void OpenMainPanel()
    {
        if (!mainPanelOpened)
        {
            mainPanelAnimator.Play("mainPanelOpen");
            mainPanelOpened = true;
        }
        else
        {
            CloseMainPanel();
        }
    }

    void CloseMainPanel()
    {
        mainPanelAnimator.Play("mainPanelClose");
        mainPanelOpened = false;
    }

    public void OpenBuildScroll()
    {
        if (!BuildScroll.activeInHierarchy)
        {
            if(!mainPanelOpened)
                OpenMainPanel();
                
            DisableScrolls();
            BuildScroll.SetActive(true);
        }
        else
        {
            CloseMainPanel();
        }
    }

    public void OpenMainBuildingPanel()
    {
        if (!MainBuildingScroll.activeInHierarchy)
        {
            if(!mainPanelOpened)
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
            if(!mainPanelOpened)
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

    void DisableScrolls()
    {
        if (BuildScroll.activeInHierarchy)
            BuildScroll.SetActive(false);

        if (WarehouseScroll.activeInHierarchy)
            WarehouseScroll.SetActive(false);

        if (MainBuildingScroll.activeInHierarchy)
            MainBuildingScroll.SetActive(false);


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
