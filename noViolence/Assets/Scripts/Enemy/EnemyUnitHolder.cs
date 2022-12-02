using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitHolder : MonoBehaviour
{
    public static EnemyUnitHolder instance;
    [SerializeField] string fileName;
    public List<EnemyUnit> EnemyUnits = new List<EnemyUnit>();
    public List<eUnitSaveData> eUnitData = new List<eUnitSaveData>();

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
        foreach (var eUnit in EnemyUnits)
        {
            if (eUnit != null)
                eUnit.SaveState();
        }
        SaveLoadSystem.SaveToJSON<eUnitSaveData>(eUnitData, fileName);
    }

    public void LoadUnits()
    {
        eUnitData = SaveLoadSystem.ReadFromJSON<eUnitSaveData>(fileName);

        for (int i = 0; i < EnemyUnits.Count; i++)
        {
            if (EnemyUnits[i] != null)
            {
                EnemyUnits[i].LoadState(eUnitData[i]);
                if (eUnitData[i]._destroyed)
                {
                    Destroy(EnemyUnits[i].gameObject);
                }

                print("Loaded unit: " + i);

            }
        }
    }
}
