using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KitchenUI : MonoBehaviour
{
    public static KitchenUI kitchenUI;
    void Awake() { kitchenUI = this; }

    public Image kitchenProgressBar;
    public TextMeshProUGUI totalProdTxt;

    public TextMeshProUGUI workerTxt;
    public Button KitchenTabSwitchButton;


    public GameObject ProdScroll;
    
    public GameObject WorkerScroll;
    public Transform WorkerPanel;
    public GameObject workerPanelPrefab;
    public List<GameObject> WorkerPanels;

[Header("Upgrade")]
    public TextMeshProUGUI upgradeLvlTxt;
    public TextMeshProUGUI upgradeStonePriceTxt;
    public TextMeshProUGUI upgradeWoodPriceTxt;
    public GameObject upgradePanel;
    public Button openUpgradePanel;
    public Button increaseBuildingLevelButton;

    // [Header("Porridge")]
    // public TextMeshProUGUI porRateTxt;
    // public TextMeshProUGUI porRatePercTxt;
    // public TextMeshProUGUI porProdTxt;

    // public Toggle porBerry;
    // public Toggle porHoney;

    // public Button addPorProdButton;
    // public Button removePorProdButton;

    // [Header("Pottage")]
    // public TextMeshProUGUI potRateTxt;
    // public TextMeshProUGUI potRatePercTxt;
    // public TextMeshProUGUI potProdTxt;

    // public Toggle potWheat;
    // public Toggle potPot;

    // public Button addPotProdButton;
    // public Button removePotProdButton;

    // [Header("Bread")]
    // public TextMeshProUGUI brRateTxt;
    // public TextMeshProUGUI brRatePercTxt;
    // public TextMeshProUGUI brProdTxt;

    // public Toggle brPot;
    // public Toggle brHoney;

    // public Button addBrProdButton;
    // public Button removeBrProdButton;
    


}
