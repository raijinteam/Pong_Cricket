using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityInfoUI : MonoBehaviour
{
    private int myAbilityIndex;
    [SerializeField] private GameObject panel_Locked;
    [SerializeField] private Image img_Icon;
    [SerializeField] private TextMeshProUGUI txt_AbilityName;
    [SerializeField] private TextMeshProUGUI txt_AbilityLevel;
    [SerializeField] private GameObject panel_Level;
    [SerializeField] private TextMeshProUGUI txt_Description;
    [SerializeField] private GameObject panel_Description;

    public void SetMyPanel(int _myIndex, bool isUnlocked, Sprite _icon, string _name, int _level, string _description)
	{
        myAbilityIndex = _myIndex;

		if (isUnlocked)
		{
            panel_Locked.SetActive(false);
            panel_Level.SetActive(true);
		}
		else
		{
            panel_Locked.SetActive(true);
            panel_Level.SetActive(false);
		}

        img_Icon.sprite = _icon;
        txt_AbilityName.text = _name;

        int levelToDisplay = _level + 1;
        txt_AbilityLevel.text = levelToDisplay.ToString();

        txt_Description.text = _description;
    }

    public void UpdateMyPanel()
	{
        panel_Locked.SetActive(false);
        panel_Level.SetActive(true);

        int levelToDisplay = AbilityManager.Instance.GetAbilityCurrentLevel(myAbilityIndex) + 1;
        txt_AbilityLevel.text = levelToDisplay.ToString();

    }

    public void TurnOffDescriptionPanel()
	{
        panel_Description.SetActive(false);
	}

   

    public void OnClick_Ability()
	{
        if (!panel_Description.activeSelf)
		{
            UIManager.Instance.ui_Abilities.DisableAllInfoPanel();
            panel_Description.SetActive(true);
        }
        else
		{
            panel_Description.SetActive(false);
        }      
	}

   
}
