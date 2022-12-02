using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChurchUI : MonoBehaviour
{
    public static ChurchUI churchUI;
    void Awake() { churchUI = this; }

    public Image churchProgressBar;
    public TextMeshProUGUI totalProdTxt;
    public TextMeshProUGUI totalHealText;

    public TextMeshProUGUI workerTxt;
    public Button churchTabSwitchButton;


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


}
