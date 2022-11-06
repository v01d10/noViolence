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
        if(playerUnit)
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

        warehouseLocation = whouse.transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if(resTarget != null && other.GetComponent<Resource>() == resTarget && resTarget.Gatherers.Contains(this))
        {
            res = other.GetComponent<Resource>();

            toGather = true;

            if(cType.ToString() != res.rType.ToString())
            {
                if(res.rType == Resource.ResType.Wood) cType = CarryType.Wood;
                else if(res.rType == Resource.ResType.Stone) cType = CarryType.Stone;
                else if(res.rType == Resource.ResType.Berry) cType = CarryType.Berry;
                else if(res.rType == Resource.ResType.Mushrooms) cType = CarryType.Mushrooms;
                else if(res.rType == Resource.ResType.Wheat) cType = CarryType.Wheat;
            }

            StartCoroutine("LoadResource");

            if(!playerUnit)
            {
                eUnit.checkIfGathering();
            }           
        }

        if(other.CompareTag("Warehouse") && toUnload)
        {
            UnloadResouce();
        }
    }

    
    IEnumerator LoadResource()
    {
        if(cType == CarryType.Wood)
            StartCoroutine("LoadWood");
        else if(cType == CarryType.Stone)
            StartCoroutine("LoadStone");
        else if(cType == CarryType.Berry)
            StartCoroutine("LoadBerry");
        else if(cType == CarryType.Mushrooms)
            StartCoroutine("LoadMushrooms");
        else if(cType == CarryType.Wheat)
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

    IEnumerator LoadWood()
    {
        if(res.TotalAmount > gatherAmount)
        {
            if(TotalCarryAmount + gatherAmount <= MaxCarryAmount)
            {
                print("Gathering " + cType);
                res.RemoveResource(gatherAmount);
                CarryWood += gatherAmount;
                yield return new WaitForSeconds(GatherTime);
                StartCoroutine("LoadResource");
            }
            else
            {
                if(playerUnit)
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
                yield return null;
            }
        }
        else
        {
            print("Resource depleted");
            res.RemoveResource(res.TotalAmount);
            CarryWood += res.TotalAmount;
            Destroy(res.gameObject, 3);

            if(playerUnit)
                thisUnit.MoveTo(warehouseLocation);
            else
                eUnit.MoveTo(warehouseLocation);

            toUnload = true;
            toGather = false;
            thisUnit.Agent.isStopped = true;
        }
    }

    IEnumerator LoadStone()
    {
        if(res.TotalAmount > gatherAmount)
        {
            if(TotalCarryAmount + gatherAmount <= MaxCarryAmount)
            {
                print("Gathering " + cType);
                res.RemoveResource(gatherAmount);
                CarryStone += gatherAmount;
                yield return new WaitForSeconds(GatherTime);
                StartCoroutine("LoadResource");

            }
            else
            {
                thisUnit.Agent.isStopped = false;
                eUnit.agent.isStopped = false;
                thisUnit.MoveTo(warehouseLocation);
                eUnit.MoveTo(warehouseLocation);
                toUnload = true;
                toGather = false;
                yield return null;
            }
        }
        else
        {
            print("Resource depleted");
            res.RemoveResource(res.TotalAmount);
            CarryStone += res.TotalAmount;
            Destroy(res.gameObject, 3);
            thisUnit.MoveTo(warehouseLocation);
            eUnit.MoveTo(warehouseLocation);
            toUnload = true;
            toGather = false;
            thisUnit.Agent.isStopped = true;
        }
    }
    
    IEnumerator LoadBerry()
    {
        if(res.TotalAmount > gatherAmount)
        {
            if(TotalCarryAmount + gatherAmount <= MaxCarryAmount)
            {
                print("Gathering " + cType);
                res.RemoveResource(gatherAmount);
                CarryBerry += gatherAmount;
                yield return new WaitForSeconds(GatherTime);
                StartCoroutine("LoadResource");

            }
            else
            {
                thisUnit.Agent.isStopped = false;
                eUnit.agent.isStopped = false;
                thisUnit.MoveTo(warehouseLocation);
                eUnit.MoveTo(warehouseLocation);
                toUnload = true;
                toGather = false;
                yield return null;
            }
        }
        else
        {
            print("Resource depleted");
            res.RemoveResource(res.TotalAmount);
            CarryBerry += res.TotalAmount;
            Destroy(res.gameObject, 3);
            thisUnit.MoveTo(warehouseLocation);
            eUnit.MoveTo(warehouseLocation);
            toUnload = true;
            toGather = false;
            thisUnit.Agent.isStopped = true;
        }
    }

    IEnumerator LoadMushrooms()
    {
        if(res.TotalAmount > gatherAmount)
        {
            if(TotalCarryAmount + gatherAmount <= MaxCarryAmount)
            {
                print("Gathering " + cType);
                res.RemoveResource(gatherAmount);
                CarryMushrooms += gatherAmount;
                yield return new WaitForSeconds(GatherTime);
                StartCoroutine("LoadResource");

            }
            else
            {
                thisUnit.Agent.isStopped = false;
                eUnit.agent.isStopped = false;
                thisUnit.MoveTo(warehouseLocation);
                eUnit.MoveTo(warehouseLocation);
                toUnload = true;
                toGather = false;
                yield return null;
            }
        }
        else
        {
            print("Resource depleted");
            res.RemoveResource(res.TotalAmount);
            CarryMushrooms += res.TotalAmount;
            Destroy(res.gameObject, 3);
            thisUnit.MoveTo(warehouseLocation);
            eUnit.MoveTo(warehouseLocation);
            toUnload = true;
            toGather = false;
            thisUnit.Agent.isStopped = true;
        }
    }

    IEnumerator LoadWheat()
    {
        if(res.TotalAmount > gatherAmount)
        {
            if(TotalCarryAmount + gatherAmount <= MaxCarryAmount)
            {
                print("Gathering " + cType);
                res.RemoveResource(gatherAmount);
                CarryWheat += gatherAmount;
                yield return new WaitForSeconds(GatherTime);
                StartCoroutine("LoadResource");

            }
            else
            {
                thisUnit.Agent.isStopped = false;
                eUnit.agent.isStopped = false;
                thisUnit.MoveTo(warehouseLocation);
                eUnit.MoveTo(warehouseLocation);
                toUnload = true;
                toGather = false;
                yield return null;
            }
        }
        else
        {
            print("Resource depleted");
            res.RemoveResource(res.TotalAmount);
            CarryWheat += res.TotalAmount;
            Destroy(res.gameObject, 3);
            thisUnit.MoveTo(warehouseLocation);
            eUnit.MoveTo(warehouseLocation);
            toUnload = true;
            toGather = false;
            thisUnit.Agent.isStopped = true;
        }
    }
    
    void UnloadWood()
    {
        if(whouse.woodMaxAmount >= CarryWood)
        {
            whouse.woodAmount += CarryWood;
            CarryWood = 0;
            toUnload = false;

            if(resTarget != null)
            {
                if(playerUnit)
                    thisUnit.MoveTo(target);
                else
                    eUnit.MoveTo(target);

                toGather = true;
            }
            else
            {
                print("No target");
                if(playerUnit)
                    thisUnit.Agent.isStopped = true;
                else
                    eUnit.agent.isStopped = true;
            }
        }
        else
        {
            whouse.woodAmount += whouse.woodMaxAmount - whouse.woodAmount;
            CarryWood -= whouse.woodMaxAmount - whouse.woodAmount;

            if(playerUnit)
                thisUnit.Agent.isStopped = true;
            else
                eUnit.agent.isStopped = true;
            print("Warehouse is full - " + cType);
        }
    }

    void UnloadStone()
    {
        if(whouse.stoneMaxAmount >= CarryStone)
        {
            whouse.stoneAmount += CarryStone;
            CarryStone = 0;
            toUnload = false;

            if(resTarget != null)
            {
                if(playerUnit)
                    thisUnit.MoveTo(target);
                else
                    eUnit.MoveTo(target);

                toGather = true;
            }
            else
            {
                print("No target");
                if(playerUnit)
                    thisUnit.Agent.isStopped = true;
                else
                    eUnit.agent.isStopped = true;
            }
        }
        else
        {
            whouse.stoneAmount += whouse.stoneMaxAmount - whouse.stoneAmount;
            CarryStone -= whouse.stoneMaxAmount - whouse.stoneAmount;

            if(playerUnit)
                thisUnit.Agent.isStopped = true;
            else
                eUnit.agent.isStopped = true;
            print("Warehouse is full - " + cType);
        }
    }

    void UnloadBerry()
    {
        if(whouse.berryMaxAmount >= CarryBerry)
        {
            whouse.berryAmount += CarryBerry;
            CarryBerry = 0;
            toUnload = false;

            if(resTarget != null)
            {
                if(playerUnit)
                    thisUnit.MoveTo(target);
                else
                    eUnit.MoveTo(target);

                toGather = true;
            }
            else
            {
                print("No target");
                if(playerUnit)
                    thisUnit.Agent.isStopped = true;
                else
                    eUnit.agent.isStopped = true;
            }
        }
        else
        {
            whouse.berryAmount += whouse.berryMaxAmount - whouse.berryAmount;
            CarryBerry -= whouse.berryMaxAmount - whouse.berryAmount;

            if(playerUnit)
                thisUnit.Agent.isStopped = true;
            else
                eUnit.agent.isStopped = true;
            print("Warehouse is full - " + cType);
        }
    }

    void UnloadMushrooms()
    {
        if(whouse.mushroomsMaxAmount >= CarryMushrooms)
        {
            whouse.mushroomsAmount += CarryMushrooms;
            CarryMushrooms = 0;
            toUnload = false;

            if(resTarget != null)
            {
                if(playerUnit)
                    thisUnit.MoveTo(target);
                else
                    eUnit.MoveTo(target);

                toGather = true;
            }
            else
            {
                print("No target");
                if(playerUnit)
                    thisUnit.Agent.isStopped = true;
                else
                    eUnit.agent.isStopped = true;
            }
        }
        else
        {
            whouse.mushroomsAmount += whouse.mushroomsMaxAmount - whouse.mushroomsAmount;
            CarryMushrooms -= whouse.mushroomsMaxAmount - whouse.mushroomsAmount;

            if(playerUnit)
                thisUnit.Agent.isStopped = true;
            else
                eUnit.agent.isStopped = true;
            print("Warehouse is full - " + cType);
        }
    }

    void UnloadWheat()
    {
        if(whouse.wheatMaxAmount >= CarryWheat)
        {
            whouse.wheatAmount += CarryWheat;
            CarryWheat = 0;
            toUnload = false;

            if(resTarget != null)
            {
                if(playerUnit)
                    thisUnit.MoveTo(target);
                else
                    eUnit.MoveTo(target);

                toGather = true;
            }
            else
            {
                print("No target");
                if(playerUnit)
                    thisUnit.Agent.isStopped = true;
                else
                    eUnit.agent.isStopped = true;
            }
        }
        else
        {
            whouse.wheatAmount += whouse.wheatMaxAmount - whouse.wheatAmount;
            CarryWheat -= whouse.wheatMaxAmount - whouse.wheatAmount;

            if(playerUnit)
                thisUnit.Agent.isStopped = true;
            else
                eUnit.agent.isStopped = true;
            print("Warehouse is full - " + cType);
        }
    }
}
