using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static WarehouseUI;

public class Warehouse : MonoBehaviour
{
    public static Warehouse instance;
    void Awake() { instance = this; LoadResourceText(); }

    public bool PlayerWarehouse;

    [Header("MainAmounts")]
    public float woodAmount;
    public float woodMaxAmount;
    public float stoneAmount;
    public float stoneMaxAmount;
    public float foodAmount;
    public float foodMaxAmount;

    [Header("SideAmounts")]
    public float berryAmount;
    public float berryMaxAmount;
    public float shroomAmount;
    public float shroomMaxAmount;
    public float wheatAmount;
    public float wheatMaxAmount;
    public float potatoAmount;
    public float potatoesMaxAmount;
    public float honeyAmount;
    public float honeyMaxAmount;
    public float ironAmount;
    public float ironMaxAmount;

    void Start()
    {
        if (PlayerWarehouse)
        {
            PlayerManager.instance.playerWarehouse = this;
            LoadResourceText();
        }

    }

    public void LoadResourceText()
    {
        if (PlayerWarehouse)
        {
            whouseUI.woodText.text = woodAmount.ToString();
            whouseUI.maxWoodText.text = woodMaxAmount.ToString();
            whouseUI.stoneText.text = stoneAmount.ToString();
            whouseUI.maxStoneText.text = stoneMaxAmount.ToString();
            whouseUI.foodText.text = foodAmount.ToString();
            whouseUI.maxFoodText.text = foodMaxAmount.ToString();
            whouseUI.berryText.text = berryAmount.ToString();
            whouseUI.maxBerryText.text = berryMaxAmount.ToString();
            whouseUI.shroomText.text = shroomAmount.ToString();
            whouseUI.maxShroomText.text = shroomMaxAmount.ToString();

        }
        else
            return;
    }
}
