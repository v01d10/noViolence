using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;
    
    public ObjectPool heartPool;    
    public ObjectPool notePool; 

    void Awake()
    {
        instance = this;
    }
}
