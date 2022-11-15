using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : MonoBehaviour
{
    public GameObject unitModel;
    public string Name;
    public float Health;

    public int level;
    public float exp;
    public float expNeeded;

    [Header("Attributes")]
    public int atPoints;
    public int intLevel;
    public int strLevel;
    public int skillLevel;
    public float attackRate;

    void Start()
    {
        
    }

    public void AddExperience(int amount)
    {
        if (exp + amount < expNeeded)
        {
            exp += amount;
        }
        else
        {
            level++;
            atPoints++;
            exp = 0;
            expNeeded *= 1.5f;
        }
    }

}
