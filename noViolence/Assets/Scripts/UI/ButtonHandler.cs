using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ButtonHandler : MonoBehaviour, IPointerEnterHandler
{
    Button thisButton;

    void Awake()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(ButtonHighlight);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ButtonHighlight();

    }

    public void ButtonHighlight()
    {
        LeanTween.scale(gameObject, new Vector3(0.8f, 0.8f, 0.8f), 0.1f).setOnComplete(() =>
        {
            LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.1f);
        });
    }

    public void PlayClickSound()
    {
        sfxManager.instance.PlayClickSound();
    }

}
