using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatTrick_Hero : AchievementBase {

    [SerializeField] private int Max_value = 3;
    [SerializeField] private int currentTarget;
    [SerializeField] private int currentProgress;


    private void OnEnable() {

        DailyTaskManager.isGamewin += IncreasedGame;
    }

    private void OnDisable() {
        DailyTaskManager.isGamewin -= IncreasedGame;
    }

    private void IncreasedGame(bool isWin) {
        if (hasCompletedTask) {
            return;
        }

        if (!isWin) {
            taskShowData taskData = new taskShowData();
            taskData.taskName = str_AchievementDescription;
            taskData.prevousValue = currentProgress;
            taskData.UpdateValue = 0;
            taskData.targetValue = currentTarget;
            DailyTaskManager.Instance.AddShownList(taskData);
           
        }
        else {
            taskShowData taskData = new taskShowData();
            taskData.taskName = str_AchievementDescription;
            taskData.prevousValue = currentProgress;
            taskData.UpdateValue = currentProgress + 1;
            taskData.targetValue = currentTarget;
            DailyTaskManager.Instance.AddShownList(taskData);
            currentProgress += 1;
        }

        if (currentProgress >= currentTarget) {
            currentProgress = currentTarget;
            hasCompletedTask = true;
            // DailyTaskManager.Instance.UpdateCurrentTaskPointsValue(achievementPoints);         
        }

        DailyTaskManager.Instance.SaveTaskProgress(taskIndex, currentProgress);
        UIManager.Instance.ui_HomeScreen.SetDailyTaskPanel(); // TEMP CODE
    }

   


    

    public override void SetTaskCompletionTarget() {
        currentTarget = Max_value;
        str_AchievementDescription = "Hat - Trick Hero";

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

        str_AchievementDescription = "Hat - Trick Hero";
    }

    public override int GetTaskCurrentProgress() {
        return currentProgress;
    }

    public override int GetTaskTarget() {
        return currentTarget;
    }
}
