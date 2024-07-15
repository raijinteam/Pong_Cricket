using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelDataUI : MonoBehaviour
{
	private int myLevelIndex;
	[SerializeField] private TextMeshProUGUI txt_LevelName;
	[SerializeField] private Image img_LevelIcon;
	[SerializeField] private TextMeshProUGUI txt_WinTrophyAmount;
	[SerializeField] private TextMeshProUGUI txt_LossTrophyAmount;
	[SerializeField] private TextMeshProUGUI txt_WinAmount;
	[SerializeField] private TextMeshProUGUI txt_EntryFee;
	[SerializeField] private Button btn_StartLevel;

	public void SetLevelData(int _levelIndex)
	{
		myLevelIndex = _levelIndex;
		txt_LevelName.text = LevelManager.Instance.GetLevelName(myLevelIndex);
		txt_WinTrophyAmount.text = "+" + LevelManager.Instance.GetWinTrophyAmount(myLevelIndex);
		txt_LossTrophyAmount.text = "-" + LevelManager.Instance.GetLoseTrophyAmount(myLevelIndex);
		
		txt_WinAmount.text = LevelManager.Instance.GetLevelWinAmount(myLevelIndex).ToString();
		txt_EntryFee.text = LevelManager.Instance.GetLevelEntryFee(myLevelIndex).ToString();
		img_LevelIcon.sprite = LevelManager.Instance.GetLevelIcon(_levelIndex);
  
	}

	public void OnClick_StartLevel() {

		if (DataManager.Instance.coins < LevelManager.Instance.GetLevelEntryFee(myLevelIndex)) {
			UIManager.Instance.spawnPopup("You Have no Entry Fee");
			return;
		}

        LevelManager.Instance.currentLevelIndex = myLevelIndex;
        DataManager.Instance.DecresedCoin(LevelManager.Instance.GetLevelEntryFee(myLevelIndex));
        UIManager.Instance.ui_PanelSerachingPlayer.gameObject.SetActive(true);

    }
}
