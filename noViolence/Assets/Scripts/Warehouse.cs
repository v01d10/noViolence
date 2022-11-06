using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Warehouse : MonoBehaviour
{
    public static Warehouse instance;
    void Awake() {instance = this;}

[Header("Amounts")]
    public float woodAmount;
    public float woodMaxAmount;
    public float stoneAmount;
    public float stoneMaxAmount;
    public float berryAmount;
    public float berryMaxAmount;
    public float mushroomsAmount;
    public float mushroomsMaxAmount;
    public float wheatAmount;
    public float wheatMaxAmount;

[Header("Texts")]
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI stoneText;
    public TextMeshProUGUI berriesText;
    public TextMeshProUGUI MushroomsText;
    public TextMeshProUGUI WheatText;

    public void LoadResourceText()
    {
        woodText.text = woodAmount.ToString();
        stoneText.text = stoneAmount.ToString();
        berriesText.text = berryAmount.ToString();
        MushroomsText.text = mushroomsAmount.ToString();
        WheatText.text = wheatAmount.ToString();
    }
}
