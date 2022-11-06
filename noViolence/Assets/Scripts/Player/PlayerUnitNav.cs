using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerUnitNav : MonoBehaviour
{
    public static PlayerUnitNav instance;
    NavMeshAgent agent;

    public Vector3 Target;
    public Transform[] TargetPositions;
    public GameObject selectionMarker;
    public float velocity;

    public bool zoomingIn;
    public bool zoomingOut;

    void Start()
    {
        instance = this;
        agent = GetComponent<NavMeshAgent>();
    }
    
    void Update()
    {
        if(transform.position.x == Target.x && transform.position.z == Target.z)
        {
            Target = Vector3.zero;
            StartCoroutine("GoToTarget");
        }
        else
        {
            if(Target != Vector3.zero)
                agent.destination = Target;

        }

        if(zoomingIn)
        {
            if(Camera.main.fieldOfView > 5)
                Camera.main.fieldOfView = Mathf.Lerp(10, 5, 4);
            else
                zoomingIn = false;
        }

        if(zoomingOut)
        {
            if(Camera.main.fieldOfView < 10)
                Camera.main.fieldOfView = Mathf.Lerp(5, 10, 8);
            else
                zoomingOut = false;
        } 
    }

    IEnumerator GoToTarget()
    {
        if(Target == Vector3.zero)
        {
            Target = TargetPositions[Random.Range(0, TargetPositions.Length)].position;
            yield return null;
            
        }
    }

}
