using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    public int Hours;
    public int Days;
    public float waitTime;
    public float timeIncrement;
    private float fixedDeltaTime;

    public TextMeshProUGUI HourText;
    public TextMeshProUGUI DayText;

    public Button PlayButton;
    public Button PauseButton;
    public Button FastFwrdButton;
    public Button FasterFwrdButton;

    void Awake()
    {
        instance = this;
        StartCoroutine("HandleTime");

        PlayButton.onClick.AddListener(Play);
        PauseButton.onClick.AddListener(Pause);
        FastFwrdButton.onClick.AddListener(FastForward);
        FasterFwrdButton.onClick.AddListener(FasterForward);

        this.fixedDeltaTime = Time.fixedDeltaTime;
    }

    void Update()
    {
        Time.timeScale = timeIncrement;
    }

    void Play()
    {
        timeIncrement = 1;
        PlayButton.gameObject.SetActive(false);

        if (!FastFwrdButton.gameObject.activeInHierarchy)
            FastFwrdButton.gameObject.SetActive(true);
        if (!PauseButton.gameObject.activeInHierarchy)
            PauseButton.gameObject.SetActive(true);
        if (!FasterFwrdButton.gameObject.activeInHierarchy)
            FasterFwrdButton.gameObject.SetActive(true);
    }

    void Pause()
    {
        timeIncrement = 0;
        PauseButton.gameObject.SetActive(false);

        if (!PlayButton.gameObject.activeInHierarchy)
            PlayButton.gameObject.SetActive(true);
        if (!FastFwrdButton.gameObject.activeInHierarchy)
            FastFwrdButton.gameObject.SetActive(true);
        if (!FasterFwrdButton.gameObject.activeInHierarchy)
            FasterFwrdButton.gameObject.SetActive(true);
    }

    void FastForward()
    {
        timeIncrement = 2.5f;
        FastFwrdButton.gameObject.SetActive(false);

        if (!PlayButton.gameObject.activeInHierarchy)
            PlayButton.gameObject.SetActive(true);
        if (!PauseButton.gameObject.activeInHierarchy)
            PauseButton.gameObject.SetActive(true);
        if (!FasterFwrdButton.gameObject.activeInHierarchy)
            FasterFwrdButton.gameObject.SetActive(true);

    }

    void FasterForward()
    {
        timeIncrement = 5f;
        FasterFwrdButton.gameObject.SetActive(false);

        if (!PlayButton.gameObject.activeInHierarchy)
            PlayButton.gameObject.SetActive(true);
        if (!PauseButton.gameObject.activeInHierarchy)
            PauseButton.gameObject.SetActive(true);
        if (!FastFwrdButton.gameObject.activeInHierarchy)
            FastFwrdButton.gameObject.SetActive(true);

    }

    IEnumerator HandleTime()
    {
        if (Hours < 23)
        {
            NewHour();
            yield return new WaitForSeconds(waitTime);
            StartCoroutine("HandleTime");
        }
        else
        {
            NewDay();
            yield return new WaitForSeconds(waitTime);
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
