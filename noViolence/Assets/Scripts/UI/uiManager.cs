using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class uiManager : MonoBehaviour
{
    public static uiManager instance;
    void Awake() { instance = this; }

    [Header("Warehouse")]
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI stoneText;
    public TextMeshProUGUI berriesText;
    public TextMeshProUGUI MushroomsText;
    public TextMeshProUGUI WheatText;

    [Header("MainPanel")]
    public GameObject MainPanel;
    public GameObject BuildScroll;
    public GameObject WarehouseScroll;

    public Animator mainPanelAnimator;
    bool mainPanelOpened;
    GameObject ob;

    void OpenMainPanel()
    {
        if (!mainPanelOpened)
        {
            mainPanelAnimator.Play("mainPanelOpen");
            mainPanelOpened = true;
        }
        else
        {
            mainPanelAnimator.Play("mainPanelClose");
            mainPanelOpened = false;
        }
    }

    public void OpenBuildScroll()
    {
        OpenMainPanel();

        if (!BuildScroll.activeInHierarchy)
        {
            if(WarehouseScroll.activeInHierarchy)
                WarehouseScroll.SetActive(false);

            BuildScroll.SetActive(true);
        }
        else
        {
            DisableObject(BuildScroll, 0.5f);
        }
    }

    public void OpenWarehouseScroll()
    {
        OpenMainPanel();

        if (!WarehouseScroll.activeInHierarchy)
        {
            if(BuildScroll.activeInHierarchy)
                BuildScroll.SetActive(false);

            WarehouseScroll.SetActive(true);
            PlayerManager.instance.playerWarehouse.LoadResourceText();
        }
        else
        {
            DisableObject(WarehouseScroll, 0.5f);
        }
    }

    void DisableObject(GameObject obj, float time)
    {
        ob = obj;
        Invoke("DelayDisabling", time);
    }

    void DelayDisabling()
    {
        if(ob.activeInHierarchy)
            ob.SetActive(false);
        else
            return;
    }
}
