using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGameLoosingWicket : AchievementBase {

    [SerializeField] private int currentTarget;
    [SerializeField] private int currentProgress;


    private void OnEnable() {
        DailyTaskManager.winGameLoosingWicket += WinGameInNoWicket;
    }

    private void OnDisable() {

        DailyTaskManager.winGameLoosingWicket -= WinGameInNoWicket;
    }

    private void WinGameInNoWicket() {
        if (hasCompletedTask) {
            return;
        }

        taskShowData taskData = new taskShowData();
        taskData.taskName = str_AchievementDescription;
        taskData.prevousValue = currentProgress;
        taskData.UpdateValue = currentProgress + currentTarget;
        taskData.targetValue = currentTarget;
        DailyTaskManager.Instance.AddShownList(taskData);

        currentProgress += currentTarget;
        if (currentProgress >= currentTarget) {
            currentProgress = currentTarget;
            hasCompletedTask = true;
            // DailyTaskManager.Instance.UpdateCurrentTaskPointsValue(achievementPoints);         
        }

        DailyTaskManager.Instance.SaveTaskProgress(taskIndex, currentProgress);
        UIManager.Instance.ui_HomeScreen.SetDailyTaskPanel(); // TEMP CODE
    }

   




    public override void SetTaskCompletionTarget() {
        currentTarget = 100;
        str_AchievementDescription = "Win game Loosing wicket";

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

        str_AchievementDescription = "Win Game Loosing Wicket";
    }

    public override int GetTaskCurrentProgress() {
        return currentProgress;
    }

    public override int GetTaskTarget() {
        return currentTarget;
    }
}
