using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelDataUI : MonoBehaviour
{
	private int myLevelIndex;
	[SerializeField] private GameObject panel_Locked;
	[SerializeField] private GameObject panel_Unlocked;
	[SerializeField] private TextMeshProUGUI txt_LevelName;
	[SerializeField] private Image img_LevelIcon;
	[SerializeField] private TextMeshProUGUI txt_WinTrophyAmount;
	[SerializeField] private TextMeshProUGUI txt_LossTrophyAmount;
	[SerializeField] private TextMeshProUGUI txt_TrophyRequiredToUnlock;
	[SerializeField] private TextMeshProUGUI txt_WinAmount;
	[SerializeField] private TextMeshProUGUI txt_EntryFee;
	[SerializeField] private Button btn_StartLevel;

	public void SetLevelData(int _levelIndex)
	{
		myLevelIndex = _levelIndex;
		txt_LevelName.text = LevelManager.Instance.GetLevelName(myLevelIndex);
		txt_WinTrophyAmount.text = "+" + LevelManager.Instance.GetWinTrophyAmount(myLevelIndex);
		txt_LossTrophyAmount.text = "-" + LevelManager.Instance.GetLoseTrophyAmount(myLevelIndex);
		txt_TrophyRequiredToUnlock.text = LevelManager.Instance.GetTrophyRequiredToPlayLevel(myLevelIndex).ToString();
		txt_WinAmount.text = LevelManager.Instance.GetLevelWinAmount(myLevelIndex).ToString();
		txt_EntryFee.text = LevelManager.Instance.GetLevelEntryFee(myLevelIndex).ToString();

		if(DataManager.Instance.trophy >= LevelManager.Instance.GetTrophyRequiredToPlayLevel(myLevelIndex))
		{
			// user can play Level
			panel_Locked.SetActive(false);
			panel_Unlocked.SetActive(true);

			btn_StartLevel.interactable = true;
		}
		else
		{
			// does not have enough trophy

			panel_Locked.SetActive(true);
			panel_Unlocked.SetActive(false);

			btn_StartLevel.interactable = false;
		}
	}

	public void OnClick_StartLevel() {

		if (DataManager.Instance.coins >= LevelManager.Instance.GetLevelEntryFee(myLevelIndex)) {

			LevelManager.Instance.currentLevelIndex = myLevelIndex;
			DataManager.Instance.DecresedCoin(LevelManager.Instance.GetLevelEntryFee(myLevelIndex));
			UIManager.Instance.ui_PanelSerachingPlayer.gameObject.SetActive(true);
			
			

		}



	}
}
