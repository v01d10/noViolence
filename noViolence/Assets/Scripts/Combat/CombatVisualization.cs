using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CombatVisualization : MonoBehaviour
{
    EnemyUnit thisUnitE;
    PlayerUnit thisUnitP;

    GameObject objToShoot;

    ObjectPool heartPool;
    ObjectPool notePool;

    public float JumpPower;
    public float Duration;

    void Start()
    {
        if(GetComponent<CombatDetection>().isPlayerUnit)
        {
            thisUnitP = GetComponentInParent<PlayerUnit>();
        }
        else
        {
            thisUnitE = GetComponentInParent<EnemyUnit>();
        }

        heartPool = ObjectPoolManager.instance.heartPool;
        notePool = ObjectPoolManager.instance.notePool;
    }

    public void ShootObject(Transform target)
    {
        if (thisUnitP != null)
        {
            if (thisUnitP.puType == PlayerUnit.pUnitType.Basic)
                objToShoot = heartPool.GetPooledObject();
            else if(thisUnitP.puType == PlayerUnit.pUnitType.Joyful)
                objToShoot = notePool.GetPooledObject();

            if (objToShoot != null)
            {
                objToShoot.transform.position = transform.position;
                objToShoot.transform.rotation = transform.rotation;
                objToShoot.SetActive(true);
                objToShoot.GetComponent<CombatProjectile>().InvokeDisabling();
                MoveObject(objToShoot, target);
            }
        }
        else if(thisUnitE != null )
        {

        }
    }

    public void MoveObject(GameObject obj, Transform _target)
    {
        if (obj || _target != null)
        {
            objToShoot.transform.DOJump(_target.position, JumpPower, 1, Duration);
        }
    }
}
