using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    public enum EnemyType { Basic, Strong, Smart, Joyful };
    public EnemyType enemyType;

    public string Name;
    public float Health;
    public float Resistance;

    public int intLevel;
    public int strLevel;
    public int jllnsLevel;
    public float attackRate;

    public List<PlayerUnit> pUnitsFighWith;
    UnitGathering unitGathering;

    bool unitSet;

    void Awake()
    {
        if (!unitSet)
        {
            unitGathering = GetComponentInChildren<UnitGathering>();
            Name = GameManager.instance.UnitNames[Random.Range(0, GameManager.instance.UnitNames.Count)];
            Health = enemyType == EnemyType.Strong ? Health = Random.Range(140, 200) : Health = Random.Range(80, 100);
            Resistance = enemyType == EnemyType.Strong ? Resistance = Random.Range(140, 200) : Resistance = Random.Range(80, 100);
            unitSet = true;
        }
    }

    public void CancelGathering()
    {
        unitGathering.resTarget.Gatherers.Remove(unitGathering);
        unitGathering.resTarget.placIndex--;
        unitGathering.toGather = false;
        unitGathering.toUnload = false;
        unitGathering.resTarget = null;
        unitGathering.gatheringSource.Stop();
        unitGathering.StopAllCoroutines();
    }
}
