using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreRunsSingleMatchAchievement : AchievementBase
{
    [SerializeField] private int minimumRuns;
    [SerializeField] private int maximumRuns;
    [SerializeField] private int runsToScore;
    [SerializeField] private int currentTarget; // 1
    [SerializeField] private int currentProgress;

    // Private Player prefs
    private string key_RunsToScore = "RunsToScoreInSingleMatchTask";



    private void OnEnable() {
        DailyTaskManager.increasedRunWhenGameOver += RunIncreased;
    }

    private void RunIncreased(int _Run) {
        if (hasCompletedTask) {
            return;
        }

        taskShowData taskData = new taskShowData();
        taskData.taskName = str_AchievementDescription;
        taskData.prevousValue = currentProgress;
        taskData.UpdateValue = currentProgress + _Run;
        taskData.targetValue = currentTarget;

        DailyTaskManager.Instance.AddShownList(taskData);

        currentProgress += _Run;
        if (currentProgress >= currentTarget) {
            currentProgress = currentTarget;
            hasCompletedTask = true;
            // DailyTaskManager.Instance.UpdateCurrentTaskPointsValue(achievementPoints);         
        }

        DailyTaskManager.Instance.SaveTaskProgress(taskIndex, currentProgress);
        UIManager.Instance.ui_HomeScreen.SetDailyTaskPanel(); // TEMP COD
    }

   

    private void OnDisable() {
        DailyTaskManager.increasedRunWhenGameOver -= RunIncreased;
    }

    public override void SetTaskCompletionTarget()
    {
        currentTarget = 1;
        runsToScore = Random.Range(minimumRuns, maximumRuns);
        str_AchievementDescription = "Score " + runsToScore + " runs in a single match";

        currentProgress = 0;
        hasCompletedTask = false;
        hasClaimedTheTaskReward = false;

        PlayerPrefs.SetInt(key_RunsToScore, runsToScore);
    }

    public override void SetCurrentTargetAndProgress(int _target, int _progress)
    {
        currentTarget = _target;
        currentProgress = _progress;

        runsToScore = PlayerPrefs.GetInt(key_RunsToScore, 0);

        if (currentProgress >= currentTarget)
        {
            currentProgress = currentTarget;
            hasCompletedTask = true;
        }

        str_AchievementDescription = "Score " + runsToScore + " runs in a single match";
    }

    public override int GetTaskCurrentProgress()
    {
        return currentProgress;
    }

    public override int GetTaskTarget()
    {
        return currentTarget;
    }

}
