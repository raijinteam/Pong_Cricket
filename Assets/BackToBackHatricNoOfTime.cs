using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToBackHatricNoOfTime : AchievementBase {

    [SerializeField] private int minTarget;
    [SerializeField] private int maxTarget;
    [SerializeField] private int currentTarget;
    [SerializeField] private int currentProgress;
    private int Current;


    private void OnEnable() {
        DailyTaskManager.BackToBackHatTrickBoundry += BackToBackBoundry;
    }

    private void OnDisable() {
        DailyTaskManager.BackToBackHatTrickBoundry -= BackToBackBoundry;
    }



    private void BackToBackBoundry(bool isBoudry) {

        if (isBoudry) {
            Current++;
        }
        else {
            Current = 0;
        }

        if (Current < 3) {
            return;
        }
      
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
        currentTarget = Random.Range(minTarget, maxTarget);
        str_AchievementDescription = "Back To back hat- Trick Boundry " + currentTarget + " no Time";

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

        str_AchievementDescription = "Back To back hat- Trick Boundry " + currentTarget + "no Time";
    }

    public override int GetTaskCurrentProgress() {
        return currentProgress;
    }

    public override int GetTaskTarget() {
        return currentTarget;
    }

}

