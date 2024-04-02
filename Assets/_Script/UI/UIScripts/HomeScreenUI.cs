using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeScreenUI : MonoBehaviour
{
	[Header("Scripts")]
	public SlotChestInfoUI ui_SlotChestInfo;
	[SerializeField] private DailyRewardUI ui_DailyReward;
	[SerializeField] private WheelRouletteUI ui_WheelRoullete;
    [SerializeField] private ChestSlotsUI[] all_ChestSlots;
	[SerializeField] private DailyTaskUI ui_DailyTask;

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
		if (RewardsManager.Instance.dailyRewardData.GetIsDailyRewardActive())
		{
			panel_DailyRewardNotification.SetActive(true);
		}

		if (RewardsManager.Instance.wheelRouletteRewardData.IsWheelRouletteActive())
		{
			panel_WheelRouletteNotification.SetActive(true);
		}
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
		ui_DailyReward.gameObject.SetActive(true);
	}

	public void OnClick_WheelRoulette()
	{
		ui_WheelRoullete.gameObject.SetActive(true);
	}


	public void OnClick_StartBtnClick() {

		UIManager.Instance.ui_LevelSelection.gameObject.SetActive(true);
    }

	public void OnClick_OnSettting() {
		UIManager.Instance.ui_SettingScrenn.gameObject.SetActive(true);
	}

	public void OnClick_OnDailyTask() {
		ui_DailyTask.gameObject.SetActive(true);
	}




}
