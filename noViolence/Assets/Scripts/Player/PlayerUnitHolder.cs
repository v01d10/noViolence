using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitHolder : MonoBehaviour
{
    public static PlayerUnitHolder instance;

    [SerializeField] string fileName;
    public List<PlayerUnit> PlayerUnits = new List<PlayerUnit>();
    public List<pUnitSaveData> pUnitData = new List<pUnitSaveData>();

    void Awake()
    {
        instance = this;
        Invoke("LoadUnits", 0.3f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SaveUnits();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadUnits();
        }
    }

    public void SaveUnits()
    {
        foreach (var pUnit in PlayerUnits)
        {
            if (pUnit != null)
                pUnit.SaveState();
        }
        SaveLoadSystem.SaveToJSON<pUnitSaveData>(pUnitData, fileName);
    }

    void LoadUnits()
    {
        pUnitData = SaveLoadSystem.ReadFromJSON<pUnitSaveData>(fileName);

        for (int i = 0; i < PlayerUnits.Count; i++)
        {
            if (PlayerUnits[i] != null)
            {
                PlayerUnits[i].LoadState(pUnitData[i]);
                if (pUnitData[i]._destroyed)
                {
                    Destroy(PlayerUnits[i].gameObject);
                }

                print("Loaded unit: " + i);

            }
        }
    }
}
