using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectionButtonUI : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private int myIndex;
    [SerializeField] private Sprite[] all_FillImageSprites;

    [Header("UI")]
    [SerializeField] private GameObject panel_Locked;
    [SerializeField] private GameObject panel_Selected;
    [SerializeField] private Slider slider_CardsProgress;
    [SerializeField] private Image img_FillImageInCardsSlider;
    [SerializeField] private Image img_PlayerIcon;
    [SerializeField] private TextMeshProUGUI txt_PlayerLevel;
    [SerializeField] private TextMeshProUGUI txt_PlayerCardsValue;
    [SerializeField] private TextMeshProUGUI txt_PlayerName;
    [SerializeField] private GameObject panel_UpgradeAvailableNotification;

    public void SetMyData(int _myIndex, bool _IsUnlocked, int _CurrentCards, int _MaxCards, Sprite _CharacterIcon, int _CharacterLevel, string _CharacterName)
	{
        panel_Selected.SetActive(false);
  
        myIndex = _myIndex;
        if (_IsUnlocked)
		{
            panel_Locked.SetActive(false);
		}

		if (CharacterManager.Instance.IsCurrentCharacterAtMaxLevel(myIndex))
		{
            // Character at max level, disable slider
            slider_CardsProgress.gameObject.SetActive(false);
            panel_UpgradeAvailableNotification.SetActive(false);
        }
		else
		{
            slider_CardsProgress.gameObject.SetActive(true);
            slider_CardsProgress.maxValue = _MaxCards;
            slider_CardsProgress.value = _CurrentCards;


            if (_CurrentCards >= _MaxCards)
            {
                // Upgrade Available
                panel_UpgradeAvailableNotification.SetActive(true); // Show notification of upgrade available
                img_FillImageInCardsSlider.sprite = all_FillImageSprites[1]; // Change slider color to green
            }
            else
            {
                // Not enough cards for upgrade
                panel_UpgradeAvailableNotification.SetActive(false);
                img_FillImageInCardsSlider.sprite = all_FillImageSprites[0]; // Normal Blue Slider
            }
        }
       

        img_PlayerIcon.sprite = _CharacterIcon;
        int currentLevelForDisplay = _CharacterLevel + 1;
        txt_PlayerLevel.text = currentLevelForDisplay.ToString();
        txt_PlayerCardsValue.text = _CurrentCards + " / " + _MaxCards;
        txt_PlayerName.text = _CharacterName;

	}

    public void UpdateMyData(int _CurrentCards, int _MaxCards, int _CharacterLevel)
	{

        if (CharacterManager.Instance.IsCurrentCharacterAtMaxLevel(myIndex))
        {
            // Character at max level, disable slider
            slider_CardsProgress.gameObject.SetActive(false);
            panel_UpgradeAvailableNotification.SetActive(false);
        }
        else
        {
            slider_CardsProgress.gameObject.SetActive(true);
            slider_CardsProgress.maxValue = _MaxCards;
            slider_CardsProgress.value = _CurrentCards;

            if (_CurrentCards >= _MaxCards)
            {
                // Upgrade Available
                panel_UpgradeAvailableNotification.SetActive(true); // Show notification of upgrade available
                img_FillImageInCardsSlider.sprite = all_FillImageSprites[1]; // Change slider color to green
            }
            else
            {
                // Not enough cards for upgrade
                panel_UpgradeAvailableNotification.SetActive(false);
                img_FillImageInCardsSlider.sprite = all_FillImageSprites[0]; // Normal Blue Slider
            }
        }
    
        int currentLevelForDisplay = _CharacterLevel + 1;
        txt_PlayerLevel.text = currentLevelForDisplay.ToString();
        txt_PlayerCardsValue.text = _CurrentCards + " / " + _MaxCards;    
    }

    public void SelectThisCharacter()
	{
        panel_Selected.SetActive(true);
	}

    public void DeSelectThisCharacter()
	{
        panel_Selected.SetActive(false);
	}

    public void OnClick_Player()
	{
        UIManager.Instance.ui_PlayerSelection.ClickedOnACharacter(myIndex);
	}
}
