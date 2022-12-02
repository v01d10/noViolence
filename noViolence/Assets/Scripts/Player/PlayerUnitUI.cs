using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUnitUI : MonoBehaviour
{
    public static PlayerUnitUI PUI;

    [Header("TMPROs")]
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI ExpText;

    public Image HealthBar;
    public Image ExpBar;

    public TextMeshProUGUI APText;
    public TextMeshProUGUI SkillText;
    public TextMeshProUGUI IntText;
    public TextMeshProUGUI StrText;

    [Header("Buttons")]
    public Button SkillButton;
    public Button IntButton;
    public Button StrButton;

    public Button SkillSpecButton;
    public Button IntSpecButton;
    public Button StrSpecButton;

    [Header("Objects")]
    public GameObject PlayerUnitPanel;
    public GameObject SpecOffPanel;
    public GameObject SpecOnPanel;
    public GameObject ChooseSpecPanel;
    public GameObject SkillPanel;
    public GameObject SmartPanel;
    public GameObject StrongPanel;

    void Awake()
    {
        PUI = this;
    }

    public void OpenPlayerUnitPanel(PlayerUnit selUnit)
    {
        uiManager.instance.OpenMainPanel();


        if (!PlayerUnitPanel.activeInHierarchy)
        {
            PlayerUnitPanel.SetActive(true);
            LoadUnitInfo(selUnit);
            LoadButtons(selUnit);
        }
        else
        {
            PlayerUnitPanel.SetActive(false);
        }

    }

    void LoadUnitInfo(PlayerUnit _selUnit)
    {
        NameText.text = _selUnit.Name;
        HealthText.text = _selUnit.Health.ToString() + "/" + _selUnit.MaxHealth.ToString();
        ExpText.text = _selUnit.exp.ToString() + "/" + _selUnit.expNeeded.ToString();

        _selUnit.HealthPerc = _selUnit.Health / _selUnit.MaxHealth;
        _selUnit.expPerc = _selUnit.exp / _selUnit.expNeeded;

        HealthBar.fillAmount = _selUnit.HealthPerc;
        ExpBar.fillAmount = _selUnit.expPerc;

        APText.text = _selUnit.atPoints.ToString();
        SkillText.text = _selUnit.skillLevel.ToString();
        IntText.text = _selUnit.intLevel.ToString();
        StrText.text = _selUnit.strLevel.ToString();

        if (_selUnit.skillLevel + _selUnit.intLevel + _selUnit.skillLevel < 5)
        {
            SpecOffPanel.SetActive(true);
        }
        else
        {
            if (SpecOffPanel.activeInHierarchy)
            {
                SpecOffPanel.SetActive(false);

                if (_selUnit.ChoosedSpec)
                {
                    ChooseSpecPanel.SetActive(false);
                }

                if (_selUnit.puType == PlayerUnit.pUnitType.Joyful)
                {
                    CloseSpecPanels();
                    SkillPanel.SetActive(true);
                }
                else if (_selUnit.puType == PlayerUnit.pUnitType.Smart)
                {
                    CloseSpecPanels();
                    SmartPanel.SetActive(true);
                }
                else if (_selUnit.puType == PlayerUnit.pUnitType.Strong)
                {
                    CloseSpecPanels();
                    StrongPanel.SetActive(true);
                }
            }
        }
    }

    void LoadButtons(PlayerUnit _selUnit)
    {
        SkillButton.onClick.RemoveAllListeners();
        IntButton.onClick.RemoveAllListeners();
        StrButton.onClick.RemoveAllListeners();

        SkillButton.onClick.AddListener(() => IncreaseSkillLevel(_selUnit));
        IntButton.onClick.AddListener(() => IncreaseIntLevel(_selUnit));
        StrButton.onClick.AddListener(() => IncreaseStrLevel(_selUnit));

        if (_selUnit.skillLevel + _selUnit.intLevel + _selUnit.skillLevel < 5 && !_selUnit.ChoosedSpec)
        {
            SkillSpecButton.onClick.RemoveAllListeners();
            IntSpecButton.onClick.RemoveAllListeners();
            StrSpecButton.onClick.RemoveAllListeners();

            SkillSpecButton.onClick.AddListener(() => ChooseSkillSpec(_selUnit));
            IntSpecButton.onClick.AddListener(() => ChooseSmartSpec(_selUnit));
            StrSpecButton.onClick.AddListener(() => ChooseStrongSpec(_selUnit));
        }
    }

    void IncreaseSkillLevel(PlayerUnit _selUnit)
    {
        if (_selUnit.atPoints >= 1)
        {
            _selUnit.atPoints--;
            _selUnit.skillLevel++;
        }
    }

    void IncreaseIntLevel(PlayerUnit _selUnit)
    {
        if (_selUnit.atPoints >= 1)
        {
            _selUnit.atPoints--;
            _selUnit.intLevel++;
        }
    }

    void IncreaseStrLevel(PlayerUnit _selUnit)
    {
        if (_selUnit.atPoints >= 1)
        {
            _selUnit.atPoints--;
            _selUnit.strLevel++;
        }
    }


    void ChooseSkillSpec(PlayerUnit _selUnit)
    {
        _selUnit.puType = PlayerUnit.pUnitType.Joyful;
        _selUnit.ChoosedSpec = true;
        ChooseSpecPanel.SetActive(false);
        CloseSpecPanels();
        SkillPanel.SetActive(true);
    }

    void ChooseSmartSpec(PlayerUnit _selUnit)
    {
        _selUnit.puType = PlayerUnit.pUnitType.Smart;
        _selUnit.ChoosedSpec = true;
        ChooseSpecPanel.SetActive(false);
        CloseSpecPanels();
        SmartPanel.SetActive(true);
    }

    void ChooseStrongSpec(PlayerUnit _selUnit)
    {
        _selUnit.puType = PlayerUnit.pUnitType.Strong;
        _selUnit.ChoosedSpec = true;
        ChooseSpecPanel.SetActive(false);
        CloseSpecPanels();
        StrongPanel.SetActive(true);
    }

    void CloseSpecPanels()
    {
        SkillPanel.SetActive(false);
        SmartPanel.SetActive(false);
        StrongPanel.SetActive(false);
    }
}

