
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    public enum eUnitType { Basic, Strong, Smart, Joyful };
    public eUnitType euType;

    public GameObject FuturePlayerUnit;
    public string Name;
    public float Health;
    public float Resistance;
    public float MaxResistance;

    public int intLevel;
    public int strLevel;
    public int jllnsLevel;
    public float attackRate;

    public Vector3 position;

    public eUnitSaveData eUnitData;
    public List<PlayerUnit> pUnitsFighWith;
    public UnitGathering eUnitGathering;
    public CombatDetection eCombatDetection;

    bool unitSet;
    public bool Destroyed;

    void Awake()
    {
        if (!EnemyUnitHolder.instance.EnemyUnits.Contains(this))
        {
            EnemyUnitHolder.instance.EnemyUnits.Add(this);
        }
    }

    void Start()
    {
        if (!unitSet)
        {
            eUnitGathering = GetComponentInChildren<UnitGathering>();
            eCombatDetection = GetComponentInChildren<CombatDetection>();

            Name = GameManager.instance.UnitNames[Random.Range(0, GameManager.instance.UnitNames.Length)];
            Health = euType == eUnitType.Strong ? Health = Random.Range(140, 200) : Health = Random.Range(80, 100);
            Resistance = euType == eUnitType.Strong ? Resistance = Random.Range(140, 200) : Resistance = Random.Range(80, 100);
            unitSet = true;
        }

        Invoke("LateStart", 0.1f);

    }

    void LateStart()
    {
        if (Destroyed)
            Destroy(gameObject);
    }

    public void CancelAllActions()
    {
        CancelGathering();
        CancelCombat();
    }

    public void CancelGathering()
    {
        eUnitGathering.resTarget.Gatherers.Remove(eUnitGathering);
        eUnitGathering.resTarget.placIndex--;
        eUnitGathering.toGather = false;
        eUnitGathering.toUnload = false;
        eUnitGathering.resTarget = null;
        eUnitGathering.gatheringSource.Stop();
        eUnitGathering.StopAllCoroutines();
    }

    public void CancelCombat()
    {
        if (eCombatDetection.eUnitTarget.eUnitsFighWith.Contains(this))
            eCombatDetection.eUnitTarget.eUnitsFighWith.Remove(this);
        eCombatDetection.fighting = false;
        eCombatDetection.pUnitTarget = null;
        eCombatDetection.StopAllCoroutines();
    }

    public void SaveState()
    {
        if (GetComponentInParent<EnemyUnitHolder>().eUnitData.Contains(eUnitData))
        {
            int indexOf = GetComponentInParent<EnemyUnitHolder>().eUnitData.IndexOf(eUnitData);

            if (GetComponentInParent<EnemyUnitHolder>().eUnitData[indexOf] != null)
            {
                eUnitSaveData euData = GetComponentInParent<EnemyUnitHolder>().eUnitData[indexOf];

                euData._euType = (eUnitSaveData.eUnitType)euType;
                euData._FuturePlayerUnit = this.FuturePlayerUnit;
                euData._name = this.Name;
                euData._intLevel = this.intLevel; euData._strLevel = this.strLevel; euData._jllnsLevel = this.jllnsLevel;
                euData._resistance = this.Resistance; euData._maxResistance = this.MaxResistance;
                euData._attackRate = this.attackRate;
                euData._position = transform.position; euData._unitSet = this.unitSet; euData._destroyed = this.Destroyed;
            }
        }
        else
        {
            eUnitData = new eUnitSaveData();

            eUnitData._euType = (eUnitSaveData.eUnitType)euType;
            eUnitData._FuturePlayerUnit = this.FuturePlayerUnit;
            eUnitData._name = this.Name;
            eUnitData._intLevel = this.intLevel; eUnitData._strLevel = this.strLevel; eUnitData._jllnsLevel = this.jllnsLevel;
            eUnitData._resistance = this.Resistance; eUnitData._maxResistance = this.MaxResistance;
            eUnitData._attackRate = this.attackRate;
            eUnitData._position = transform.position; eUnitData._unitSet = this.unitSet; eUnitData._destroyed = this.Destroyed;

            GetComponentInParent<EnemyUnitHolder>().eUnitData.Add(eUnitData);
            print(GetComponentInParent<EnemyUnitHolder>().eUnitData.Count);

        }

    }

    public void LoadState(eUnitSaveData _eUnitData)
    {
        eUnitData = _eUnitData;
        Destroyed = eUnitData._destroyed;

        if (Destroyed == true)
        {
            transform.parent.position = transform.position;

            PlayerUnit futurePlayerUnit = FuturePlayerUnit.GetComponent<PlayerUnit>();
            futurePlayerUnit.transform.position = transform.position;
            futurePlayerUnit.transform.parent.SetParent(PlayerManager.instance.playerUnitHolder.transform);
            futurePlayerUnit.enabled = true;

            futurePlayerUnit.Name = Name;
            futurePlayerUnit.Health = Health;

            if (strLevel > 5)
                futurePlayerUnit.strLevel = strLevel < 15 ? strLevel -= Random.Range(1, 3) : strLevel -= Random.Range(3, 6);
            if (intLevel > 5)
                futurePlayerUnit.intLevel = futurePlayerUnit.intLevel < 15 ? intLevel -= Random.Range(1, 3) : intLevel -= Random.Range(3, 6);
            if (jllnsLevel > 5)
                futurePlayerUnit.skillLevel = futurePlayerUnit.skillLevel < 15 ? jllnsLevel -= Random.Range(1, 3) : jllnsLevel -= Random.Range(3, 6);

            if (pUnitsFighWith.Count > 0)
            {
                for (int i = 0; i < pUnitsFighWith.Count; i++)
                {
                    pUnitsFighWith.Remove(pUnitsFighWith[i]);
                }
            }

            FuturePlayerUnit.SetActive(true);
            Destroy(gameObject);
            print("Unit converted...");
        }
        else
        {
            euType = (eUnitType)_eUnitData._euType;
            FuturePlayerUnit = eUnitData._FuturePlayerUnit;
            Name = eUnitData._name;
            intLevel = eUnitData._intLevel; strLevel = eUnitData._strLevel; jllnsLevel = eUnitData._jllnsLevel;
            Resistance = eUnitData._resistance; MaxResistance = eUnitData._maxResistance;
            attackRate = eUnitData._attackRate;
            position = (transform.parent != null) ? transform.parent.position = eUnitData._position : transform.position = eUnitData._position;
            unitSet = eUnitData._unitSet;
        }
    }
}


[System.Serializable]
public class eUnitSaveData
{
    public enum eUnitType { Basic, Strong, Smart, Joyful };
    public eUnitType _euType;
    public GameObject _FuturePlayerUnit;
    public string _name;
    public float _resistance, _maxResistance;

    public int _intLevel, _strLevel, _jllnsLevel;

    public float _attackRate;

    public Vector3 _position;
    public bool _unitSet; public bool _destroyed;
}
