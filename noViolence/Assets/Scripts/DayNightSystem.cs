using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightSystem : MonoBehaviour
{
    public float dayLengthMinutes;

    float rotationSpeed;

    void Start()
    {
        rotationSpeed = 360 / dayLengthMinutes / 60;
    }

    void Update()
    {
        transform.Rotate(new Vector3(1, 0, 0) * rotationSpeed * Time.deltaTime);
    }
}
