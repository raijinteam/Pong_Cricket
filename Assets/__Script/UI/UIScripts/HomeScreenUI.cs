using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class HomeScreenUI : MonoBehaviour
{
	[Header("Scripts")]

	public SlotChestInfoUI ui_SlotChestInfo;    // Ui SlotChest panel 
	[SerializeField] private DailyRewardUI ui_DailyReward;  // Daily Reward Panel; 
	[SerializeField] private WheelRouletteUI ui_WheelRoullete; // Wheel Rulet Panel 
    [SerializeField] private ChestSlotsUI[] all_ChestSlots;   // all Slot Panel 
	[SerializeField] private DailyTaskUI ui_DailyTask;   // Daily Task panel 

	[Header("Panels")]
	[SerializeField] private GameObject panel_DailyRewardNotification;
	[SerializeField] private GameObject panel_WheelRouletteNotification;
	[SerializeField] private GameObject panel_Menu;
	[SerializeField] private GameObject panel_LevelSelection;

	private void OnEnable()
	{
		SetTheChestSlotsAccordingToCurrentStates();
		panel_Menu.SetActive(true);
		panel_LevelSelection.SetActive(false);
		DailyTaskManager.Instance.ShowTaskBar();
	}

	private void Start()
	{

		// Daily reaward Actvte So Notification On
        panel_DailyRewardNotification.SetActive(RewardsManager.Instance.dailyRewardData.GetIsDailyRewardActive());


		// Wheel Rullet Actvete So Notification On
		panel_WheelRouletteNotification.
							SetActive(RewardsManager.Instance.wheelRouletteRewardData.IsWheelRouletteActive());


       
	}

	private void Update()
	{
		if (ChestManager.Instance.IsChestUnlockProcessRunning())
		{
			// One of the chest is being unlocked, share how much time is left 
			string formattedTime = UtilityManager.Instance.FormatTimeToString(ChestManager.Instance.GetTimeLeftForChestUnlock());
			all_ChestSlots[ChestManager.Instance.currentChestUnlockInProgressIndex].SetTimeLeftForUnlocking(formattedTime);
		}
	}


	public void CompletedChestUnlockingProcess(int _index)
	{
		all_ChestSlots[_index].SwitchToChestUnlocked();
		ui_SlotChestInfo.gameObject.SetActive(false);
	}

	public void FillTheEmptySlotWithChest(int _slotIndex)
	{
		all_ChestSlots[_slotIndex].FillThisChestSlot();
	}

	public void SetChestData(int _slotIndex)
	{
		all_ChestSlots[_slotIndex].SetCurrentChestData();
	}

	public void ShowChestSlotUnlocking()
	{
		all_ChestSlots[ChestManager.Instance.currentChestUnlockInProgressIndex].SwitchToChestRunning();
	}

	private void SetTheChestSlotsAccordingToCurrentStates()
	{
		for(int i = 0; i < all_ChestSlots.Length; i++)
		{
			all_ChestSlots[i].SetCurrentState();
		}
	}

	public void DailyRewardIsAvailableAgain()
	{
		panel_DailyRewardNotification.SetActive(true);
		ui_DailyReward.SetAllPanels();
	}

	public void WheelRouletteIsAvailableAgain()
	{
		panel_WheelRouletteNotification.SetActive(true);
		ui_WheelRoullete.SetWheelSpinAvailability();
	}

	public void DailyRewardClaimed()
	{
		panel_DailyRewardNotification.SetActive(false);
	}

	public void WheelRouletteClaimed()
	{
		panel_WheelRouletteNotification.SetActive(false);
	}

	public void SetDailyTaskPanel()
	{
		ui_DailyTask.SetTaskData();
	}

	// BUTTON FUNCTIONS BELOW

	public void OnClick_DailyReward()
	{
		AudioManager.insatance.PlayBtnClickSFX();
		ui_DailyReward.gameObject.SetActive(true);
	}

	public void OnClick_WheelRoulette()
	{
		AudioManager.insatance.PlayBtnClickSFX();
		ui_WheelRoullete.gameObject.SetActive(true);
	}


	public void OnClick_StartBtnClick() {

		AudioManager.insatance.PlayBtnClickSFX();
		UIManager.Instance.ui_LevelSelection.gameObject.SetActive(true);
    }

	public void OnClick_OnSettting() {
		
		AudioManager.insatance.PlayBtnClickSFX();
		UIManager.Instance.ui_SettingScrenn.gameObject.SetActive(true);
	}

	public void OnClick_OnDailyTask() {

		AudioManager.insatance.PlayBtnClickSFX();
		ui_DailyTask.gameObject.SetActive(true);
	}

    public void GetSkipTaskReward() {
		ui_DailyTask.GetSkipTaskReward();
    }
}
