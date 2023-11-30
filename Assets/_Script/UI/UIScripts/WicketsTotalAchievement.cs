using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WicketsTotalAchievement : AchievementBase
{
    [SerializeField] private int minimumWickets;
    [SerializeField] private int maximumWickets;
    [SerializeField] private int currentTarget;
    [SerializeField] private int currentProgress;

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
