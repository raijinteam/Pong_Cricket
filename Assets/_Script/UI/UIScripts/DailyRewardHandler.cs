using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyRewardHandler : MonoBehaviour
{
    [SerializeField] private bool isDailyRewardActive;
    [SerializeField] private int currentDayIndexForDailyReward;
	private DateTime dt_NextRewardTime;
  

    [Header("Daily Reward Config")]
    [SerializeField] private int rewardHours = 6;
    [SerializeField] private int rewardMinutes = 0;
    [SerializeField] private int rewardSeconds = 0;

    [Header("Reward Data for 7 days")]
    [SerializeField] private int[] all_RewardAmounts;
    [SerializeField] private Sprite[] all_RewardSprites;
    [SerializeField] private DailyRewardType[] all_RewardType;

	public void SetDailyRewardAvailability()
	{
		if (PlayerPrefs.HasKey(RewardPlayerPrefKeys.KEY_NEXTDAILYREWARDTIME))
		{
			FetchDailyRewardData();				
		}
		else
		{		
			SetFirstTimeData();
		}
		
	}

	private void SetFirstTimeData()
	{
		isDailyRewardActive = true;
		PlayerPrefs.SetInt(RewardPlayerPrefKeys.KEY_CURRENTACTIVEDAYFORDAILYREWARD, currentDayIndexForDailyReward);
	}

	private void FetchDailyRewardData()
	{
		string storedTime = PlayerPrefs.GetString(RewardPlayerPrefKeys.KEY_NEXTDAILYREWARDTIME);
		dt_NextRewardTime = DateTime.FromBinary(Convert.ToInt64(storedTime));

		currentDayIndexForDailyReward = PlayerPrefs.GetInt(RewardPlayerPrefKeys.KEY_CURRENTACTIVEDAYFORDAILYREWARD, currentDayIndexForDailyReward);
		CheckIfWeCanRewardNow();
	}

	private void CheckIfWeCanRewardNow()
	{
		if(GetCurrentTimeLeft() <= TimeSpan.Zero)
		{
			isDailyRewardActive = true;

			if (currentDayIndexForDailyReward >= all_RewardAmounts.Length) // If current day index gets more than total days, reset it.
			{
				currentDayIndexForDailyReward = 0;
			}
		}
		else
		{
			isDailyRewardActive = false;
		}
	}


	public void CalcuateDailyRewardTime()
	{

		if (GetCurrentTimeLeft() <= TimeSpan.Zero)
		{
			isDailyRewardActive = true;
			if (currentDayIndexForDailyReward >= all_RewardAmounts.Length) // If current day index gets more than total days, reset it.
			{
				currentDayIndexForDailyReward = 0;
			}

			UIManager.Instance.ui_HomeScreen.DailyRewardIsAvailableAgain(); // Send message to home panel that daily reward is available again
		}		
	}

	public void ClaimedCurrentDailyReward()
	{
		isDailyRewardActive = false;	
		currentDayIndexForDailyReward += 1;
		SaveNextRewardTime();
	}

	private void SaveNextRewardTime()
	{
		dt_NextRewardTime = DateTime.Now.Add(new TimeSpan(rewardHours, rewardMinutes, rewardSeconds));
		PlayerPrefs.SetString(RewardPlayerPrefKeys.KEY_NEXTDAILYREWARDTIME, dt_NextRewardTime.ToBinary().ToString());
		PlayerPrefs.SetInt(RewardPlayerPrefKeys.KEY_CURRENTACTIVEDAYFORDAILYREWARD, currentDayIndexForDailyReward);

		Debug.Log("DIT NEXT 1: " + dt_NextRewardTime.ToBinary().ToString());
	}

	public TimeSpan GetCurrentTimeLeft()
	{
		return dt_NextRewardTime - DateTime.Now; 
	}

	public bool GetIsDailyRewardActive()
	{
		return isDailyRewardActive;
	}

	public int GetCurrentDayIndex()
	{
		return currentDayIndexForDailyReward;
	}

	public int GetRewardAmount(int _index)
	{
		return all_RewardAmounts[_index];
	}

	public Sprite GetRewardSprite(int _index)
	{
		return all_RewardSprites[_index];
	}

	public DailyRewardType GetRewardType(int _index)
	{
		return all_RewardType[_index];
	}
}

public enum DailyRewardType
{
    Coins,
    Gems,
    SkipIts,
    Chest
}
