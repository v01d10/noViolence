using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class MainMenuHandler : MonoBehaviour
{
    public TextMeshProUGUI PlayText;
    public TextMeshProUGUI SettingsText;
    public TextMeshProUGUI QuitText;

    [SerializeField] RectTransform fader;

    public AudioClip firstTimePlaySound;
    public AudioSource sfxSource;

    public bool firstTime;

    void Start()
    {
        fader.gameObject.SetActive(true);

        LeanTween.alpha(fader, 0, 0);
        LeanTween.alpha(fader, 1, 0.5f).setOnComplete(() =>
        {
            fader.gameObject.SetActive(false);
        });
    }

    public void PlayGame()
    {
        if (firstTime)
        {
            sfxSource.clip = firstTimePlaySound;
            sfxSource.volume = 0.8f;
            sfxSource.Play();
            firstTime = false;
        }
        fader.gameObject.SetActive(true);
        LeanTween.alpha(fader, 0, 0);
        LeanTween.alpha(fader, 1, 0.5f).setOnComplete(() =>
        {
            Invoke("LoadScene", 0.5f);
        });

    }

    void LoadScene()
    {
        SceneManager.LoadScene("Main");
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
