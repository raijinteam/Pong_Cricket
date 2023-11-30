using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRouletteRewardHandler : MonoBehaviour
{
    [SerializeField] private bool isWheelRouletteActive;
    private DateTime dt_NextRewardTime;

    [Header("Daily Reward Config")]
    [SerializeField] private int rewardHours = 6;
    [SerializeField] private int rewardMinutes = 0;
    [SerializeField] private int rewardSeconds = 0;

    [Header("Reward Data for Wheel")]
    [SerializeField] private int[] all_RewardAmounts;
    [SerializeField] private Sprite[] all_RewardSprites;
    [SerializeField] private WheelRouletteRewardType[] all_RewardType;
    [SerializeField] private int[] all_RewardProbability;

    public void SetWheelSpinAvailability()
	{
		if (PlayerPrefs.HasKey(RewardPlayerPrefKeys.KEY_NEXTWHEELSPINREWARDTIME))
		{
            FetchData();
		}
		else
		{
            SaveFirstTimeData();
		}
	}

    private void SaveFirstTimeData()
	{
        isWheelRouletteActive = true;
	}

    private void FetchData()
	{   
        string storedTime = PlayerPrefs.GetString(RewardPlayerPrefKeys.KEY_NEXTWHEELSPINREWARDTIME);
        dt_NextRewardTime = DateTime.FromBinary(Convert.ToInt64(storedTime));

        CheckIfWeCanSpinNow();
    }

    private void CheckIfWeCanSpinNow()
    {
        if (DataManager.Instance.skipIts > 0)
        {
            isWheelRouletteActive = true;
            return;
        }

        if (GetCurrentTimeLeft() <= TimeSpan.Zero)
        {
            isWheelRouletteActive = true;        
        }
        else
        {
            isWheelRouletteActive = false;
        }
    }

    public void CalcuateDailyRewardTime()
    {
        if (GetCurrentTimeLeft() <= TimeSpan.Zero)
        {
            ActivateWheelRoulette();       
        }
    }

    public void ActivateWheelRoulette()
	{
        isWheelRouletteActive = true;
        UIManager.Instance.ui_HomeScreen.WheelRouletteIsAvailableAgain(); // Send message to home panel that wheel roulette reward is available again
    }

    public void WheelRouletteClaimed()
	{
        isWheelRouletteActive = false;
        SaveNextRewardTime();
    }

    private void SaveNextRewardTime()
    {
        dt_NextRewardTime = DateTime.Now.Add(new TimeSpan(rewardHours, rewardMinutes, rewardSeconds));
        PlayerPrefs.SetString(RewardPlayerPrefKeys.KEY_NEXTWHEELSPINREWARDTIME, dt_NextRewardTime.ToBinary().ToString());
    }

    public TimeSpan GetCurrentTimeLeft()
    {
        return dt_NextRewardTime - DateTime.Now;
    }

    public int GetRewardAmount(int _index)
	{
        return all_RewardAmounts[_index];
	}

    public Sprite GetRewardIcon(int _index)
	{
        return all_RewardSprites[_index];
	}

    public int GetRewardProbability(int _index)
	{
        return all_RewardProbability[_index];
    }

    public bool IsWheelRouletteActive()
	{
        return isWheelRouletteActive;
    }
}

public enum WheelRouletteRewardType
{
    Coins,
    Gems,
    SkipIts,
    Cards
}
