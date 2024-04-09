using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopChestInfoUI : MonoBehaviour
{
	//[SerializeField] private TextMeshProUGUI txt_ChestName;
	[SerializeField] private Image img_ChestIcon;
	[SerializeField] private TextMeshProUGUI txt_GoldRewardRange;
	[SerializeField] private TextMeshProUGUI txt_GemRewardRange;
	[SerializeField] private TextMeshProUGUI txt_CommonCardsRewardRange;
	[SerializeField] private TextMeshProUGUI txt_RareCardsRewardRange;
	[SerializeField] private TextMeshProUGUI txt_EpicCardsRewardRange;
	[SerializeField] private GameObject panel_EpicCards;
	[SerializeField] private TextMeshProUGUI txt_UnlockPrice;
	private ChestShopScriptableObject myChest;

    public void SetChestPanel(ChestShopScriptableObject _chestInfo)
	{
		myChest = _chestInfo;
		gameObject.SetActive(true);
		//txt_ChestName.text = _chestInfo.str_ChestName;
		img_ChestIcon.sprite = _chestInfo.sprite_ChestIcon;

		txt_GoldRewardRange.text = _chestInfo.coinRewardRange[0] + " - " + _chestInfo.coinRewardRange[1];
		txt_GemRewardRange.text = _chestInfo.gemRewardRange[0] + " - " + _chestInfo.gemRewardRange[1];

		int totalDifferentCardsCount = _chestInfo.numberOfCommonCharactersToReward; // how many different common character card user will receive
		int minimumCardsCount = _chestInfo.commonCardRewardRange[0] * totalDifferentCardsCount;
		int maximumCardsCount = _chestInfo.commonCardRewardRange[1] * totalDifferentCardsCount;
		txt_CommonCardsRewardRange.text = minimumCardsCount + " - " + maximumCardsCount;

		totalDifferentCardsCount = _chestInfo.numberOfRareCharactersToReward; // how many different rare character cards user will receive
		minimumCardsCount = _chestInfo.rareCardRewardRange[0] * totalDifferentCardsCount;
		maximumCardsCount = _chestInfo.rareCardRewardRange[1] * totalDifferentCardsCount;
		txt_RareCardsRewardRange.text = minimumCardsCount + " - " + maximumCardsCount;

		totalDifferentCardsCount = _chestInfo.numberOfEpicCharactersToReward; // how many different epic character cards user will receive
		if(totalDifferentCardsCount == 0)
		{
			// In this chest no epic cards will be found. Disable the panel
			panel_EpicCards.SetActive(false);
		}
		else
		{
			panel_EpicCards.SetActive(true);
			minimumCardsCount = _chestInfo.epicCardRewardRange[0] * totalDifferentCardsCount;
			maximumCardsCount = _chestInfo.epicCardRewardRange[1] * totalDifferentCardsCount;
			txt_EpicCardsRewardRange.text = minimumCardsCount + " - " + maximumCardsCount;
		}

		txt_UnlockPrice.text = _chestInfo.costToOpenTheChest.ToString();
	}

	public void OnClick_Close()
	{
		gameObject.SetActive(false);
	}
	public void OnClick_OnBuyChest() {
		if (DataManager.Instance.Gems < myChest.costToOpenTheChest) {
			Debug.Log("You have No gems To Buy This Chest");
		}

		this.gameObject.SetActive(false);
		UIManager.Instance.ui_ChestOpping.ActavetedShopBaseChest(myChest);
		

	}
}
