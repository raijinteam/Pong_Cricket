using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardUI : MonoBehaviour
{
	[SerializeField] private GameObject[] panel_All_RewardActivebg;
	[SerializeField] private GameObject[] panel_All_RewardClaimedbg;
	[SerializeField] private GameObject[] panel_AllRewardActve;
	[SerializeField] private GameObject[] panel_AllClaimed;
	[SerializeField] private GameObject panel_Timer;
    [SerializeField] private TextMeshProUGUI[] txt_All_RewardAmounts;
	[SerializeField] private Image[] img_All_RewardImages;
	[SerializeField] private TextMeshProUGUI txt_TimeLeft;

	private void OnEnable()
	{
		SetAllPanels();
	}

	private void Start()
	{
		SetAllRewardAmounts();
	}

	private void Update()
	{
		if (!RewardsManager.Instance.dailyRewardData.GetIsDailyRewardActive())
		{
			// if reward is not active, show timer.
			TimeSpan timeLeftInUnlockingProcess = RewardsManager.Instance.dailyRewardData.GetCurrentTimeLeft();
			string formattedTime = UtilityManager.Instance.FormatTimeToString(timeLeftInUnlockingProcess);
			txt_TimeLeft.text = formattedTime;
		}
	}

	public void SetAllPanels()
	{
		// Turn off all the reward active panels
		for(int i = 0; i < panel_All_RewardActivebg.Length; i++)
		{
            panel_All_RewardActivebg[i].SetActive(false);
			panel_AllRewardActve[i].SetActive(false);
		}

		if (RewardsManager.Instance.dailyRewardData.GetIsDailyRewardActive())
		{
            // if daily reward is active, show reward active panel on current day.
            panel_AllRewardActve[RewardsManager.Instance.dailyRewardData.GetCurrentDayIndex()].SetActive(true);
            panel_All_RewardActivebg[RewardsManager.Instance.dailyRewardData.GetCurrentDayIndex()].SetActive(true);
            panel_Timer.SetActive(false);
		}
		else
		{
			panel_Timer.SetActive(true);
		}

		// Turn of all reward claimed panels
		for(int i = 0; i < panel_All_RewardClaimedbg.Length; i++)
		{
			panel_All_RewardClaimedbg[i].SetActive(false);
            panel_AllClaimed[i].SetActive(false);
		}

		// Turn on reward claimed panels till the current day index
		for(int i = 0; i < RewardsManager.Instance.dailyRewardData.GetCurrentDayIndex(); i++)
		{
            panel_All_RewardClaimedbg[i].SetActive(true);
            panel_AllClaimed[i].SetActive(true);
        }
	}

	private void SetAllRewardAmounts()
	{
		for(int i = 0; i < txt_All_RewardAmounts.Length; i++)
		{

			int rewardAmount = RewardsManager.Instance.dailyRewardData.GetRewardAmount(i);
			string formattedValue = UtilityManager.Instance.FormatIntegerValueToStringWithComma(rewardAmount);
			txt_All_RewardAmounts[i].text = "x" + formattedValue;


			img_All_RewardImages[i].sprite = RewardsManager.Instance.dailyRewardData.GetRewardSprite(i);
		}
	}

	public void OnClick_ClaimReward(int _index)
	{
		if (!RewardsManager.Instance.dailyRewardData.GetIsDailyRewardActive())
		{
			// No reward is active at the moment
			Debug.Log("Daily reward is not active right now");
			return;
		}
		else if (RewardsManager.Instance.dailyRewardData.GetCurrentDayIndex() != _index)
		{
			// Did not click on the correct active button
			Debug.Log("Claim Reward clicked button is not the same as current Day.");
			return;
		}

		// Give reward and Reset Daily Reward
		RewardsManager.Instance.dailyRewardData.ClaimedCurrentDailyReward();
		UIManager.Instance.ui_HomeScreen.DailyRewardClaimed();
		SetAllPanels();
	}

	public void OnClick_OnClosed() {
		this.gameObject.SetActive(false);
	}
}
