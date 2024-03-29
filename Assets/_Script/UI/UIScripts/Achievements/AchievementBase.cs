using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AchievementBase : MonoBehaviour
{
    public int taskIndex;
    [HideInInspector] public string str_AchievementDescription;
    
    [field :SerializeField]public int rewardValue { get; set; }
    [field: SerializeField] public int achievementPoints { get; set; }
    [field: SerializeField] public int levelPoints { get; set; }
    [field: SerializeField] public bool hasCompletedTask { get; set; }
    [field: SerializeField] public bool hasClaimedTheTaskReward { get; set; }

    public abstract void SetTaskCompletionTarget();

    public abstract void SetCurrentTargetAndProgress(int _target, int _progress);
    public abstract int GetTaskCurrentProgress();
    public abstract int GetTaskTarget();
   
}

