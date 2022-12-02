using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildingHolder : MonoBehaviour
{
    public static PlayerBuildingHolder instance;
    [SerializeField] string fileName;
    public List<Building> PlayerBuildings = new List<Building>();
    public List<pBuildingSaveData> pBuildingData = new List<pBuildingSaveData>();

    void Awake()
    {
        instance = this;
        Invoke("LoadBuildings", 0.3f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SaveBuildings();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadBuildings();
        }
    }

    public void SaveBuildings()
    {
        foreach (var eBuilding in PlayerBuildings)
        {
            if (eBuilding != null)
                eBuilding.SaveState();
        }
        SaveLoadSystem.SaveToJSON<pBuildingSaveData>(pBuildingData, fileName);
    }

    public void LoadBuildings()
    {
        pBuildingData = SaveLoadSystem.ReadFromJSON<pBuildingSaveData>(fileName);

        for (int i = 0; i < PlayerBuildings.Count; i++)
        {
            if (PlayerBuildings[i] != null)
            {
                PlayerBuildings[i].LoadState(pBuildingData[i]);

                print("Loaded Building: " + i);

            }
        }
    }
}
