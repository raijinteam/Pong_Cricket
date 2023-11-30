using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChestManager : MonoBehaviour
{
	public static ChestManager Instance;

	//// TEMP //
	//public string chestType;
	//public string chestLocation;
	//public int[] array_RangeOfCoins;
	//public int[] array_RangeOfGems;
	//public int[] array_RangeOfCardsCommon;
	//public int[] array_RangeOfCardsRare;
	//public int[] array_RangeOfCardsEpic;
	//public float maxTimtToUnlockAChest = 30f;
	//// TEMP //

	private void Awake()
	{
		Instance = this;
	}

	[Header("Chest Slot")]
	[SerializeField] private ChestLevelsScriptableObject[] all_ChestTypes;
	[SerializeField] private string[] all_str_ChestNames;

	[Header("Chest Slots Handling")]
	//[SerializeField] private ChestInformationScriptableObject basicChest;
	[SerializeField] private ChestSlot[] all_ChestSlots; // Reference to all slots current state
	private int maxChestLevel = 5; 	
	public int currentChestUnlockInProgressIndex = 0; // index of slot where chest is being unlocked at the moment
	[SerializeField] private float flt_TimeUserCanSkipThroughWatchingAdForUnlockingChest; // Time value in seconds, if user watches an ad for fastening the unlock process. We have to reduce the time taken by this amount
	private int currentEmptySlotIndex = 0; // Used to assign a chest in an empty spot
	private bool isChestUnlockInProgress = false; // To check if there is any unlock in process
	private DateTime dt_ChestUnlockTime;


	private void Start()
	{
		if(PlayerPrefs.HasKey(ChestHandlerPlayerPrefKeys.KEY_CHESTSTATUS + "" + 0))
		{
			// Get Data
			FetchData();
		}
		else
		{
			// Save Data
			SaveFirstTimeData();
		}
	}

	private void SaveFirstTimeData()
	{
		for(int i = 0; i < all_ChestSlots.Length; i++)
		{
			PlayerPrefs.SetInt(ChestHandlerPlayerPrefKeys.KEY_CHESTSTATUS + "" + i, 0); // SET DEFAULT TO EMPTY
		}
	}

	private void FetchData()
	{
		string storedTime = PlayerPrefs.GetString(ChestHandlerPlayerPrefKeys.KEY_CHESTUNLOCKTIME, "0");
		dt_ChestUnlockTime = DateTime.FromBinary(Convert.ToInt64(storedTime));

		// FIRST FETCH CURRENT STATES OF ALL THE CHEST SLOTS
		for (int i = 0; i < all_ChestSlots.Length; i++)
		{
			int chestStatus = PlayerPrefs.GetInt(ChestHandlerPlayerPrefKeys.KEY_CHESTSTATUS + "" + i, 0);
			Debug.Log("Chest status: " + chestStatus + " INDEX : " + i);

			if(chestStatus > 0)
			{			
				int chestTypeIndex = PlayerPrefs.GetInt(ChestHandlerPlayerPrefKeys.KEY_CHESTTYPE + "" + i, 0); // get chest type index 
				int chestLevel = PlayerPrefs.GetInt(ChestHandlerPlayerPrefKeys.KEY_CHESTLEVEL + "" + i, 0); // get chest level

				if(chestStatus == 1) // Chest is in state filled in this index
				{
					all_ChestSlots[i].GotChestSlotDataFromDatabase(SlotState.Filled, chestLevel, all_ChestTypes[chestTypeIndex]);
					UIManager.Instance.ui_HomeScreen.FillTheEmptySlotWithChest(i);
				}
				else if(chestStatus == 2)
				{
					currentChestUnlockInProgressIndex = i;
					all_ChestSlots[i].GotChestSlotDataFromDatabase(SlotState.ChestUnlockInProgress, chestLevel, all_ChestTypes[chestTypeIndex]);
					isChestUnlockInProgress = true;
					all_ChestSlots[currentChestUnlockInProgressIndex].slotState = SlotState.ChestUnlockInProgress;
					UIManager.Instance.ui_HomeScreen.ShowChestSlotUnlocking();
				}
				else
				{
					all_ChestSlots[i].GotChestSlotDataFromDatabase(SlotState.ChestUnlockCompleted, chestLevel, all_ChestTypes[chestTypeIndex]);
					UIManager.Instance.ui_HomeScreen.CompletedChestUnlockingProcess(i);
				}

				UIManager.Instance.ui_HomeScreen.SetChestData(i);
			}
		}
	}

	public void RewardChestIfPossible() {

        if (!IsRewardingWithChestPossible()) {

			Debug.Log("NO EMPTY SLOT");
			return;
        }

		FillTheEmptySlotWithAChest();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.C))
		{
			if (IsRewardingWithChestPossible())
			{
				Debug.Log("Found 1 empty Slot at index : " + currentEmptySlotIndex);
				FillTheEmptySlotWithAChest();
			}
		}

		if (isChestUnlockInProgress)
		{
			if (GetTimeLeftForChestUnlock() <= TimeSpan.Zero)
			{
				ChestUnlockProcessCompleted();
			}
		}
	}

	private void FillTheEmptySlotWithAChest()
	{
		//all_ChestSlots[currentEmptySlotIndex].slotState = SlotState.Filled;

		// RANDOMIZE CHEST FROM ALL LEVELS. TEMPORARY
		int chestLevel = LevelManager.Instance.currentLevelIndex;
		all_ChestSlots[currentEmptySlotIndex].FillChestSlot(chestLevel, all_ChestTypes[0]);

		UIManager.Instance.ui_HomeScreen.FillTheEmptySlotWithChest(currentEmptySlotIndex);
		UpdateChestSlotFilled(currentEmptySlotIndex);
	}

	public void StartChestUnlockProcess(int _slotIndex)
	{
		currentChestUnlockInProgressIndex = _slotIndex;
		
		int timeInHours = all_ChestSlots[currentChestUnlockInProgressIndex].chestInThisSlot.rewardHours;
		int timeInMinutes = all_ChestSlots[currentChestUnlockInProgressIndex].chestInThisSlot.rewardMinutes;
		int timeInSeconds = all_ChestSlots[currentChestUnlockInProgressIndex].chestInThisSlot.rewardSeconds;
		dt_ChestUnlockTime = DateTime.Now.Add(new TimeSpan(timeInHours, timeInMinutes, timeInSeconds));
		
		isChestUnlockInProgress = true;
		all_ChestSlots[currentChestUnlockInProgressIndex].slotState = SlotState.ChestUnlockInProgress;

		UIManager.Instance.ui_HomeScreen.ShowChestSlotUnlocking();

		UpdateChestSlotStartedUnlocking();

	}

	public void ChestUnlockProcessCompleted()
	{
		isChestUnlockInProgress = false; // unlocked the chest
		all_ChestSlots[currentChestUnlockInProgressIndex].slotState = SlotState.ChestUnlockCompleted;
		UIManager.Instance.ui_HomeScreen.CompletedChestUnlockingProcess(currentChestUnlockInProgressIndex);

		UpdateChestSlotUnlocked();
	}

	public void UserCollectedTheChestSlot(int _slotIndex)
	{
		all_ChestSlots[_slotIndex].slotState = SlotState.Empty;
		UpdateChestSlotCollected(_slotIndex);
	}

	public void UserCompletedWatchAdToSkipTime()
	{
		int SkipHours = 3;
		dt_ChestUnlockTime = dt_ChestUnlockTime.Subtract(new TimeSpan(SkipHours, 0, 0));
	}

	public TimeSpan GetTimeLeftForChestUnlock()
	{
		return dt_ChestUnlockTime - DateTime.Now;
	}

	public bool IsRewardingWithChestPossible()
	{
		for(int i = 0; i < all_ChestSlots.Length; i++)
		{
			if(all_ChestSlots[i].slotState == SlotState.Empty)
			{
				currentEmptySlotIndex = i;
				return true; // Yes, Found One slot empty
			}
		}

		return false; // Not Possible, all the slots are filled
	}

	public bool IsChestUnlockProcessRunning()
	{
		return isChestUnlockInProgress;
	}

	public SlotState GetCurrentSlotState(int _slotIndex)
	{
		return all_ChestSlots[_slotIndex].slotState;
	}

	public Sprite GetIconOfChest(int _slotIndex)
	{
		return all_ChestSlots[_slotIndex].chestInThisSlot.str_ChestIcon;
	}

	public string GetChestLocationName(int _slotIndex)
	{
		return all_str_ChestNames[all_ChestSlots[_slotIndex].chestLevelIndex];
	}

	public int[] GetCoinRewardRange(int _slotIndex)
	{
		int[] coinRange = new int[2];
		coinRange[0] = all_ChestSlots[_slotIndex].chestInThisSlot.all_MinimumCoinRewardBasedOnLevel[all_ChestSlots[_slotIndex].chestLevelIndex];
		coinRange[1] = all_ChestSlots[_slotIndex].chestInThisSlot.all_MaximumCoinRewardBasedOnLevel[all_ChestSlots[_slotIndex].chestLevelIndex];

		return coinRange;
	}

	public int[] GetGemsRewardRange(int _slotIndex)
	{
		int[] gemsRange = new int[2];

		gemsRange[0] = all_ChestSlots[_slotIndex].chestInThisSlot.all_MinimumGemRewardBasedOnLevel[all_ChestSlots[_slotIndex].chestLevelIndex];
		gemsRange[1] = all_ChestSlots[_slotIndex].chestInThisSlot.all_MaximumGemRewardBasedOnLevel[all_ChestSlots[_slotIndex].chestLevelIndex];

		return gemsRange;
	}

	public int GetTotalDifferentCardsUserCanGet(int _slotIndex)
	{
		int cardCount = 0;
		cardCount += all_ChestSlots[_slotIndex].chestInThisSlot.maxNumberOfCommonCharactersToReward;
		cardCount += all_ChestSlots[_slotIndex].chestInThisSlot.maxNumberOfRareCharactersToReward;
		cardCount += all_ChestSlots[_slotIndex].chestInThisSlot.maxNumberOfEpicCharactersToReward;

		return cardCount;
	}

	public string GetUnlockTimeInString(int _slotIndex)
	{
		int timeInHours = all_ChestSlots[_slotIndex].chestInThisSlot.rewardHours;
		int timeInMinutes = all_ChestSlots[_slotIndex].chestInThisSlot.rewardMinutes;
		int timeInSeconds = all_ChestSlots[_slotIndex].chestInThisSlot.rewardSeconds;

		float totalTime = (timeInHours * 60 * 60) + (timeInMinutes * 60) + timeInSeconds;

		TimeSpan timeSpan = TimeSpan.FromSeconds(totalTime);
		return UtilityManager.Instance.FormatTimeToString(timeSpan);
	}

	public float GetTotalUnlockTimeInFloat(int _slotIndex)
	{
		float timeInHours = all_ChestSlots[_slotIndex].chestInThisSlot.rewardHours;
		float timeInMinutes = all_ChestSlots[_slotIndex].chestInThisSlot.rewardMinutes;
		float timeInSeconds = all_ChestSlots[_slotIndex].chestInThisSlot.rewardSeconds;

		float totalTime = (timeInHours * 60 * 60) + (timeInMinutes * 60) + timeInSeconds;

		return totalTime;
	}

	#region Data Save and Get

	private void UpdateChestSlotFilled(int _chestSlot)
	{
		PlayerPrefs.SetInt(ChestHandlerPlayerPrefKeys.KEY_CHESTSTATUS + "" + _chestSlot, 1);
		PlayerPrefs.SetInt(ChestHandlerPlayerPrefKeys.KEY_CHESTTYPE + "" + _chestSlot, 0); // TEMPORARY ONLY 1 TYPE OF CHEST IS AVAILABLE
		PlayerPrefs.SetInt(ChestHandlerPlayerPrefKeys.KEY_CHESTLEVEL + "" + _chestSlot, all_ChestSlots[_chestSlot].chestLevelIndex);
	}

	private void UpdateChestSlotStartedUnlocking()
	{
		PlayerPrefs.SetString(ChestHandlerPlayerPrefKeys.KEY_CHESTUNLOCKTIME, dt_ChestUnlockTime.ToBinary().ToString());
		PlayerPrefs.SetInt(ChestHandlerPlayerPrefKeys.KEY_CHESTSTATUS + "" + currentChestUnlockInProgressIndex, 2);
	}

	private void UpdateChestSlotUnlocked()
	{
		PlayerPrefs.SetInt(ChestHandlerPlayerPrefKeys.KEY_CHESTSTATUS + "" + currentChestUnlockInProgressIndex, 3);
	}

	private void UpdateChestSlotCollected(int _chestSlot)
	{
		PlayerPrefs.SetInt(ChestHandlerPlayerPrefKeys.KEY_CHESTSTATUS + "" + _chestSlot, 0);
	}

	#endregion
}
