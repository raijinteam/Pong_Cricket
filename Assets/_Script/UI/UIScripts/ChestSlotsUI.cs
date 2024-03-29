using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestSlotsUI : MonoBehaviour
{
	[SerializeField] private int slotIndex;
	[SerializeField] private GameObject panel_ChestSlotEmpty;
	[SerializeField] private GameObject panel_ChestSlotFilled;
	[SerializeField] private GameObject panel_ChestSlotRunning;
	[SerializeField] private GameObject panel_ChestSlotCompleted;
	[SerializeField] private TextMeshProUGUI txt_TimeLeftForUnlocking; // time left for the chest which is unlocking
	[SerializeField] private Image[] all_img_ChestIcons;
	[SerializeField] private TextMeshProUGUI[] all_txt_ChestLevelNames;
	[SerializeField] private TextMeshProUGUI txt_TimeRequiredToUnlockTheChest; // static time to show how much time it would take for the chest to unlock

	public void SetCurrentState()
	{
		panel_ChestSlotEmpty.SetActive(false);
		panel_ChestSlotFilled.SetActive(false);
		panel_ChestSlotRunning.SetActive(false);
		panel_ChestSlotCompleted.SetActive(false);
	
		if (ChestManager.Instance.GetCurrentSlotState(slotIndex) == SlotState.Empty)
		{
			panel_ChestSlotEmpty.SetActive(true);
		}
		else if (ChestManager.Instance.GetCurrentSlotState(slotIndex) == SlotState.Filled)
		{
			panel_ChestSlotFilled.SetActive(true);
		}
		else if (ChestManager.Instance.GetCurrentSlotState(slotIndex) == SlotState.ChestUnlockInProgress)
		{
			panel_ChestSlotRunning.SetActive(true);
		}
		else
		{
			panel_ChestSlotCompleted.SetActive(true);
		}
	}

	

    public void FillThisChestSlot()
	{
		panel_ChestSlotEmpty.SetActive(false);
		panel_ChestSlotFilled.SetActive(true);
		SetCurrentChestData();
		
	}

	public void SetCurrentChestData()
	{
		for (int i = 0; i < all_img_ChestIcons.Length; i++)
		{
			all_img_ChestIcons[i].sprite = ChestManager.Instance.GetIconOfChest(slotIndex);
		}

		for (int i = 0; i < all_txt_ChestLevelNames.Length; i++)
		{
			all_txt_ChestLevelNames[i].text = ChestManager.Instance.GetChestLocationName(slotIndex);
		}

		TimeSpan timeSpan = TimeSpan.FromSeconds(ChestManager.Instance.GetTotalUnlockTimeInFloat(slotIndex));
		txt_TimeRequiredToUnlockTheChest.text = UtilityManager.Instance.FormatTimeToSingularValue(timeSpan);
	}

	public void SwitchToChestRunning()
	{
		panel_ChestSlotEmpty.SetActive(false);
		panel_ChestSlotFilled.SetActive(false);
		panel_ChestSlotRunning.SetActive(true);
	}

	public void SwitchToChestUnlocked()
	{
		panel_ChestSlotEmpty.SetActive(false);
		panel_ChestSlotRunning.SetActive(false);
		panel_ChestSlotCompleted.SetActive(true);
	}

	private void EmptyThisSlot()
	{
		panel_ChestSlotCompleted.SetActive(false);
		panel_ChestSlotEmpty.SetActive(true);
	}

	public void SetTimeLeftForUnlocking(string _timeLeft)
	{
		txt_TimeLeftForUnlocking.text = _timeLeft;
	}

	public void OnClick_ChestSlot()
	{
		if (panel_ChestSlotEmpty.activeSelf)
		{
			// No Chest Found
			Debug.Log("Chest Slot Empty, Share A message to user");

		}
		else if (panel_ChestSlotFilled.activeSelf)
		{
			Debug.Log("Open Chest Info Panel With Start Unlock Option");
			UIManager.Instance.ui_HomeScreen.ui_SlotChestInfo.SetChestInfo(slotIndex, false);
		}
		else if (panel_ChestSlotRunning.activeSelf)
		{
			Debug.Log("Open Info panel showing chest unlock in progress");
			UIManager.Instance.ui_HomeScreen.ui_SlotChestInfo.SetChestInfo(slotIndex, true);
		}
		else if (panel_ChestSlotCompleted.activeSelf)
		{
            // Give Reward, and empty this slot
            UIManager.Instance.ui_ChestOpping.ActivetedLevelChestOpnened(ChestManager.Instance.GetSlot(slotIndex));
            EmptyThisSlot();
			ChestManager.Instance.UserCollectedTheChestSlot(slotIndex);
		}
	}
}
