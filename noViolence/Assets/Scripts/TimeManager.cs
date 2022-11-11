using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public int Hours;
    public int Days;
    public int DayTimer;

    public TextMeshProUGUI HourText;
    public TextMeshProUGUI DayText;

    void Start()
    {
        StartCoroutine("HandleTime");
    }

    IEnumerator HandleTime()
    {
        if (Hours < 23)
        {
            NewHour();
            yield return new WaitForSeconds(DayTimer);
            StartCoroutine("HandleTime");
        }
        else
        {
            NewDay();
            yield return new WaitForSeconds(DayTimer);
            StartCoroutine("HandleTime");
        }
    }

    void NewHour()
    {
        Hours++;
        HandleTimeText();
    }

    void NewDay()
    {
        Days++;
        Hours = 0;
        HandleTimeText();
    }

    void HandleTimeText()
    {
        HourText.text = Hours < 10 ? "0" + Hours.ToString() + " : 00" : Hours.ToString() + " : 00";
        DayText.text = "Day: " + Days.ToString();
    }
}
