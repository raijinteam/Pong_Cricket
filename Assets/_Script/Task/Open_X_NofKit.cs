using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Open_X_NofKit : AchievementBase {

    [SerializeField] private int minimumkits;
    [SerializeField] private int maximumKits;
    [SerializeField] private int currentTarget;
    [SerializeField] private int currentProgress;


    private void OnEnable() {
        DailyTaskManager.increasedNoofKit += IncreasedKit;
    }

    private void OnDisable() {
        DailyTaskManager.increasedNoofKit -= IncreasedKit;
    }



    private void IncreasedKit(int _kit) {

        if (hasCompletedTask) {
            return;
        }

        taskShowData taskData = new taskShowData();
        taskData.taskName = str_AchievementDescription;
        taskData.prevousValue = currentProgress;
        taskData.UpdateValue = currentProgress + _kit;
        taskData.targetValue = currentTarget;


        DailyTaskManager.Instance.AddShownList(taskData);

        currentProgress += _kit;
        if (currentProgress >= currentTarget) {
            currentProgress = currentTarget;
            hasCompletedTask = true;
            // DailyTaskManager.Instance.UpdateCurrentTaskPointsValue(achievementPoints);         
        }

        DailyTaskManager.Instance.SaveTaskProgress(taskIndex, currentProgress);
        UIManager.Instance.ui_HomeScreen.SetDailyTaskPanel(); // TEMP CODE
    }

    public override void SetTaskCompletionTarget() {
        currentTarget = Random.Range(minimumkits, maximumKits);
        str_AchievementDescription = "Open " + currentTarget + " no Of Kit";

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

        str_AchievementDescription = "Open " + currentTarget + "Kit";
    }

    public override int GetTaskCurrentProgress() {
        return currentProgress;
    }

    public override int GetTaskTarget() {
        return currentTarget;
    }
}
