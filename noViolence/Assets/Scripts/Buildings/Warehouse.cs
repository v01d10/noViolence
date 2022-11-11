using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    public float mushroomsAmount;
    public float mushroomsMaxAmount;
    public float wheatAmount;
    public float wheatMaxAmount;
    void Start()
    {
        if(PlayerWarehouse)
        {
            PlayerManager.instance.playerWarehouse = this;
            LoadResourceText();
        }

    }

    public void LoadResourceText()
    {
        if (PlayerWarehouse)
        {
            uiManager.instance.woodText.text = woodAmount.ToString();
            uiManager.instance.stoneText.text = stoneAmount.ToString();
            uiManager.instance.berriesText.text = berryAmount.ToString();
            uiManager.instance.MushroomsText.text = mushroomsAmount.ToString();
            uiManager.instance.WheatText.text = wheatAmount.ToString();
        }
        else 
            return;
    }
}
