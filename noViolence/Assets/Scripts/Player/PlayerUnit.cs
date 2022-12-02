using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : MonoBehaviour
{
    public enum pUnitType { Basic, Strong, Smart, Joyful };
    public pUnitType puType;

    [Header("Base stats")]
    public int pUnitID;
    public GameObject unitModel;
    public GameObject thisUnitModel;

    public string Name;
    public float Health;
    public float MaxHealth;
    public float HealthPerc;

    public int level;
    public float exp;
    public float expNeeded;
    public float expPerc;

    [Header("Attributes")]
    public int atPoints;
    public int intLevel;
    public int strLevel;
    public int skillLevel;
    public float attackRate;

    public Vector3 position;
    public bool unitSet;
    public bool ChoosedSpec;
    public bool Active;
    public bool Destroyed;

    public pUnitSaveData pUnitData;

    public List<EnemyUnit> eUnitsFighWith;
    public UnitGathering pUnitGathering;
    public CombatDetection pCombatDetection;
    public PlayerBuildingDetection playerBdetection;
    Animator pUnitAnimator;

    void Awake()
    {
        if (!GetComponentInParent<PlayerUnitHolder>().PlayerUnits.Contains(this))
        {
            GetComponentInParent<PlayerUnitHolder>().PlayerUnits.Add(this);
        }
    }

    void Start()
    {
        if (!unitSet)
        {
            thisUnitModel = Instantiate(PlayerManager.instance.unitModels[UnityEngine.Random.Range(0, PlayerManager.instance.unitModels.Length)], transform);
            thisUnitModel.transform.position = transform.position;
            unitSet = true;
        }

        pUnitGathering = GetComponentInChildren<UnitGathering>();
        pCombatDetection = GetComponentInChildren<CombatDetection>();
        playerBdetection = GetComponentInChildren<PlayerBuildingDetection>();
        pUnitAnimator = thisUnitModel.GetComponentInChildren<Animator>();

        Invoke("LateStart", 0.1f);
    }

    void LateStart()
    {
        if (Destroyed)
            Destroy(gameObject);
    }

    void Update()
    {
        pUnitAnimator.SetFloat("Move", GetComponent<SelectableUnit>().Agent.velocity.magnitude);
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlayerUnitUI.PUI.OpenPlayerUnitPanel(this);
        }
    }

    public void AddExperience(int amount)
    {
        if (exp + amount < expNeeded)
        {
            exp += amount;
        }
        else
        {
            level++;
            atPoints++;
            exp = 0;
            expNeeded *= 1.5f;
        }
    }

    public void CancelAllActions()
    {
        CancelGathering();
        CancelCombat();
        CancelCooking();
    }

    public void CancelGathering()
    {
        if (pUnitGathering.resTarget != null)
        {
            if (pUnitGathering.resTarget.Gatherers.Contains(pUnitGathering))
                pUnitGathering.resTarget.Gatherers.Remove(pUnitGathering);
            pUnitGathering.resTarget.placIndex--;
            pUnitGathering.toGather = false;
            pUnitGathering.toUnload = false;
            pUnitGathering.resTarget = null;
            pUnitGathering.gatheringSource.Stop();
            pUnitGathering.StopAllCoroutines();
        }
    }

    public void CancelCombat()
    {
        if (pCombatDetection.fighting && pCombatDetection.pUnitTarget != null)
        {
            if (pCombatDetection.pUnitTarget.pUnitsFighWith.Contains(this))
                pCombatDetection.pUnitTarget.pUnitsFighWith.Remove(this);
            pCombatDetection.fighting = false;
            pCombatDetection.pUnitTarget = null;
            pCombatDetection.StopAllCoroutines();

        }
    }

    void CancelCooking()
    {
        if (playerBdetection._selKitchen != null)
        {
            if (playerBdetection._selKitchen.workingUnits.Count == 0)
                KitchenUI.kitchenUI.kitchenProgressBar.fillAmount = 0;

            if (playerBdetection._selKitchen.workingUnits.Contains(this))
            {
                playerBdetection._selKitchen.workingUnits.Remove(this);
                playerBdetection._selKitchen = null;

                if (uiManager.instance.KitchenScroll.activeInHierarchy && playerBdetection._selKitchen != null)
                    playerBdetection._selKitchen.LoadKitchenText();
                playerBdetection.StopAllCoroutines();
            }
        }
    }

    public void SaveState()
    {
        if (GetComponentInParent<PlayerUnitHolder>().pUnitData.Contains(pUnitData))
        {
            int indexOf = GetComponentInParent<PlayerUnitHolder>().pUnitData.IndexOf(pUnitData);

            if (GetComponentInParent<PlayerUnitHolder>().pUnitData[indexOf] != null)
            {
                pUnitSaveData puData = GetComponentInParent<PlayerUnitHolder>().pUnitData[indexOf];

                puData._puType = (pUnitSaveData.pUnitType)puType;
                puData._unitModel = this.unitModel;
                puData._thisUnitModel = this.thisUnitModel;
                puData._pUnitID = this.pUnitID;
                puData._name = this.name;
                puData._health = this.Health; puData._maxHealth = this.MaxHealth;
                puData._level = this.level; puData._exp = this.exp; puData._expNeeded = this.expNeeded;
                puData._atPoints = this.atPoints; puData._intLevel = this.intLevel; puData._strLevel = this.strLevel; puData._skillLevel = this.skillLevel;
                puData._attackRate = this.attackRate;
                puData._position = transform.position; puData._unitSet = this.unitSet; puData._active = this.Active; puData._destroyed = this.Destroyed;
            }
        }
        else
        {
            pUnitData = new pUnitSaveData();

            pUnitData._puType = (pUnitSaveData.pUnitType)puType;
            pUnitData._unitModel = this.unitModel;
            pUnitData._thisUnitModel = this.thisUnitModel;
            pUnitData._pUnitID = this.pUnitID;
            pUnitData._name = this.name;
            pUnitData._health = this.Health; pUnitData._maxHealth = this.MaxHealth;
            pUnitData._level = this.level; pUnitData._exp = this.exp; pUnitData._expNeeded = this.expNeeded;
            pUnitData._atPoints = this.atPoints; pUnitData._intLevel = this.intLevel; pUnitData._strLevel = this.strLevel; pUnitData._skillLevel = this.skillLevel;
            pUnitData._attackRate = this.attackRate;

            pUnitData._position = transform.position; pUnitData._unitSet = this.unitSet; pUnitData._active = this.Active; pUnitData._destroyed = this.Destroyed;

            GetComponentInParent<PlayerUnitHolder>().pUnitData.Add(pUnitData);
            print(GetComponentInParent<PlayerUnitHolder>().pUnitData.Count);

        }

    }

    public void LoadState(pUnitSaveData _pUnitData)
    {
        pUnitData = _pUnitData;

        puType = (pUnitType)_pUnitData._puType;
        unitModel = pUnitData._unitModel;
        thisUnitModel = pUnitData._thisUnitModel;
        pUnitID = pUnitData._pUnitID;
        Name = pUnitData._name;
        Health = pUnitData._health; MaxHealth = pUnitData._maxHealth;
        level = pUnitData._level; exp = pUnitData._exp; expNeeded = pUnitData._expNeeded;
        atPoints = pUnitData._atPoints; intLevel = pUnitData._intLevel; strLevel = pUnitData._strLevel; skillLevel = pUnitData._skillLevel;
        attackRate = pUnitData._attackRate;
        unitSet = pUnitData._unitSet;
        Destroyed = pUnitData._destroyed;



        if (transform.parent != null)
        {
            transform.parent.position = pUnitData._position;
        }
        else
        {
            transform.position = pUnitData._position;

            if (Destroyed == true)
            {
                SelectionManager.instance.SelectedUnits.Remove(GetComponent<SelectableUnit>());
                SelectionManager.instance.AvailableUnits.Remove(GetComponent<SelectableUnit>());
                Destroy(gameObject);
            }
        }

    }

}

[Serializable]
public class pUnitSaveData
{
    public enum pUnitType { Basic, Strong, Smart, Joyful };
    public pUnitType _puType;
    public GameObject _unitModel;
    public GameObject _thisUnitModel;
    public int _pUnitID;
    public string _name;
    public float _health, _maxHealth;

    public int _level;
    public float _exp, _expNeeded;

    public int _atPoints;
    public int _intLevel, _strLevel, _skillLevel;

    public float _attackRate;

    public Vector3 _position;
    public bool _unitSet; public bool _active; public bool _destroyed;

}