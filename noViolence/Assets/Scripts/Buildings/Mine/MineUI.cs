using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MineUI : MonoBehaviour
{
    public static MineUI mineUI;
    void Awake() { mineUI = this; }

    public Image MineProgressBar;
    public TextMeshProUGUI totalProdTxt;
    public TextMeshProUGUI totalHealText;

    public TextMeshProUGUI workerTxt;
    public Button MineTabSwitchButton;


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
