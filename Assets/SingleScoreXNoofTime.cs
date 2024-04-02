using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleScoreXNoofTime : AchievementBase {

    [SerializeField] private int minimumSingleRun;
    [SerializeField] private int maximumSingleRun;
    [SerializeField] private int currentTarget;
    [SerializeField] private int currentProgress;


    private void OnEnable() {
        DailyTaskManager.singleScoreGet += SingleRunget;
    }

   

    private void OnDisable() {
        DailyTaskManager.singleScoreGet -= SingleRunget;
    }

    private void SingleRunget() {

        if (hasCompletedTask) {
            return;
        }

        taskShowData taskData = new taskShowData();
        taskData.taskName = str_AchievementDescription;
        taskData.prevousValue = currentProgress;
        taskData.UpdateValue = currentProgress + 1;
        taskData.targetValue = currentTarget;


        DailyTaskManager.Instance.AddShownList(taskData);

        currentProgress += 1;
        if (currentProgress >= currentTarget) {
            currentProgress = currentTarget;
            hasCompletedTask = true;
            // DailyTaskManager.Instance.UpdateCurrentTaskPointsValue(achievementPoints);         
        }

        DailyTaskManager.Instance.SaveTaskProgress(taskIndex, currentProgress);
        UIManager.Instance.ui_HomeScreen.SetDailyTaskPanel(); // TEMP CODE
    }

   

    public override void SetTaskCompletionTarget() {
        currentTarget = Random.Range(minimumSingleRun, maximumSingleRun);
        str_AchievementDescription = "Single Run " + currentTarget + " no Of Time";

        currentProgress = 0;
        hasCompletedTask = false;
        hasClaimedTheTaskReward = false;
    }

    public override void SetCurrentTargetAndProgress(int _target, int _progress) {
        currentTarget = _target;
        currentProgress = _progress;

        if (currentProgress >= currentTarget) {
            currentProgress = currentTarget;
            hasCompletedTask = true;
        }

        str_AchievementDescription = "single Run " + currentTarget + "No of Time";
    }

    public override int GetTaskCurrentProgress() {
        return currentProgress;
    }

    public override int GetTaskTarget() {
        return currentTarget;
    }
}
