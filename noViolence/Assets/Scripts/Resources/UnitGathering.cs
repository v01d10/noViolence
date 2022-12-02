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

    [Header("Base Stats")]
    public float MaxCarryAmount;
    public float TotalCarryAmount;
    public float CarryWood;
    public float CarryStone;
    public float CarryBerry;
    public float CarryMushrooms;

    public float GatherTime;
    public float gatherAmount;
    public float RadiusAroundResource = 1.5f;
    float distanceToTarget;

    public bool toGather;
    public bool toUnload;
    public bool playerUnit;
    bool facingRes;

    [Header("Units")]
    SelectableUnit thisUnit;
    EnemyUnitNav eUnit;
    public Resource resTarget;
    Warehouse whouse;
    public Vector3 warehouseLocation;
    public Vector3 targetPosition;
    EnemyManager eManager;
    RaycastHit fHit;
    public AudioSource gatheringSource;

    void Start()
    {
        gatheringSource = GetComponentInParent<AudioSource>();

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

    public void StartGathering()
    {
        if (resTarget != null && resTarget.Gatherers.Contains(this))
        {
            toGather = true;

            if (resTarget.rType == Resource.ResType.Wood)
            {
                cType = CarryType.Wood;
                StartCoroutine("LoadWood");
            }
            else if (resTarget.rType == Resource.ResType.Stone)
            {
                cType = CarryType.Stone;
                StartCoroutine("LoadStone");
            }
            else if (resTarget.rType == Resource.ResType.Berry)
            {
                cType = CarryType.Berry;
                StartCoroutine("LoadBerry");
            }

            if (!playerUnit)
            {
                eUnit.checkIfGathering();
            }
        }
    }

    IEnumerator LoadWood()
    {
        distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        TotalCarryAmount = CarryWood + CarryStone + CarryBerry + CarryMushrooms;

        if (distanceToTarget < 3)
        {
            PlayGatherSound(sfxManager.instance.treeChopings[Random.Range(0, sfxManager.instance.treeChopings.Length)]);
            
            if (resTarget.TotalAmount >= gatherAmount)
            {

                if (TotalCarryAmount + gatherAmount <= MaxCarryAmount)
                {
                    if (playerUnit)
                        thisUnit.Agent.isStopped = true;
                    else
                        eUnit.agent.isStopped = true;

                    print("Gathering " + cType);
                    resTarget.RemoveResource(gatherAmount);
                    CarryWood += gatherAmount;
                    thisUnit.thisPunit.exp += thisUnit.thisPunit.expNeeded / 1000;

                    yield return new WaitForSeconds(GatherTime);
                    StartGathering();
                }
                else
                {
                    GoToWarehouse();
                }
            }
            else
            {
                CarryWood += resTarget.TotalAmount;
                ResourceDepleted();
            }
        }
        else
        {
            yield return new WaitForSeconds(GatherTime * 0.5f);
            StartGathering();
        }

    }

    IEnumerator LoadStone()
    {
        distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        TotalCarryAmount = CarryWood + CarryStone + CarryBerry + CarryMushrooms;

        if (distanceToTarget < 3)
        {
            if (resTarget.TotalAmount >= gatherAmount)
            {
                if (TotalCarryAmount + gatherAmount <= MaxCarryAmount)
                {
                    if (playerUnit)
                        thisUnit.Agent.isStopped = true;
                    else
                        eUnit.agent.isStopped = true;

                    print("Gathering " + cType);
                    resTarget.RemoveResource(gatherAmount);
                    CarryStone += gatherAmount;
                    thisUnit.thisPunit.exp += thisUnit.thisPunit.expNeeded / 1000;

                    yield return new WaitForSeconds(GatherTime);
                    StartGathering();
                }
                else
                {
                    GoToWarehouse();
                }
            }
            else
            {
                CarryStone += resTarget.TotalAmount;
                StartGathering();
            }
        }
        else
        {
            yield return new WaitForSeconds(GatherTime * 0.5f);
            StartCoroutine(LoadStone());
        }

    }

    IEnumerator LoadBerry()
    {
        distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        TotalCarryAmount = CarryWood + CarryStone + CarryBerry + CarryMushrooms;

        if (distanceToTarget < 3)
        {
            if (resTarget.TotalAmount >= gatherAmount)
            {
                if (TotalCarryAmount + gatherAmount <= MaxCarryAmount)
                {
                    if (playerUnit)
                        thisUnit.Agent.isStopped = true;
                    else
                        eUnit.agent.isStopped = true;

                    print("Gathering " + cType);
                    resTarget.RemoveResource(gatherAmount);
                    CarryWood += gatherAmount;
                    thisUnit.thisPunit.exp += thisUnit.thisPunit.expNeeded / 1000;

                    yield return new WaitForSeconds(GatherTime);
                    StartGathering();
                }
                else
                {
                    GoToWarehouse();
                }
            }
            else
            {
                CarryWood += resTarget.TotalAmount;
                ResourceDepleted();
            }
        }
        else
        {
            yield return new WaitForSeconds(GatherTime * 0.5f);
            StartGathering();
        }

    }

    IEnumerator UnloadResouce()
    {
        distanceToTarget = Vector3.Distance(transform.position, warehouseLocation);

        if (distanceToTarget < 3)
        {
            UnloadWood();
            UnloadStone();
            UnloadBerry();
            UnloadMushrooms();

            TotalCarryAmount = CarryWood + CarryStone + CarryBerry + CarryMushrooms;
            Warehouse.instance.LoadResourceText();
        }
        else
        {
            print("No warehouse in reach");
            yield return new WaitForSeconds(0.3f);
            StartCoroutine("UnloadResouce");
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
        if (whouse.shroomMaxAmount >= CarryMushrooms)
        {
            whouse.shroomAmount += CarryMushrooms;
            CarryMushrooms = 0;
            toUnload = false;

            CheckForTarget();
        }
        else
        {
            whouse.shroomAmount += whouse.shroomMaxAmount - whouse.shroomAmount;
            CarryMushrooms -= whouse.shroomMaxAmount - whouse.shroomAmount;

            if (playerUnit)
                thisUnit.Agent.isStopped = true;
            else
                eUnit.agent.isStopped = true;
            print("Warehouse is full - " + cType);
        }
    }

    void PlayGatherSound(AudioClip clip)
    {
        gatheringSource.clip = clip;
        gatheringSource.Play();
    }


    void ResourceDepleted()
    {
        resTarget.RemoveResource(resTarget.TotalAmount);
        Destroy(resTarget.gameObject);

        if (TotalCarryAmount < MaxCarryAmount)
        {
            if (Physics.SphereCast(transform.position, 80, transform.forward, out fHit))
            {
                if (fHit.collider.CompareTag("Tree"))
                {
                    print("Found " + fHit.collider.tag);

                    resTarget = fHit.collider.GetComponent<Resource>();
                    resTarget.Gatherers.Add(this);
                    targetPosition = new Vector3(
                                fHit.point.x + RadiusAroundResource * 2 * Mathf.Cos(2 * Mathf.PI * resTarget.Gatherers.IndexOf(this) / resTarget.Gatherers.Count),
                                fHit.point.y,
                                fHit.point.z + RadiusAroundResource * 2 * Mathf.Sin(2 * Mathf.PI * resTarget.Gatherers.IndexOf(this) / resTarget.Gatherers.Count));

                    if (playerUnit)
                    {
                        thisUnit.MoveTo(fHit.point);
                    }
                    else
                    {

                        eUnit.Gathering = true;
                        eUnit.MoveTo(fHit.point);
                    }
                }
                else
                    print("No trees in reach");
            }
        }
        else
        {
            GoToWarehouse();
            print("Resource depleted");
            toUnload = true;
            toGather = false;
        }


    }

    void GoToWarehouse()
    {
        gatheringSource.Stop();
        StartCoroutine(UnloadResouce());
        if (playerUnit)
        {
            thisUnit.MoveTo(warehouseLocation);
        }
        else
        {
            eUnit.MoveTo(warehouseLocation);
        }

        toUnload = true;
        toGather = false;
    }

    void CheckForTarget()
    {
        StopAllCoroutines();

        if (resTarget != null)
        {
            if (playerUnit)
                thisUnit.MoveTo(targetPosition);
            else
                eUnit.MoveTo(targetPosition);

            StartGathering();
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

}
