using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DailyTaskManager : MonoBehaviour
{
    public static DailyTaskManager Instance;

	private void Awake()
	{
        Instance = this;
	}

    private int pointsRequiredToGetReward = 200;
    [SerializeField] private int currentPointsUserHas = 0;

	[SerializeField] private AchievementBase[] all_Tasks;
    [SerializeField] private AchievementBase[] all_current_ActivatedTasks;
    private int totalTaskToActivate = 3;

    private DateTime dt_DailyTaskResetTime;
    [Header("Daily Task Config")]
    [SerializeField] private int timeInHours;
    [SerializeField] private int timeInMinutes;
    [SerializeField] private int timeInSeconds;


    // TEMPERORY CODE

    public delegate void AddRuns(int value);
    public static event AddRuns AddRunsHandler;

    [SerializeField] private List<taskShowData> list_TaskShowndata;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.S))
		{
            Debug.Log("Score RUns Event");
            AddRunsHandler?.Invoke(Random.Range(200,250));
            UIManager.Instance.ui_taskProgress.ActivetedTaskProgersPanel(list_TaskShowndata);
        }
        

        if (GetCurrentTimeLeft() <= TimeSpan.Zero)
		{
            SelectRandomAchievements();
            SetDailyTaskEndTime();
            SaveDailyTaskData();
        }
    }

	// END TEMPORARY CODE

	private void Start()
	{
        all_current_ActivatedTasks = new AchievementBase[totalTaskToActivate];

        for(int i = 0; i < all_Tasks.Length; i++)
		{
            all_Tasks[i].taskIndex = i;
		}

		if (PlayerPrefs.HasKey(DailyTaskPlayerPrefKeys.key_DailyTaskEndTime))
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
        SelectRandomAchievements();
        SetDailyTaskEndTime();
        SaveDailyTaskData();

        PlayerPrefs.SetInt(DailyTaskPlayerPrefKeys.key_CurrentTaskPoints, 0);
    }

    private void FetchData()
    {
        currentPointsUserHas = PlayerPrefs.GetInt(DailyTaskPlayerPrefKeys.key_CurrentTaskPoints, 0);

        // Check if time for daily task is over?
        string storedTime = PlayerPrefs.GetString(DailyTaskPlayerPrefKeys.key_DailyTaskEndTime);
        dt_DailyTaskResetTime = DateTime.FromBinary(Convert.ToInt64(storedTime));

        if (GetCurrentTimeLeft() <= TimeSpan.Zero)
        {
            // Time Up, reset the tasks
            SelectRandomAchievements();
            SetDailyTaskEndTime();
            SaveDailyTaskData();
            return;
        }

        // Get chosen tasks indexes
        for (int i = 0; i < all_current_ActivatedTasks.Length; i++)
        {
            int taskIndex = PlayerPrefs.GetInt(DailyTaskPlayerPrefKeys.key_ActiveTaskIndex + "" + i);
            all_current_ActivatedTasks[i] = all_Tasks[taskIndex];
        }

        // Get Chosen task target and progress values
        for (int i = 0; i < all_current_ActivatedTasks.Length; i++)
		{
            int targetAmount = PlayerPrefs.GetInt(DailyTaskPlayerPrefKeys.key_CurrentTargetForTask + "" + i);
            int currentProgress = PlayerPrefs.GetInt(DailyTaskPlayerPrefKeys.key_CurrentProgressForTask + "" + i);
            all_current_ActivatedTasks[i].SetCurrentTargetAndProgress(targetAmount, currentProgress);

            int rewardClaimValue = PlayerPrefs.GetInt(DailyTaskPlayerPrefKeys.key_TaskRewardClaimedStatus + "" + i, 0);
            if(rewardClaimValue == 1)
			{
                all_current_ActivatedTasks[i].hasClaimedTheTaskReward = true;
			}
		}

        UIManager.Instance.ui_HomeScreen.SetDailyTaskPanel(); // TEMPORARY
    }


    private void SelectRandomAchievements()
	{
        List<AchievementBase> list_TakenAchievements = new List<AchievementBase>();

        for(int i = 0; i < all_Tasks.Length; i++)
		{
            list_TakenAchievements.Add(all_Tasks[i]);
        }

        for(int i = 0; i < totalTaskToActivate; i++)
		{
            int randomIndex = Random.Range(0, list_TakenAchievements.Count);
            all_current_ActivatedTasks[i] = list_TakenAchievements[randomIndex];
            list_TakenAchievements.RemoveAt(randomIndex);
		}

        SetAchivementTargets();
	}

    private void SetAchivementTargets()
	{
        for(int i = 0; i < all_current_ActivatedTasks.Length; i++)
		{
            all_current_ActivatedTasks[i].SetTaskCompletionTarget();
		}

        UIManager.Instance.ui_HomeScreen.SetDailyTaskPanel();
	}

    private void SetDailyTaskEndTime()
	{
        dt_DailyTaskResetTime = DateTime.Now.Add(new TimeSpan(timeInHours, timeInMinutes, timeInSeconds));
        PlayerPrefs.SetString(DailyTaskPlayerPrefKeys.key_DailyTaskEndTime, dt_DailyTaskResetTime.ToBinary().ToString());
    }

    private void SaveDailyTaskData()
	{
        // first save the chosen task index
        for(int i = 0; i < all_current_ActivatedTasks.Length; i++)
		{
            int taskIndex = all_current_ActivatedTasks[i].taskIndex;
            PlayerPrefs.SetInt(DailyTaskPlayerPrefKeys.key_ActiveTaskIndex + "" + i, taskIndex);
        }

        // Reset and save current progress, target, status for each task;
        for(int i = 0; i < all_current_ActivatedTasks.Length; i++)
		{
            int currentTarget = GetTaskTarget(i);
            PlayerPrefs.SetInt(DailyTaskPlayerPrefKeys.key_CurrentTargetForTask + "" + i, currentTarget);

            int currentProgress = GetTaskCurrentProgress(i);
            PlayerPrefs.SetInt(DailyTaskPlayerPrefKeys.key_CurrentProgressForTask + "" + i, currentProgress);

            PlayerPrefs.SetInt(DailyTaskPlayerPrefKeys.key_TaskRewardClaimedStatus + "" + i, 0);
        }

	}

    public TimeSpan GetCurrentTimeLeft()
    {
        return dt_DailyTaskResetTime - DateTime.Now;
    }

    public void ClaimRewardFromTheTask(int _index)
	{
        UpdateCurrentTaskPointsValue(all_current_ActivatedTasks[_index].achievementPoints); // update task completion points value
        all_current_ActivatedTasks[_index].hasClaimedTheTaskReward = true;

        UpdateTaskRewardClaimStatus(_index);
	}

    public int GetRequiredPointsForReward()
	{
        return pointsRequiredToGetReward;
	}

    public int GetCurrentPointsReachedForReward()
	{
        return currentPointsUserHas;
	}

    public string GetTaskDetail(int _index)
	{
        return all_current_ActivatedTasks[_index].str_AchievementDescription;
	}

    public int GetTaskCurrentProgress(int _index)
	{
        return all_current_ActivatedTasks[_index].GetTaskCurrentProgress();
    }

    public int GetTaskTarget(int _index)
    {
        return all_current_ActivatedTasks[_index].GetTaskTarget();
    }

    public int GetTaskRewardValue(int _index)
    {
        return all_current_ActivatedTasks[_index].rewardValue;
    }

    public int GetTaskCompletionLevelPoints(int _index)
    {
        return all_current_ActivatedTasks[_index].levelPoints;
    }

    public int GetTaskCompletionAchievementPoints(int _index)
    {
        return all_current_ActivatedTasks[_index].achievementPoints;
    }

    public bool GetTaskCompletionStatus(int _index)
	{
        return all_current_ActivatedTasks[_index].hasCompletedTask;
	}

    public bool GetTaskRewardClaimStatus(int _index)
	{
        return all_current_ActivatedTasks[_index].hasClaimedTheTaskReward;
    }

    // SAVE AN UPDATE DATA FROM LOCAL DATABASE FUNCTIONS BELOW

    public void SaveTaskProgress(int _taskindex, int _progress)
	{
        int orderIndex = 0;
        for(int i = 0; i < all_current_ActivatedTasks.Length; i++)
		{
            if (all_current_ActivatedTasks[i].taskIndex == _taskindex)
			{
                orderIndex = i;
                break;
            }
		}

        PlayerPrefs.SetInt(DailyTaskPlayerPrefKeys.key_CurrentProgressForTask + "" + orderIndex, _progress);
	}

   
    public void UpdateCurrentTaskPointsValue(int _amount)
	{
        currentPointsUserHas += _amount;
        if (currentPointsUserHas >= pointsRequiredToGetReward)
		{
            currentPointsUserHas = pointsRequiredToGetReward;
		}

        PlayerPrefs.SetInt(DailyTaskPlayerPrefKeys.key_CurrentTaskPoints, currentPointsUserHas);
    }

    public void ResetCurrentTaskPointsValue()
	{
        currentPointsUserHas = 0;
        PlayerPrefs.SetInt(DailyTaskPlayerPrefKeys.key_CurrentTaskPoints, currentPointsUserHas);
    }

    public void UpdateTaskRewardClaimStatus(int _index)
	{
        PlayerPrefs.SetInt(DailyTaskPlayerPrefKeys.key_TaskRewardClaimedStatus + "" + _index, 1);
    }

    public void AddShownList(taskShowData task) {
        list_TaskShowndata.Add(task);
    }

    public void ResetListData() {
        list_TaskShowndata.Clear();
    }
}

[System.Serializable]
public struct taskShowData {

    public string taskName;
    public int prevousValue;
    public int UpdateValue;
    public int targetValue;
}
