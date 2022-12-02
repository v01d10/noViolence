using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SelectableUnit : MonoBehaviour
{
    public NavMeshAgent Agent;
    [SerializeField]
    private SpriteRenderer SelectionSprite;
    public PlayerUnit thisPunit;


    private void Awake()
    {
        SelectionManager.instance.AvailableUnits.Add(this);
        Agent = GetComponent<NavMeshAgent>();
        thisPunit = GetComponent<PlayerUnit>();
    }

    public void MoveTo(Vector3 Position)
    {
        Agent.isStopped = false;
        Agent.SetDestination(Position);
    }

    public void OnSelected()
    {
        SelectionSprite.gameObject.SetActive(true);
    }

    public void OnDeselected()
    {
        SelectionSprite.gameObject.SetActive(false);
    }
}
