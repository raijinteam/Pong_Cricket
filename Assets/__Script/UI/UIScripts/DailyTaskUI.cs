using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class DailyTaskUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txt_TimeLeft;

    [Header("Reward Data")]
    [SerializeField] private TextMeshProUGUI txt_RewardProgress;
    [SerializeField] private Slider slider_RewardProgress;
    [SerializeField] private GameObject btn_Climed;


    [Header("Task Data")]
    [SerializeField] private Image[] all_TaskRewardImage;
    [SerializeField] private TextMeshProUGUI[] all_txt_TaskDescription;
    [SerializeField] private TextMeshProUGUI[] all_txt_TaskProgress;
    [SerializeField] private TextMeshProUGUI[] all_txt_RewardValue;
    [SerializeField] private TextMeshProUGUI[] all_txt_LevelPoints;
    [SerializeField] private TextMeshProUGUI[] all_txt_AchievementPoints;
    [SerializeField] private Slider[] all_slider_Progress;
    [SerializeField] private GameObject[] all_panel_TaskCompleted;
    [SerializeField] private GameObject[] all_panel_RewardInfo;
    [SerializeField] private GameObject[] all_btn_ChangeTask;
    [SerializeField] private GameObject[] all_btn_ClaimReward;


    [Header("Animation")]
    [SerializeField] private RectTransform rect_Main;
    [SerializeField] private float flt_AnimtionTime;
    [SerializeField] private float flt_CloseAnimation; 


    
    
	private void OnEnable()
	{
		SetTaskData();
        SetTaskRewardPanel();
        Panel_Animation.instance.Enable_PopUp(rect_Main, flt_AnimtionTime);
    }


	private void Update()
	{
        string formattedTime = UtilityManager.Instance.FormatTimeToString(DailyTaskManager.Instance.GetCurrentTimeLeft());
        txt_TimeLeft.text = formattedTime;
    }

	public void SetTaskData()
	{
        for(int i = 0; i < all_txt_TaskDescription.Length; i++)
		{
            all_TaskRewardImage[i].sprite = DailyTaskManager.Instance.DailyGetTaskRewardSprite(i);
            all_txt_TaskDescription[i].text = DailyTaskManager.Instance.GetTaskDetail(i);
            int currentProgress = DailyTaskManager.Instance.GetTaskCurrentProgress(i);
            int target = DailyTaskManager.Instance.GetTaskTarget(i);
            all_txt_TaskProgress[i].text = currentProgress + " / " + target;
            all_txt_RewardValue[i].text = DailyTaskManager.Instance.GetTaskRewardValue(i).ToString();
            all_txt_LevelPoints[i].text =  "+" + DailyTaskManager.Instance.GetTaskCompletionLevelPoints(i);
            all_txt_AchievementPoints[i].text = DailyTaskManager.Instance.GetTaskCompletionAchievementPoints(i).ToString();

            all_slider_Progress[i].maxValue = target;
            all_slider_Progress[i].value = currentProgress;

           

            if (DailyTaskManager.Instance.GetTaskCompletionStatus(i))
			{
                // task has been completed

               
                all_btn_ChangeTask[i].SetActive(false);

                // Check if reward has been claimed
                if (DailyTaskManager.Instance.GetTaskRewardClaimStatus(i))
				{
                    // Has Claimed The Reward from the task
                    all_panel_TaskCompleted[i].SetActive(true);                  
                    all_btn_ClaimReward[i].SetActive(false);
                }
				else
				{
                    // Task has been completed but reward not claimed yet.
                    all_panel_TaskCompleted[i].SetActive(false);
                    all_btn_ClaimReward[i].SetActive(true);
                }
			}
			else
			{
                all_panel_TaskCompleted[i].SetActive(false);
               
                all_btn_ChangeTask[i].SetActive(AdsManager.instance.IsRewardAdLoad);
                all_btn_ClaimReward[i].SetActive(false);
            }
        }
        SetTaskRewardPanel();
    }

   

    private void SetTaskRewardPanel()
	{
        int requiredPoints = DailyTaskManager.Instance.GetRequiredPointsForReward();
        int currentPoints = DailyTaskManager.Instance.GetCurrentPointsReachedForReward();

        slider_RewardProgress.maxValue = requiredPoints;
        slider_RewardProgress.value = currentPoints;

        txt_RewardProgress.text = currentPoints + " / " + requiredPoints;

        btn_Climed.SetActive(DailyTaskManager.Instance.isClaimPointTask);

    }

    public void OnClick_ClaimReward(int _index)
	{
        AudioManager.insatance.PlayBtnClickSFX();
        DailyTaskManager.Instance.ClaimRewardFromTheTask(_index);
        SetTaskData();
	}

    public void OnClick_Closed() {

        AudioManager.insatance.PlayBtnClickSFX();
        Panel_Animation.instance.Disable_PopUp(rect_Main, flt_AnimtionTime , this.gameObject);
    }

    public void Onclick_onClaimBtn() {
        AudioManager.insatance.PlayBtnClickSFX();
        SetTaskRewardPanel();
        DailyTaskManager.Instance.ClaimPointTaskComplteted();
    }
    private int indexOfSkip;
    public void OnClick_SkipBtn(int index) {
        AudioManager.insatance.PlayBtnClickSFX();
        indexOfSkip = index;

        if (DataManager.Instance.skipIts <= 0) {
            AdsManager.instance.ShowRewardAds(AdsRewardType.SkipDailyTask);
        }
        else {
            DataManager.Instance.RemoveSkipIts();
        }
        
    }

    public void GetSkipTaskReward() {
        DailyTaskManager.Instance.skipTask(indexOfSkip);
    }

   
}
