using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

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
    public bool movingToTarget;
    public bool fighting;

    CombatVisualization combatVisualization;

    public VisualEffect smokePoofSmall;

    void Start()
    {
        combatVisualization = GetComponent<CombatVisualization>();
        if (isPlayerUnit)
        {
            thisUnitP = GetComponentInParent<PlayerUnit>();
        }
        else
        {
            thisUnitE = GetComponentInParent<EnemyUnit>();
        }

        StartCoroutine(CheckForEnemiesAround());
    }

    IEnumerator CheckForEnemiesAround()
    {
        if (Physics.SphereCast(transform.position, 20, transform.forward, out cHit))
        {
            if (isPlayerUnit)
            {
                if (cHit.collider.CompareTag("EnemyUnit") && !fighting)
                {
                    if (!SelectionManager.instance.SelectedUnits.Contains(thisUnitP.GetComponent<SelectableUnit>()) || !SelectionManager.instance.AvailableUnits.Contains(thisUnitP.GetComponent<SelectableUnit>()))
                    {
                        if (pUnitTarget != null && !cHit.collider.GetComponent<EnemyUnit>().pUnitsFighWith.Contains(thisUnitP))
                        {
                            fighting = true;
                            thisUnitP.CancelAllActions();

                            pUnitTarget.pUnitsFighWith.Add(thisUnitP);
                            pUnitTarget = cHit.collider.GetComponentInParent<EnemyUnit>();

                            StartCombat();
                            print("Enemy close. Defending");
                        }
                    }
                }
            }
            else
            {
                if (cHit.collider.CompareTag("PlayerUnit"))
                {
                    if (eUnitTarget == null && !fighting && cHit.collider.GetComponent<PlayerUnit>().unitModel.activeInHierarchy)
                    {
                        fighting = true;
                        thisUnitE.CancelAllActions();

                        eUnitTarget.eUnitsFighWith.Add(thisUnitE);
                        eUnitTarget = cHit.collider.GetComponent<PlayerUnit>();

                        StartCombat();
                        print("Player unit close. Defending");
                    }
                }
            }
        }
        else
        {
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(CheckForEnemiesAround());
        }
    }

    public void StartCombat()
    {
        StartCoroutine("MovingToTarget");
    }

    IEnumerator MovingToTarget()
    {
        if (isPlayerUnit)
        {
            if (pUnitTarget != null)
            {
                thisUnitP.GetComponent<SelectableUnit>().MoveTo(pUnitTarget.transform.position);

                distanceToTarget = Vector3.Distance(pUnitTarget.transform.position, thisUnitP.transform.position);
                if (distanceToTarget < 10)
                {
                    StartCoroutine(PlayerConversion());
                }
                else
                {
                    yield return new WaitForSeconds(1f);
                    StartCoroutine(MovingToTarget());
                }
            }
        }
        else
        {
            if (eUnitTarget != null)
            {
                thisUnitP.GetComponent<SelectableUnit>().MoveTo(pUnitTarget.transform.position);

                distanceToTarget = Vector3.Distance(pUnitTarget.transform.position, thisUnitP.transform.position);
                if (distanceToTarget < 10)
                {
                    StartCoroutine(EnemyAttack());
                }
                else
                {
                    yield return new WaitForSeconds(1f);
                    StartCoroutine(MovingToTarget());
                }
            }
        }
    }

    IEnumerator PlayerConversion()
    {
        if (pUnitTarget != null)
        {
            distanceToTarget = Vector3.Distance(pUnitTarget.transform.position, thisUnitP.transform.position);

            if (distanceToTarget <= 25)
            {
                fighting = true;

                targetPos = new Vector3(
                targetPos.x + 5 * Mathf.Cos(2 * Mathf.PI * pUnitTarget.pUnitsFighWith.IndexOf(thisUnitP) / pUnitTarget.pUnitsFighWith.Count),
                targetPos.y,
                targetPos.z + 5 * Mathf.Sin(2 * Mathf.PI * pUnitTarget.pUnitsFighWith.IndexOf(thisUnitP) / pUnitTarget.pUnitsFighWith.Count));
                thisUnitP.GetComponent<SelectableUnit>().MoveTo(pUnitTarget.transform.position);

                if (pUnitTarget.euType == EnemyUnit.eUnitType.Basic)
                {
                    resistanceDamage = (thisUnitP.strLevel * 0.33f) - (pUnitTarget.strLevel / 5) + (thisUnitP.intLevel * 0.33f) - (pUnitTarget.intLevel / 5) + (thisUnitP.skillLevel * 0.33f) - (pUnitTarget.jllnsLevel / 5);
                }
                else if (pUnitTarget.euType == EnemyUnit.eUnitType.Strong)
                {
                    resistanceDamage = (thisUnitP.strLevel * 0.6f) - (pUnitTarget.strLevel / 5) + (thisUnitP.intLevel * 0.3f) - (pUnitTarget.intLevel / 5) + (thisUnitP.skillLevel * 0.1f) - (pUnitTarget.jllnsLevel / 5);
                }
                else if (pUnitTarget.euType == EnemyUnit.eUnitType.Smart)
                {
                    resistanceDamage = (thisUnitP.strLevel * 0.3f) - (pUnitTarget.strLevel / 5) + (thisUnitP.intLevel * 0.6f) - (pUnitTarget.intLevel / 5) + (thisUnitP.skillLevel * 0.3f) - (pUnitTarget.jllnsLevel / 5);
                }
                else if (pUnitTarget.euType == EnemyUnit.eUnitType.Joyful)
                {
                    resistanceDamage = (thisUnitP.strLevel * 0.1f) - (pUnitTarget.strLevel / 5) + (thisUnitP.intLevel * 0.3f) - (pUnitTarget.intLevel / 5) + (thisUnitP.skillLevel * 0.6f) - (pUnitTarget.jllnsLevel / 5);
                }

                if (pUnitTarget.Resistance > resistanceDamage)
                {
                    if (distanceToTarget < 5)
                    {
                        pUnitTarget.Resistance -= resistanceDamage;
                        combatVisualization.ShootObject(pUnitTarget.transform);
                    }
                }
                else
                {
                    ConvertEnemyUnit();
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
            if (thisUnitE.euType == EnemyUnit.eUnitType.Basic)
            {
                enemyDamage = (thisUnitE.strLevel * 1f);
            }
            else if (thisUnitE.euType == EnemyUnit.eUnitType.Strong)
            {
                enemyDamage = (thisUnitE.strLevel * 1.3f);
            }
            else if (thisUnitE.euType == EnemyUnit.eUnitType.Smart)
            {
                enemyDamage = (thisUnitE.strLevel * 0.5f);
            }
            else if (thisUnitE.euType == EnemyUnit.eUnitType.Joyful)
            {
                enemyDamage = (thisUnitE.strLevel * 0.7f);
            }

            if (eUnitTarget.Health > enemyDamage)
            {
                eUnitTarget.Health -= enemyDamage;
            }
            else
            {
                DestroyPlayerUnit();
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

    public void ConvertEnemyUnit()
    {
        if (pUnitTarget != null)
        {
            pUnitTarget.transform.parent.position = pUnitTarget.transform.position;
            pUnitTarget.eCombatDetection.playerUnitPrefab.SetActive(true);
            pUnitTarget.eCombatDetection.smokePoofSmall.Play();

            PlayerUnit futurePlayerUnit = pUnitTarget.eCombatDetection.playerUnitPrefab.GetComponent<PlayerUnit>();
            pUnitTarget.GetComponentInChildren<CombatDetection>().playerUnitPrefab.transform.position = pUnitTarget.transform.position;
            pUnitTarget.GetComponentInChildren<CombatDetection>().playerUnitPrefab.transform.parent.SetParent(PlayerManager.instance.playerUnitHolder.transform);

            futurePlayerUnit.Name = pUnitTarget.Name;
            futurePlayerUnit.Health = pUnitTarget.Health;
            if (pUnitTarget.strLevel > 5)
                futurePlayerUnit.strLevel = pUnitTarget.strLevel < 15 ? pUnitTarget.strLevel -= Random.Range(1, 3) : pUnitTarget.strLevel -= Random.Range(3, 6);
            if (pUnitTarget.intLevel > 5)
                futurePlayerUnit.intLevel = futurePlayerUnit.intLevel < 15 ? pUnitTarget.intLevel -= Random.Range(1, 3) : pUnitTarget.intLevel -= Random.Range(3, 6);
            if (pUnitTarget.jllnsLevel > 5)
                futurePlayerUnit.skillLevel = futurePlayerUnit.skillLevel < 15 ? pUnitTarget.jllnsLevel -= Random.Range(1, 3) : pUnitTarget.jllnsLevel -= Random.Range(3, 6);

            Destroy(pUnitTarget.gameObject);
            print("Unit converted...");
            FindNextTarget();
        }
    }

    void DestroyPlayerUnit()
    {
        smokePoofSmall.gameObject.transform.position = transform.position;
        smokePoofSmall.Play();

        SelectionManager.instance.SelectedUnits.Remove(eUnitTarget.GetComponent<SelectableUnit>());
        SelectionManager.instance.AvailableUnits.Remove(eUnitTarget.GetComponent<SelectableUnit>());

        eUnitTarget.CancelAllActions();
        StopCoroutine("EnemyAttack");
        Destroy(eUnitTarget.gameObject);
        FindNextTarget();
    }

    void FindNextTarget()
    {
        if (isPlayerUnit)
        {
            thisUnitP.CancelCombat();

            if (Physics.SphereCast(transform.position, 20, transform.forward, out cHit))
            {
                if (cHit.collider.CompareTag("EnemyUnit"))
                {
                    thisUnitP.GetComponent<SelectableUnit>().MoveTo(cHit.point);
                }
                else
                {
                    print("No enemy units in reach. Awaiting orders.");
                }
            }
        }
        else
        {
            thisUnitE.CancelCombat();

            if (Physics.SphereCast(transform.position, 20, transform.forward, out cHit))
            {
                if (cHit.collider.CompareTag("EnemyUnit"))
                {
                    thisUnitP.GetComponent<SelectableUnit>().MoveTo(cHit.point);
                }
                else
                {
                    print("No enemy units in reach. Awaiting orders.");
                }
            }
        }
    }

}
