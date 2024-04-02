using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreRunsTotalAchievement : AchievementBase
{
    [SerializeField] private int minimumRuns;
    [SerializeField] private int maximumRuns;
    [SerializeField] private int currentTarget;
    [SerializeField] private int currentProgress;


    private void OnEnable() {
        DailyTaskManager.increasedRunWhenGameOver += AddRunsToThisTask;
    }

    private void OnDisable() {
        DailyTaskManager.increasedRunWhenGameOver -= AddRunsToThisTask;
    }



    private void AddRunsToThisTask(int _runs)
	{
		if (hasCompletedTask)
		{
            return;
		}

        taskShowData taskData = new taskShowData();
        taskData.taskName = str_AchievementDescription;
        taskData.prevousValue = currentProgress;
        taskData.UpdateValue = currentProgress + _runs;
        taskData.targetValue = currentTarget;


        DailyTaskManager.Instance.AddShownList(taskData);

        currentProgress += _runs;
        if(currentProgress >= currentTarget)
		{
            currentProgress = currentTarget;
            hasCompletedTask = true;
            // DailyTaskManager.Instance.UpdateCurrentTaskPointsValue(achievementPoints);         
        }

        DailyTaskManager.Instance.SaveTaskProgress(taskIndex, currentProgress);
        UIManager.Instance.ui_HomeScreen.SetDailyTaskPanel(); // TEMP CODE
    }

	public override void SetTaskCompletionTarget()
	{
        currentTarget = Random.Range(minimumRuns, maximumRuns);
        str_AchievementDescription = "Score " + currentTarget + " runs";
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

        str_AchievementDescription = "Score " + currentTarget + " runs";
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
