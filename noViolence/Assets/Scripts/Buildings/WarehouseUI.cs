using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WarehouseUI : MonoBehaviour
{
    public static WarehouseUI whouseUI;

    [Header("Warehouse")]
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI maxWoodText;
    public TextMeshProUGUI stoneText;
    public TextMeshProUGUI maxStoneText;
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI maxFoodText;
    public TextMeshProUGUI berryText;
    public TextMeshProUGUI maxBerryText;
    public TextMeshProUGUI shroomText;
    public TextMeshProUGUI maxShroomText;
    public TextMeshProUGUI WheatText;

    void Awake()
    {
        whouseUI = this;
    }
}
