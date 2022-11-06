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
    public UnitGathering unitGathering;

    private void Awake()
    {
        SelectionManager.instance.AvailableUnits.Add(this);
        Agent = GetComponent<NavMeshAgent>();
        unitGathering = GetComponentInChildren<UnitGathering>();
    }

    public void MoveTo(Vector3 Position)
    {
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
