using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AchievementBase : MonoBehaviour
{
    public int taskIndex;
    [HideInInspector] public string str_AchievementDescription;
    public int rewardValue;
    public int achievementPoints;
    public int levelPoints;
    public bool hasCompletedTask;
    public bool hasClaimedTheTaskReward;
    
    public abstract void SetTaskCompletionTarget();

    public abstract void SetCurrentTargetAndProgress(int _target, int _progress);
    public abstract int GetTaskCurrentProgress();
    public abstract int GetTaskTarget();
   
}

