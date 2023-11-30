using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyRunsRewardHandler : MonoBehaviour
{
    private DateTime dt_NextRewardTime;

    [Header("Daily Runs Reward Data")]
    private int targetRunsRequired = 200;
    private int currentRunsScored = 0;
    private bool hasCompletedDailyTarget = false;
    private bool hasClaimedDailyRunsReward = false;

    [Header("Time Config")]
    [SerializeField] private int timeInHours = 6;
    [SerializeField] private int timeInMinutes = 0;
    [SerializeField] private int timeInSeconds = 0;

    public delegate void DailyChallengeRunsUpdated();
    public static event DailyChallengeRunsUpdated dailyChallengeRunsUpdated;

    private void OnEnable()
    {
        DailyTaskManager.AddRunsHandler += AddRunsToThisTask;
    }

    private void OnDisable()
    {
        DailyTaskManager.AddRunsHandler -= AddRunsToThisTask;
    }

    public void SetDailyRunsChallengeAvailability()
	{
        if (PlayerPrefs.HasKey(RewardPlayerPrefKeys.KEY_CURRENTRUNSPROGRESS))
        {
            // Fetch Data
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
        PlayerPrefs.SetInt(RewardPlayerPrefKeys.KEY_CURRENTRUNSPROGRESS, currentRunsScored);
        PlayerPrefs.SetInt(RewardPlayerPrefKeys.KEY_CURRENTDAILYRUNSCLAIMEDSTATUS, 0);
	}

    private void FetchData()
	{      
        currentRunsScored = PlayerPrefs.GetInt(RewardPlayerPrefKeys.KEY_CURRENTRUNSPROGRESS, currentRunsScored);
        if (currentRunsScored >= targetRunsRequired)
        {
            hasCompletedDailyTarget = true;
        }

        int statusValue = PlayerPrefs.GetInt(RewardPlayerPrefKeys.KEY_CURRENTDAILYRUNSCLAIMEDSTATUS, 0);
        if(statusValue == 1)
		{
            hasClaimedDailyRunsReward = true;
		}

		if (hasClaimedDailyRunsReward)
		{
            string storedTime = PlayerPrefs.GetString(RewardPlayerPrefKeys.KEY_NEXTDAILYRUNSRESETTIME);
            dt_NextRewardTime = DateTime.FromBinary(Convert.ToInt64(storedTime));
            if (GetCurrentTimeLeft() <= TimeSpan.Zero)
            {
                ActivateDailyRunsChallengeAgain();
            }
        }
       
    }

    public void CalcuateResetTime()
    {
        if (GetCurrentTimeLeft() <= TimeSpan.Zero)
        {
            ActivateDailyRunsChallengeAgain();
        }
    }

    private void ActivateDailyRunsChallengeAgain()
	{
        hasClaimedDailyRunsReward = false;
        hasCompletedDailyTarget = false;
        currentRunsScored = 0;
        SaveCurrentRunsScored();
        UpdateRewardClaimStatus(0);

        dailyChallengeRunsUpdated?.Invoke();
    }

    private void AddRunsToThisTask(int _runs)
	{
		if (hasCompletedDailyTarget)
		{
            return;
		}

        currentRunsScored += _runs;
        if(currentRunsScored >= targetRunsRequired)
		{
            currentRunsScored = targetRunsRequired;
            hasCompletedDailyTarget = true;
		}

        SaveCurrentRunsScored();
        dailyChallengeRunsUpdated?.Invoke();
    }

    public void RewardClaimedByUser()
	{
        hasClaimedDailyRunsReward = true;
        UpdateRewardClaimStatus(1);
        SetTimeForNextReward();
    }

    public TimeSpan GetCurrentTimeLeft()
    {
        return dt_NextRewardTime - DateTime.Now;
    }

    private void SetTimeForNextReward()
	{
        dt_NextRewardTime = DateTime.Now.Add(new TimeSpan(timeInHours, timeInMinutes, timeInSeconds));
        PlayerPrefs.SetString(RewardPlayerPrefKeys.KEY_NEXTDAILYRUNSRESETTIME, dt_NextRewardTime.ToBinary().ToString());
    }

    public int GetTargetRunsRequired()
	{
        return targetRunsRequired;
	}

    public int GetCurrentRunsProgress()
	{
        return currentRunsScored;
	}

    public bool HasCompletedTarget()
	{
        return hasCompletedDailyTarget;
    }

    public bool HasClaimedReward()
	{
        return hasClaimedDailyRunsReward;
	}

    // DATABASE SAVE AND UPDATE FUNCTIONS BELOW

    private void SaveCurrentRunsScored()
	{
        PlayerPrefs.SetInt(RewardPlayerPrefKeys.KEY_CURRENTRUNSPROGRESS, currentRunsScored);
    }

    private void UpdateRewardClaimStatus(int _value)
	{
        PlayerPrefs.SetInt(RewardPlayerPrefKeys.KEY_CURRENTDAILYRUNSCLAIMEDSTATUS, _value);
	}
}
