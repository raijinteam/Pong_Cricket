using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WicketsTotalAchievement : AchievementBase
{
    [SerializeField] private int minimumWickets;
    [SerializeField] private int maximumWickets;
    [SerializeField] private int currentTarget;
    [SerializeField] private int currentProgress;


    private void OnEnable() {
        DailyTaskManager.increaseedWicket += Increasedwicket;
    }

   

    private void OnDisable() {
        DailyTaskManager.increaseedWicket -= Increasedwicket;
    }

    private void Increasedwicket(int _wicket) {

        if (hasCompletedTask) {
            return;
        }

        taskShowData taskData = new taskShowData();
        taskData.taskName = str_AchievementDescription;
        taskData.prevousValue = currentProgress;
        taskData.UpdateValue = currentProgress + _wicket;
        taskData.targetValue = currentTarget;


        DailyTaskManager.Instance.AddShownList(taskData);

        currentProgress += _wicket;
        if (currentProgress >= currentTarget) {
            currentProgress = currentTarget;
            hasCompletedTask = true;
            // DailyTaskManager.Instance.UpdateCurrentTaskPointsValue(achievementPoints);         
        }

        DailyTaskManager.Instance.SaveTaskProgress(taskIndex, currentProgress);
        UIManager.Instance.ui_HomeScreen.SetDailyTaskPanel(); // TEMP CODE
    }

    public override void SetTaskCompletionTarget()
    {
        currentTarget = Random.Range(minimumWickets, maximumWickets);
        str_AchievementDescription = "Take " + currentTarget + " wickets";

        currentProgress = 0;
        hasCompletedTask = false;
        hasClaimedTheTaskReward = false;
    }

    public override void SetCurrentTargetAndProgress(int _target, int _progress)
    {
        currentTarget = _target;
        currentProgress = _progress;

        if (currentProgress >= currentTarget)
        {
            currentProgress = currentTarget;
            hasCompletedTask = true;
        }

        str_AchievementDescription = "Take " + currentTarget + " wickets";
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
