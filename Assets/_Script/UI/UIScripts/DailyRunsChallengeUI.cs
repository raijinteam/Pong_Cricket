using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyRunsChallengeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txt_Progress;
    [SerializeField] private Slider slider_Progress;
	[SerializeField] private GameObject panel_ChallengeRunning;
	[SerializeField] private GameObject panel_ChallengeCompleted;
	[SerializeField] private GameObject panel_WaitTime;
	[SerializeField] private TextMeshProUGUI txt_WaitTimeForNextReward;

	private void OnEnable()
	{
		//SetChallengeUI();
		DailyRunsRewardHandler.dailyChallengeRunsUpdated += SetChallengeUI;
	}

	private void OnDisable()
	{
		DailyRunsRewardHandler.dailyChallengeRunsUpdated -= SetChallengeUI;
	}

	private void Start()
	{
		SetChallengeUI();
	}

	private void Update()
	{
		if (RewardsManager.Instance.dailyRunsRewardData.HasClaimedReward())
		{
			string formattedTime = UtilityManager.Instance.FormatTimeToString(RewardsManager.Instance.dailyRunsRewardData.GetCurrentTimeLeft());
			txt_WaitTimeForNextReward.text = formattedTime;
		}
	}

	private void SetChallengeUI()
	{
		if (RewardsManager.Instance.dailyRunsRewardData.HasCompletedTarget())
		{
			// COMPLETED THE CHALLENGE
			panel_ChallengeRunning.SetActive(false);
			
			if (RewardsManager.Instance.dailyRunsRewardData.HasClaimedReward())
			{
				// CLAIMED THE CHALLENGE
				panel_ChallengeCompleted.SetActive(false);
				panel_WaitTime.SetActive(true);
			}
			else
			{
				// CHALLENGE COMPLETED BUT HAS NOT CLAIMED YET
				panel_ChallengeCompleted.SetActive(true);
				panel_WaitTime.SetActive(false);
			}
		}
		else
		{
			int currentTarget = RewardsManager.Instance.dailyRunsRewardData.GetTargetRunsRequired();
			int currentProgress = RewardsManager.Instance.dailyRunsRewardData.GetCurrentRunsProgress();

			txt_Progress.text = currentProgress + " / " + currentTarget;
			slider_Progress.maxValue = currentTarget;
			slider_Progress.value = currentProgress;

			panel_ChallengeRunning.SetActive(true);
			panel_ChallengeCompleted.SetActive(false);
			panel_WaitTime.SetActive(false);
		}		
	}

	public void OnClick_ClaimReward()
	{
		RewardsManager.Instance.dailyRunsRewardData.RewardClaimedByUser();
		SetChallengeUI();
	}
}
