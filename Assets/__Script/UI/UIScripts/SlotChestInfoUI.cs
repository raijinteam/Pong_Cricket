using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotChestInfoUI : MonoBehaviour
{

	[SerializeField] public int UnlockedGemsValue = 50;
	[SerializeField] private GameObject panel_OptionToUnlockThisChest;
	[SerializeField] private GameObject panel_ChestUnlockingInProgress;
	[SerializeField] private TextMeshProUGUI txt_TimeLeftForUnlock;
	[SerializeField] private GameObject btn_Skip;
	//[SerializeField] private TextMeshProUGUI txt_ChestLocation;
	[SerializeField] private Image img_ChestIcon;
	[SerializeField] private TextMeshProUGUI txt_CoinRange;
	[SerializeField] private TextMeshProUGUI txt_GemRange;
	[SerializeField] private TextMeshProUGUI txt_UnlockedGemsValueInProgress;
	[SerializeField] private TextMeshProUGUI txt_UnlockedGems;
	
	[SerializeField] private TextMeshProUGUI txt_CommanCardRange;
	[SerializeField] private TextMeshProUGUI txt_RareCardRange;
	[SerializeField] private TextMeshProUGUI txt_EpicCardRange;
	[SerializeField] private TextMeshProUGUI txt_UnlockTime;

	[SerializeField]private int currentOpenedChestIndex;


	[Header("animation")]
	[SerializeField] private RectTransform rect_Main;
	[SerializeField] private float flt_AnimationTime;
	[SerializeField] private float flt_ClosedAnimationTime;


    private void OnEnable() {

		Panel_Animation.instance.Enable_PopUp(rect_Main, flt_AnimationTime);
    }

    private void Update()
	{
		if (ChestManager.Instance.IsChestUnlockProcessRunning())
		{
			if (panel_ChestUnlockingInProgress.activeSelf)
			{
				// Has opened the panel for chest which is being unlocked. Update the timer
				string formattedTime = UtilityManager.Instance.FormatTimeToString(ChestManager.Instance.GetTimeLeftForChestUnlock());
				txt_TimeLeftForUnlock.text = formattedTime; // Show time left
			}
		}

		
	}

	public void SetChestInfo(int _chestSlotIndex, bool isChestSlotUnlocking)
	{
		currentOpenedChestIndex = _chestSlotIndex;
        btn_Skip.SetActive(AdsManager.instance.IsRewardAdLoad);
        if (isChestSlotUnlocking)
		{
			panel_ChestUnlockingInProgress.SetActive(true);
			
			panel_OptionToUnlockThisChest.SetActive(false);
		}
		else
		{
			panel_ChestUnlockingInProgress.SetActive(false);
			panel_OptionToUnlockThisChest.SetActive(true);

			SetChestOpenInfo();
		}

		gameObject.SetActive(true);
	}

	private void SetChestOpenInfo()
	{
		//txt_ChestLocation.text = ChestManager.Instance.GetChestLocationName(currentOpenedChestIndex);
		img_ChestIcon.sprite = ChestManager.Instance.GetIconOfChest(currentOpenedChestIndex);

		int[] coinRange = new int[2];
		coinRange = ChestManager.Instance.GetCoinRewardRange(currentOpenedChestIndex);
		txt_CoinRange.text = coinRange[0] + "-" + coinRange[1];

		int[] gemRange = new int[2];
		gemRange = ChestManager.Instance.GetGemsRewardRange(currentOpenedChestIndex);
		txt_GemRange.text = gemRange[0] + "-" + gemRange[1];

		//txt_TotalCard.text = ChestManager.Instance.GetTotalDifferentCardsUserCanGet(currentOpenedChestIndex).ToString();
        int[] commanCard = new int[2];
        commanCard = ChestManager.Instance.GetCommanRewardRange(currentOpenedChestIndex);
        txt_CommanCardRange.text = commanCard[0] + "-" + commanCard[1];


        int[] Rare = new int[2];
        Rare = ChestManager.Instance.GetRareRewardRange(currentOpenedChestIndex);
        txt_CommanCardRange.text = Rare[0] + "-" + Rare[1];


        int[] epic = new int[2];
        epic = ChestManager.Instance.GetEpicRewardRange(currentOpenedChestIndex);
        txt_CommanCardRange.text = epic[0] + "-" + epic[1];



        txt_UnlockTime.text = ChestManager.Instance.GetUnlockTimeInString(currentOpenedChestIndex);
		txt_UnlockedGemsValueInProgress.text = UnlockedGemsValue.ToString();
		txt_UnlockedGems.text = UnlockedGemsValue.ToString();

    }


	public void OnClick_StartUnlock()
	{
		// Check If any other chest is in unlock process or not
		if (ChestManager.Instance.IsChestUnlockProcessRunning())
		{
			Debug.Log("Another chest is being unlocked, try again after that is unlocked");
			UIManager.Instance.spawnPopup("Another chest is being unlocked, try again after that is unlocked");
			return; // Found one chest which is being unlocked. Cannot unlock this right now, SHOW WARNING MESSAGE
		}

		AudioManager.insatance.PlayBtnClickSFX();
		panel_ChestUnlockingInProgress.SetActive(true);
		panel_OptionToUnlockThisChest.SetActive(false);
		
		ChestManager.Instance.StartChestUnlockProcess(currentOpenedChestIndex);		
	}


	public void OnClick_OnPayCurrencyAndOpenedChest() {

		if (DataManager.Instance.Gems <= UnlockedGemsValue) {
			UIManager.Instance.spawnPopup("No Enough Gems To Buy Chest");
			return;
		}

		// Remove Currency

		AudioManager.insatance.PlayBtnClickSFX();
		DataManager.Instance.DecresedGems(UnlockedGemsValue);
        UIManager.Instance.ui_ChestOpping.ActivetedLevelChestOpnened(ChestManager.Instance.GetSlot(currentOpenedChestIndex));
        ChestManager.Instance.UserCollectedTheChestSlot(currentOpenedChestIndex);
		UIManager.Instance.ui_HomeScreen.SetChestData(currentOpenedChestIndex);
		this.gameObject.SetActive(false);

    }


	public void OnClick_WatchAdToSkipTime()
	{
		AudioManager.insatance.PlayBtnClickSFX();
		if (DataManager.Instance.skipIts <=  0) {
            AdsManager.instance.ShowRewardAds(AdsRewardType.SkipTime);
		}
		else {
			DataManager.Instance.RemoveSkipIts();
		}
		
		
	}

	public void OnClick_Back()
	{
		AudioManager.insatance.PlayBtnClickSFX();
        Panel_Animation.instance.Disable_PopUp(rect_Main, flt_AnimationTime , this.gameObject);
    }

   
}
