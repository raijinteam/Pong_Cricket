using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardsManager : MonoBehaviour
{
    public static RewardsManager Instance;

	private void Awake()
	{
		Instance = this;
	}

	public DailyRewardHandler dailyRewardData;
	public WheelRouletteRewardHandler wheelRouletteRewardData;
	public DailyRunsRewardHandler dailyRunsRewardData;

	private void Start()
	{
		SetAllRewardData();
	}

	private void Update()
	{
		DailyRewardsManagement();
	}

	private void SetAllRewardData()
	{
		dailyRewardData.SetDailyRewardAvailability();
		wheelRouletteRewardData.SetWheelSpinAvailability();
		dailyRunsRewardData.SetDailyRunsChallengeAvailability();
	}

	private void DailyRewardsManagement()
	{
		if (!dailyRewardData.GetIsDailyRewardActive())
		{
			// Daily reward is not active. Calculate Time ;
			dailyRewardData.CalcuateDailyRewardTime();
		}

		if (!wheelRouletteRewardData.IsWheelRouletteActive())
		{
			// Wheel Reward is not active. Calculate Time 
			wheelRouletteRewardData.CalcuateDailyRewardTime();
		}

		if (dailyRunsRewardData.HasClaimedReward())
		{
			// Show reset time
			dailyRunsRewardData.CalcuateResetTime();
		}
	}	
}
