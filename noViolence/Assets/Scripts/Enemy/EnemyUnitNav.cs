using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyUnitNav : MonoBehaviour
{
    public float walkRadius;
    public float resDetectionRad = 10;

    public Vector3 randomDirection;

    NavMeshHit hit;
    public NavMeshAgent agent;

    public bool LookingForRes;
    public bool Gathering;

    public UnitGathering unitG;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        unitG = GetComponentInChildren<UnitGathering>();
        unitG.enabled = false;
    
        StartCoroutine("WalkAround");
    }

    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawSphere(transform.position, resDetectionRad);
    // }

    public void MoveTo(Vector3 location)
    {
        agent.SetDestination(location);
    }

    public void checkIfGathering()
    {
        StartCoroutine("CheckIfGathering");
    }

    IEnumerator WalkAround()
    {
        if(LookingForRes)
        {
            randomDirection = Random.insideUnitSphere * walkRadius;
            randomDirection += transform.position;
            NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
            Vector3 finalPosition = hit.position;
            MoveTo(finalPosition);
            StartCoroutine("CheckForResource");

            yield return new WaitForSeconds(3);
            StartCoroutine("WalkAround");

        }
        else
        {
            yield return new WaitForSeconds(1);
            StartCoroutine("WalkAround");
        }

    }

    IEnumerator CheckForResource()
    {
        RaycastHit fHit;
        print("Checking for resource");

        if(Physics.SphereCast(transform.position, resDetectionRad, transform.forward, out fHit))
        {
            if(fHit.collider.CompareTag("Tree"))
            {
                print("Found " + fHit.collider.tag);
                MoveTo(fHit.point);
                unitG.resTarget = fHit.collider.GetComponent<Resource>();
                unitG.resTarget.Gatherers.Add(unitG);
                unitG.targetPosition = fHit.point;
                unitG.enabled = true;
                LookingForRes = false;
                Gathering = true;
                StopCoroutine("WalkAround");
                yield return null;
            }
            else 
                yield return null;
        }

        if(LookingForRes)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine("CheckForResource");
        }
    }



    IEnumerator CheckIfGathering()
    {
        if(unitG.resTarget != null)
        {
            yield return new WaitForSeconds(1);
            StartCoroutine("CheckIfGathering");
        }
        else
        {
            StartCoroutine("WalkAround");
        }
    }
}
