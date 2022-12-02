using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Notifier : MonoBehaviour
{
    public static Notifier instance;
    public GameObject notificationText;

    void Start()
    {

        instance = this;

    }

    public void Notify(string message)
    {
        GameObject newNotification = Instantiate(notificationText);
        newNotification.GetComponent<RectTransform>().SetParent(instance.transform);
        newNotification.GetComponent<RectTransform>().localScale = Vector3.one;
        newNotification.GetComponent<TextMeshProUGUI>().text = message;

        var _color = newNotification.GetComponent<TextMeshProUGUI>().color;

        LeanTween
        .value(newNotification, _color.a, 1, 1f).setIgnoreTimeScale(true)
        .setOnUpdate((float _value) =>
        {
            _color.a = _value;
            newNotification.GetComponent<TextMeshProUGUI>().color = _color;
        })
        .setOnComplete(() => LeanTween.value(newNotification, _color.a, 1, 1)).setIgnoreTimeScale(true)
        .setOnComplete(() =>
        {
            LeanTween
            .value(newNotification, _color.a, 0, 1)
            .setOnUpdate((float _value) =>
            {
                _color.a = _value;
                newNotification.GetComponent<TextMeshProUGUI>().color = _color;
            });
        });
    }
}
