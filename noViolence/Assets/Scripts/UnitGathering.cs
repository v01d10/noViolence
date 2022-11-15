using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGathering : MonoBehaviour
{
    public enum CarryType
    {
        Wood, Stone,
        Berry, Mushrooms, Wheat
    }
    public CarryType cType;

    public float MaxCarryAmount;
    public float TotalCarryAmount;
    public float CarryWood;
    public float CarryStone;
    public float CarryBerry;
    public float CarryMushrooms;
    public float CarryWheat;

    public float GatherTime;
    public float gatherAmount;
    public float RadiusAroundTarget = 1.5f;

    public bool toGather;
    public bool toUnload;
    public bool playerUnit;

    SelectableUnit thisUnit;
    EnemyUnitNav eUnit;
    Resource res;
    public Resource resTarget;
    Warehouse whouse;
    public Vector3 warehouseLocation;
    public Vector3 target;
    EnemyManager eManager;

    void Start()
    {
        if (playerUnit)
        {
            thisUnit = GetComponentInParent<SelectableUnit>();
            whouse = PlayerManager.instance.playerWarehouse;
            Warehouse.instance.LoadResourceText();
        }
        else
        {
            eUnit = GetComponentInParent<EnemyUnitNav>();
            eManager = GetComponentInParent<EnemyManager>();
            whouse = eManager.enemyWarehouse;
        }

        if (whouse != null)
            warehouseLocation = whouse.transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (resTarget != null && other.GetComponent<Resource>() == resTarget && resTarget.Gatherers.Contains(this))
        {
            res = other.GetComponent<Resource>();

            toGather = true;

            if (cType.ToString() != res.rType.ToString())
            {
                if (res.rType == Resource.ResType.Wood) cType = CarryType.Wood;
                else if (res.rType == Resource.ResType.Stone) cType = CarryType.Stone;
                else if (res.rType == Resource.ResType.Berry) cType = CarryType.Berry;
                else if (res.rType == Resource.ResType.Mushrooms) cType = CarryType.Mushrooms;
                else if (res.rType == Resource.ResType.Wheat) cType = CarryType.Wheat;
            }

            StartCoroutine("LoadResource");

            if (!playerUnit)
            {
                eUnit.checkIfGathering();
            }
        }

        if (other.CompareTag("Warehouse") && toUnload)
        {
            UnloadResouce();
        }
    }


    IEnumerator LoadResource()
    {
        if (cType == CarryType.Wood)
            StartCoroutine("LoadWood");
        else if (cType == CarryType.Stone)
            StartCoroutine("LoadStone");
        else if (cType == CarryType.Berry)
            StartCoroutine("LoadBerry");
        else if (cType == CarryType.Mushrooms)
            StartCoroutine("LoadMushrooms");
        else if (cType == CarryType.Wheat)
            StartCoroutine("LoadWheat");

        TotalCarryAmount = CarryWood + CarryStone + CarryBerry + CarryMushrooms + CarryWheat;
        yield return null;
    }

    void UnloadResouce()
    {
        UnloadWood();
        UnloadStone();
        UnloadBerry();
        UnloadMushrooms();
        UnloadWheat();

        TotalCarryAmount = CarryWood + CarryStone + CarryBerry + CarryMushrooms + CarryWheat;
        Warehouse.instance.LoadResourceText();
    }

    void ResourceDepleted()
    {
        print("Resource depleted");
        res.RemoveResource(res.TotalAmount);
        Destroy(res.gameObject, 3);
        thisUnit.MoveTo(warehouseLocation);
        eUnit.MoveTo(warehouseLocation);
        toUnload = true;
        toGather = false;
        thisUnit.Agent.isStopped = true;
    }

    void GoToWarehouse()
    {
        if (playerUnit)
        {
            thisUnit.Agent.isStopped = false;
            thisUnit.MoveTo(warehouseLocation);
        }
        else
        {
            eUnit.agent.isStopped = false;
            eUnit.MoveTo(warehouseLocation);
        }

        toUnload = true;
        toGather = false;
    }

    void CheckForTarget()
    {
        if (resTarget != null)
        {
            if (playerUnit)
                thisUnit.MoveTo(target);
            else
                eUnit.MoveTo(target);

            toGather = true;
        }
        else
        {
            print("No target");
            if (playerUnit)
                thisUnit.Agent.isStopped = true;
            else
                eUnit.agent.isStopped = true;
        }
    }

    IEnumerator LoadWood()
    {
        if (res.TotalAmount > gatherAmount)
        {
            if (TotalCarryAmount + gatherAmount <= MaxCarryAmount)
            {
                print("Gathering " + cType);
                res.RemoveResource(gatherAmount);
                CarryWood += gatherAmount;
                yield return new WaitForSeconds(GatherTime);
                StartCoroutine("LoadResource");
            }
            else
            {
                GoToWarehouse();
                yield return null;
            }
        }
        else
        {
            CarryWood += res.TotalAmount;
            ResourceDepleted();
        }
    }

    IEnumerator LoadStone()
    {
        if (res.TotalAmount > gatherAmount)
        {
            if (TotalCarryAmount + gatherAmount <= MaxCarryAmount)
            {
                print("Gathering " + cType);
                res.RemoveResource(gatherAmount);
                CarryStone += gatherAmount;
                yield return new WaitForSeconds(GatherTime);
                StartCoroutine("LoadResource");

            }
            else
            {
                GoToWarehouse();
                yield return null;
            }
        }
        else
        {
            CarryStone += res.TotalAmount;
            ResourceDepleted();
        }
    }

    IEnumerator LoadBerry()
    {
        if (res.TotalAmount > gatherAmount)
        {
            if (TotalCarryAmount + gatherAmount <= MaxCarryAmount)
            {
                print("Gathering " + cType);
                res.RemoveResource(gatherAmount);
                CarryBerry += gatherAmount;
                yield return new WaitForSeconds(GatherTime);
                StartCoroutine("LoadResource");

            }
            else
            {
                GoToWarehouse();
                yield return null;
            }
        }
        else
        {
            CarryBerry += res.TotalAmount;
            ResourceDepleted();

        }
    }

    IEnumerator LoadMushrooms()
    {
        if (res.TotalAmount > gatherAmount)
        {
            if (TotalCarryAmount + gatherAmount <= MaxCarryAmount)
            {
                print("Gathering " + cType);
                res.RemoveResource(gatherAmount);
                CarryMushrooms += gatherAmount;
                yield return new WaitForSeconds(GatherTime);
                StartCoroutine("LoadResource");

            }
            else
            {
                GoToWarehouse();
                yield return null;
            }
        }
        else
        {
            CarryMushrooms += res.TotalAmount;
            ResourceDepleted();
        }
    }

    IEnumerator LoadWheat()
    {
        if (res.TotalAmount > gatherAmount)
        {
            if (TotalCarryAmount + gatherAmount <= MaxCarryAmount)
            {
                print("Gathering " + cType);
                res.RemoveResource(gatherAmount);
                CarryWheat += gatherAmount;
                yield return new WaitForSeconds(GatherTime);
                StartCoroutine("LoadResource");
            }
            else
            {
                GoToWarehouse();
                yield return null;
            }
        }
        else
        {
            CarryWheat += res.TotalAmount;
            ResourceDepleted();
        }
    }

    void UnloadWood()
    {
        if (whouse.woodMaxAmount >= CarryWood)
        {
            whouse.woodAmount += CarryWood;
            CarryWood = 0;
            toUnload = false;

            CheckForTarget();
        }
        else
        {
            whouse.woodAmount += whouse.woodMaxAmount - whouse.woodAmount;
            CarryWood -= whouse.woodMaxAmount - whouse.woodAmount;

            if (playerUnit)
                thisUnit.Agent.isStopped = true;
            else
                eUnit.agent.isStopped = true;
            print("Warehouse is full - " + cType);
        }
    }

    void UnloadStone()
    {
        if (whouse.stoneMaxAmount >= CarryStone)
        {
            whouse.stoneAmount += CarryStone;
            CarryStone = 0;
            toUnload = false;

            CheckForTarget();
        }
        else
        {
            whouse.stoneAmount += whouse.stoneMaxAmount - whouse.stoneAmount;
            CarryStone -= whouse.stoneMaxAmount - whouse.stoneAmount;

            if (playerUnit)
                thisUnit.Agent.isStopped = true;
            else
                eUnit.agent.isStopped = true;
            print("Warehouse is full - " + cType);
        }
    }

    void UnloadBerry()
    {
        if (whouse.berryMaxAmount >= CarryBerry)
        {
            whouse.berryAmount += CarryBerry;
            CarryBerry = 0;
            toUnload = false;

            CheckForTarget();
        }
        else
        {
            whouse.berryAmount += whouse.berryMaxAmount - whouse.berryAmount;
            CarryBerry -= whouse.berryMaxAmount - whouse.berryAmount;

            if (playerUnit)
                thisUnit.Agent.isStopped = true;
            else
                eUnit.agent.isStopped = true;
            print("Warehouse is full - " + cType);
        }
    }

    void UnloadMushrooms()
    {
        if (whouse.mushroomsMaxAmount >= CarryMushrooms)
        {
            whouse.mushroomsAmount += CarryMushrooms;
            CarryMushrooms = 0;
            toUnload = false;

            CheckForTarget();
        }
        else
        {
            whouse.mushroomsAmount += whouse.mushroomsMaxAmount - whouse.mushroomsAmount;
            CarryMushrooms -= whouse.mushroomsMaxAmount - whouse.mushroomsAmount;

            if (playerUnit)
                thisUnit.Agent.isStopped = true;
            else
                eUnit.agent.isStopped = true;
            print("Warehouse is full - " + cType);
        }
    }

    void UnloadWheat()
    {
        if (whouse.wheatMaxAmount >= CarryWheat)
        {
            whouse.wheatAmount += CarryWheat;
            CarryWheat = 0;
            toUnload = false;

            CheckForTarget();
        }
        else
        {
            whouse.wheatAmount += whouse.wheatMaxAmount - whouse.wheatAmount;
            CarryWheat -= whouse.wheatMaxAmount - whouse.wheatAmount;

            if (playerUnit)
                thisUnit.Agent.isStopped = true;
            else
                eUnit.agent.isStopped = true;
            print("Warehouse is full - " + cType);
        }
    }
}
