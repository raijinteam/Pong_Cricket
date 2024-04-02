using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHomeRun_X_NoOfTime : AchievementBase {

    [SerializeField] private int minimumTarget;
    [SerializeField] private int maximumTarget;
    [SerializeField] private int currentTarget;
    [SerializeField] private int currentProgress;


    private void OnEnable() {
        DailyTaskManager.homeRunGet += IncreasedHome;
    }

    private void OnDisable() {
        DailyTaskManager.homeRunGet -= IncreasedHome;
    }



    private void IncreasedHome() {

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
        currentTarget = Random.Range(minimumTarget, maximumTarget);
        str_AchievementDescription = "Score Home Run " + currentTarget + " of Time";

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

        str_AchievementDescription = "Score Home Run  " + currentTarget + "Of Time";
    }

    public override int GetTaskCurrentProgress() {
        return currentProgress;
    }

    public override int GetTaskTarget() {
        return currentTarget;
    }

}
