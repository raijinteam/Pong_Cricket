using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyRunsChallengeUI : MonoBehaviour
{
	[SerializeField] private ChestShopScriptableObject chest;
    [SerializeField] private TextMeshProUGUI txt_Progress;
    [SerializeField] private Slider slider_Progress;
	[SerializeField] private GameObject panel_ChallengeRunning;
	[SerializeField] private GameObject panel_ChallengeCompleted;
	[SerializeField] private GameObject panel_WaitTime;
	[SerializeField] private TextMeshProUGUI txt_WaitTimeForNextReward;
	[SerializeField] private Image img_Icon;

	private void OnEnable()
	{
        //SetChallengeUI();
        DataManager.Instance.Changesprite += ChangeIconSprite;
        RewardsManager.Instance.dailyRunsRewardData.dailyChallengeRunsUpdate += SetChallengeUI;

	}

	private void OnDisable()
	{

        DataManager.Instance.Changesprite -= ChangeIconSprite;
        RewardsManager.Instance.dailyRunsRewardData.dailyChallengeRunsUpdate -= SetChallengeUI;
	}

    private void ChangeIconSprite(Sprite sprite) {
		img_Icon.sprite = sprite;
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

		img_Icon.sprite = DataManager.Instance.GetSprite();
	}

	public void OnClick_ClaimReward()
	{
		RewardsManager.Instance.dailyRunsRewardData.RewardClaimedByUser();

		UIManager.Instance.ui_ChestOpping.ActavetedShopBaseChest(chest , 0);
		SetChallengeUI();
	}

	public void OnClick_SkipButton() {

        DataManager.Instance.RemoveSkipIts();
        RewardsManager.Instance.dailyRunsRewardData.SkipTimeByUser();
        SetChallengeUI();
    }
}
