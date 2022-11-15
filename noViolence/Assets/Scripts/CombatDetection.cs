using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatDetection : MonoBehaviour
{
    EnemyUnit thisUnitE;
    PlayerUnit thisUnitP;

    public PlayerUnit eUnitTarget;
    public EnemyUnit pUnitTarget;

    public GameObject playerUnitPrefab;
    public GameObject enemyUnitPrefab;

    float distanceToTarget;
    float resistanceDamage;
    float enemyDamage;

    Vector3 targetPos;
    RaycastHit cHit;

    public bool isPlayerUnit;
    bool fighting;

    void Start()
    {
        if (isPlayerUnit)
        {
            thisUnitP = GetComponentInParent<PlayerUnit>();
        }
        else
        {
            thisUnitE = GetComponentInParent<EnemyUnit>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isPlayerUnit)
        {
            if (other.CompareTag("EnemyUnit"))
            {
                if (pUnitTarget == null && !fighting)
                {
                    pUnitTarget = other.GetComponentInParent<EnemyUnit>();
                    pUnitTarget.pUnitsFighWith.Add(thisUnitP);
                    StartCoroutine("PlayerConversion");
                    fighting = true;
                }
            }
        }
        else
        {
            if (other.CompareTag("PlayerUnit"))
            {
                if (eUnitTarget == null && !fighting)
                {
                    fighting = true;
                    thisUnitE.CancelGathering();
                    eUnitTarget = other.GetComponent<PlayerUnit>();
                    StartCoroutine("EnemyAttack");
                }
            }
        }
    }

    IEnumerator PlayerConversion()
    {
        distanceToTarget = Vector3.Distance(pUnitTarget.transform.position, thisUnitP.transform.position);

        if (distanceToTarget <= 50)
        {
            if (pUnitTarget != null)
            {
                if (pUnitTarget.enemyType == EnemyUnit.EnemyType.Basic)
                {
                    resistanceDamage = (thisUnitP.strLevel * 0.33f) - (pUnitTarget.strLevel / 5) + (thisUnitP.intLevel * 0.33f) - (pUnitTarget.intLevel / 5) + (thisUnitP.skillLevel * 0.33f) - (pUnitTarget.jllnsLevel / 5);

                    targetPos = new Vector3(
                    targetPos.x + 5 * Mathf.Cos(2 * Mathf.PI * pUnitTarget.pUnitsFighWith.IndexOf(thisUnitP) / pUnitTarget.pUnitsFighWith.Count),
                    targetPos.y,
                    targetPos.z + 5 * Mathf.Sin(2 * Mathf.PI * pUnitTarget.pUnitsFighWith.IndexOf(thisUnitP) / pUnitTarget.pUnitsFighWith.Count));
                    thisUnitP.GetComponent<SelectableUnit>().MoveTo(pUnitTarget.transform.position);

                    if (pUnitTarget.Resistance > resistanceDamage)
                        pUnitTarget.Resistance -= resistanceDamage;
                    else
                    {
                        ConvertEnemyUnit();
                    }

                }
                else if (pUnitTarget.enemyType == EnemyUnit.EnemyType.Strong)
                {
                    resistanceDamage = ((thisUnitP.strLevel * 0.6f) - pUnitTarget.strLevel) + ((thisUnitP.intLevel * 0.1f) - pUnitTarget.intLevel) + ((thisUnitP.skillLevel * 0.3f) - pUnitTarget.jllnsLevel);

                    if (pUnitTarget.Resistance > resistanceDamage)
                        pUnitTarget.Resistance -= resistanceDamage;
                }
                else if (pUnitTarget.enemyType == EnemyUnit.EnemyType.Smart)
                {
                    resistanceDamage = ((thisUnitP.strLevel * 0.1f) - pUnitTarget.strLevel) + ((thisUnitP.intLevel * 0.6f) - pUnitTarget.intLevel) + ((thisUnitP.skillLevel * 0.3f) - pUnitTarget.jllnsLevel);

                    if (pUnitTarget.Resistance > resistanceDamage)
                        pUnitTarget.Resistance -= resistanceDamage;
                }
                else if (pUnitTarget.enemyType == EnemyUnit.EnemyType.Joyful)
                {
                    resistanceDamage = ((thisUnitP.strLevel * 0.3f) - pUnitTarget.strLevel) + ((thisUnitP.intLevel * 0.1f) - pUnitTarget.intLevel) + ((thisUnitP.skillLevel * 0.6f) - pUnitTarget.jllnsLevel);

                    if (pUnitTarget.Resistance > resistanceDamage)
                        pUnitTarget.Resistance -= resistanceDamage;
                }

                yield return new WaitForSeconds(thisUnitP.attackRate);
                StartCoroutine("PlayerConversion");

            }
        }
        else
        {
            print("Target lost");
            pUnitTarget.pUnitsFighWith.Remove(thisUnitP);
            pUnitTarget = null;
            StopCoroutine("PlayerConversion");
            thisUnitP.GetComponent<SelectableUnit>().Agent.isStopped = true;
        }
    }

    IEnumerator EnemyAttack()
    {
        if (eUnitTarget != null)
        {
            if (thisUnitE.enemyType == EnemyUnit.EnemyType.Basic)
            {
                enemyDamage = (thisUnitE.strLevel * 1f);
                if(eUnitTarget.Health > enemyDamage)
                    eUnitTarget.Health -= enemyDamage;
                else
                {
                    Destroy(eUnitTarget.gameObject);
                }
            }
            else if (thisUnitE.enemyType == EnemyUnit.EnemyType.Strong)
            {
                enemyDamage = (thisUnitE.strLevel * 1.3f);
                if(eUnitTarget.Health > enemyDamage)
                    eUnitTarget.Health -= enemyDamage;
                else
                {
                    Destroy(eUnitTarget.gameObject);
                }
            }
            else if (thisUnitE.enemyType == EnemyUnit.EnemyType.Smart)
            {
                enemyDamage = (thisUnitE.strLevel * 0.5f);
                if(eUnitTarget.Health > enemyDamage)
                    eUnitTarget.Health -= enemyDamage;
                else
                {
                    Destroy(eUnitTarget.gameObject);
                }
            }
            else if (thisUnitE.enemyType == EnemyUnit.EnemyType.Joyful)
            {
                enemyDamage = (thisUnitE.strLevel * 0.7f);
                if(eUnitTarget.Health > enemyDamage)
                    eUnitTarget.Health -= enemyDamage;
                else
                {
                    Destroy(eUnitTarget.gameObject);
                }
            }

            yield return new WaitForSeconds(thisUnitE.attackRate);
            StartCoroutine("EnemyAttack");
        }
        else
        {
            print("No Player units in reach");

            yield return null;
        }
    }

    void ConvertEnemyUnit()
    {
        PlayerUnit futurePlayerUnit = pUnitTarget.GetComponentInChildren<PlayerUnit>();
        pUnitTarget.GetComponentInChildren<CombatDetection>().playerUnitPrefab.SetActive(true);
        for (int i = 0; i < pUnitTarget.pUnitsFighWith.Count; i++)
        {
            pUnitTarget.pUnitsFighWith[i].GetComponentInChildren<CombatDetection>().pUnitTarget = null;
        }

        futurePlayerUnit.Name = pUnitTarget.Name;
        futurePlayerUnit.Health = pUnitTarget.Health;
        futurePlayerUnit.strLevel = futurePlayerUnit.strLevel < 15 ? pUnitTarget.strLevel -= Random.Range(1, 3) : pUnitTarget.strLevel -= Random.Range(3, 6);
        futurePlayerUnit.intLevel = futurePlayerUnit.intLevel < 15 ? pUnitTarget.intLevel -= Random.Range(1, 3) : pUnitTarget.intLevel -= Random.Range(3, 6);
        futurePlayerUnit.skillLevel = futurePlayerUnit.skillLevel < 15 ? pUnitTarget.jllnsLevel -= Random.Range(1, 3) : pUnitTarget.jllnsLevel -= Random.Range(3, 6);

        Destroy(pUnitTarget.GetComponentInChildren<CombatDetection>().enemyUnitPrefab);
    }

    void FindNextTarget()
    {
        if(isPlayerUnit)
        {
            if(Physics.SphereCast(transform.position, 20, transform.forward, out cHit))
            {
                if(cHit.collider.CompareTag("EnemyUnit"))
                {
                    thisUnitP.GetComponent<SelectableUnit>().MoveTo(cHit.point);
                }
            }

        }
    }

}
