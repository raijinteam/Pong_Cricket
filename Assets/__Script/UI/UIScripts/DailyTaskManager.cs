using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class DailyTaskManager : MonoBehaviour
{
    public static DailyTaskManager Instance;

	private void Awake()
	{
        Instance = this;  
	}

    private int pointsRequiredToGetReward = 200;
    [SerializeField] private int currentPointsUserHas = 0;
    public bool isClaimPointTask = false;
    [SerializeField] private ChestShopScriptableObject[] all_chestShopScriptableObjects;

	[SerializeField] private AchievementBase[] all_Tasks;
    [SerializeField] private AchievementBase[] all_current_ActivatedTasks;
    private int totalTaskToActivate = 3;

    private DateTime dt_DailyTaskResetTime;
    [Header("Daily Task Config")]
    [SerializeField] private int timeInHours;
    [SerializeField] private int timeInMinutes;
    [SerializeField] private int timeInSeconds;


    // TEMPERORY CODE


  
    [SerializeField] private List<taskShowData> list_TaskShowndata;


    private void TestetaskData() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ShowTaskBar();
        }
        if (Input.GetKeyDown(KeyCode.F1)) {
            UIManager.Instance.spawnPopup("Incresed Run GameOverTime");
            increasedRunWhenGameOver?.Invoke(Random.Range(50, 75));
        }
        else if (Input.GetKeyDown(KeyCode.F2)) {
            UIManager.Instance.spawnPopup("Incresed Wicket");
            increaseedWicket?.Invoke(Random.Range(2, 5));
        }
        else if (Input.GetKeyDown(KeyCode.F3)) {
            UIManager.Instance.spawnPopup("No of kit Increased");
            increasedNoofKit?.Invoke(1);
        }
        else if (Input.GetKeyDown(KeyCode.F4)) {
            UIManager.Instance.spawnPopup("Game Win");
            isGamewin?.Invoke(true);
        }
        else if (Input.GetKeyDown(KeyCode.F5)) {
            UIManager.Instance.spawnPopup("Boundry Blaster");
            boundryBlaster?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.F6    )) {
            UIManager.Instance.spawnPopup("Six Blaster");
            sixBlaster?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.F7)) {
            UIManager.Instance.spawnPopup("winGameLoosingWicket");
            winGameLoosingWicket?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.F8)) {
            UIManager.Instance.spawnPopup("Home Run get");
            homeRunGet?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.F9)) {
            UIManager.Instance.spawnPopup("Activeted Powerup");
            ActvetedPowerup?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.F10)) {
            UIManager.Instance.spawnPopup("startMatch");
            startMatch?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.F11)) {
            UIManager.Instance.spawnPopup("upgradePaddle");
            upgradePaddle?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.F12)) {
            UIManager.Instance.spawnPopup("upgradePowerup");
            upgradePowerup?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.Print)) {
            //UIManager.Instance.spawnPopup("singleScoreGet");
            //singleScoreGet?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.ScrollLock)) {
            UIManager.Instance.spawnPopup("DoubleScoreget");
            DoubleScoreget?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.Pause)) {
            UIManager.Instance.spawnPopup("PlayMiniGame");
            PlayMiniGame?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.Insert)) {
            UIManager.Instance.spawnPopup("chasematch");
            chasematch?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.Home)) {
            UIManager.Instance.spawnPopup("BackToBackHatTrickBoundry");
            BackToBackHatTrickBoundry?.Invoke(true);
        }
    }


    // Task 
    public static Action<int> increasedRunWhenGameOver { get; set; }  // when first innnig complted called
    public static Action<int> increaseedWicket { get; set; }  // when first innnig complted called
    public static Action<int> increasedNoofKit { get; set; }   // when Chest Oppne screen Actveted Calling
    public static Action<bool> isGamewin { get; set; }   // gameManager After complted game result Checking
    public static Action boundryBlaster { get; set; }   // ColliderRunner Calling
    public static Action sixBlaster { get; set; }  // ColliderRunner Calling
    public static Action winGameLoosingWicket { get; set; }  // After Game complete calling
    public static Action homeRunGet { get; set; }   // Colliderrunner calling
    public static Action ActvetedPowerup { get; set; }   // Powerup Activeted
    public static Action startMatch { get; set; }      //  Game Over Calling
    public static Action upgradePaddle { get; set; }   // playerSelection 
    public static Action upgradePowerup { get; set; }   // abilitty upgrade calling
    public static Action singleScoreGet { get; set; }  // collider - Runner
    public static Action DoubleScoreget { get; set; }   // collider - Runner
    public static Action PlayMiniGame { get; set; }   // MiniGamemanager GameOver
    public static Action chasematch { get; set; }     // GameManager
    public static Action<bool> BackToBackHatTrickBoundry { get; set; }  


    private void Update()
	{

        TestetaskData();

        if (GetCurrentTimeLeft() <= TimeSpan.Zero)
		{
            SelectRandomAchievements();
            SetDailyTaskEndTime();
            SaveDailyTaskData();
        }
    }

	// END TEMPORARY CODE

	private void Start()
	{
        all_current_ActivatedTasks = new AchievementBase[totalTaskToActivate];

        for(int i = 0; i < all_Tasks.Length; i++)
		{
            all_Tasks[i].taskIndex = i;
		}

		if (PlayerPrefs.HasKey(DailyTaskPlayerPrefKeys.key_DailyTaskEndTime))
		{
            FetchData();
		}
		else
		{
            SaveFirstTimeData();
		}        
    }

    private void SaveFirstTimeData()
	{
        SelectRandomAchievements();
        SetDailyTaskEndTime();
        SaveDailyTaskData();

        PlayerPrefs.SetInt(DailyTaskPlayerPrefKeys.key_CurrentTaskPoints, 0);
        currentPointsUserHas = 0;
        PlayerPrefs.SetInt(DailyTaskPlayerPrefKeys.key_IsCllaimedPintTask, isClaimPointTask ? 1 : 0);
    }

    private void FetchData()
    {
        currentPointsUserHas = PlayerPrefs.GetInt(DailyTaskPlayerPrefKeys.key_CurrentTaskPoints, 0);
        isClaimPointTask = PlayerPrefs.GetInt(DailyTaskPlayerPrefKeys.key_IsCllaimedPintTask)==1;

        // Check if time for daily task is over?
        string storedTime = PlayerPrefs.GetString(DailyTaskPlayerPrefKeys.key_DailyTaskEndTime);
        dt_DailyTaskResetTime = DateTime.FromBinary(Convert.ToInt64(storedTime));

        if (GetCurrentTimeLeft() <= TimeSpan.Zero)
        {
            // Time Up, reset the tasks
            SelectRandomAchievements();
            SetDailyTaskEndTime();
            SaveDailyTaskData();
            return;
        }

        // Get chosen tasks indexes
        for (int i = 0; i < all_current_ActivatedTasks.Length; i++)
        {
            int taskIndex = PlayerPrefs.GetInt(DailyTaskPlayerPrefKeys.key_ActiveTaskIndex + "" + i);

            AchievementBase current = Instantiate(all_Tasks[taskIndex], transform);
            all_current_ActivatedTasks[i] = current;
        }

        // Get Chosen task target and progress values
        for (int i = 0; i < all_current_ActivatedTasks.Length; i++)
		{
            int targetAmount = PlayerPrefs.GetInt(DailyTaskPlayerPrefKeys.key_CurrentTargetForTask + "" + i);
            int currentProgress = PlayerPrefs.GetInt(DailyTaskPlayerPrefKeys.key_CurrentProgressForTask + "" + i);
            all_current_ActivatedTasks[i].SetCurrentTargetAndProgress(targetAmount, currentProgress);

            int rewardClaimValue = PlayerPrefs.GetInt(DailyTaskPlayerPrefKeys.key_TaskRewardClaimedStatus + "" + i, 0);
            if(rewardClaimValue == 1)
			{
                all_current_ActivatedTasks[i].hasClaimedTheTaskReward = true;
			}
		}

        UIManager.Instance.ui_HomeScreen.SetDailyTaskPanel(); // TEMPORARY
    }


    private void SelectRandomAchievements()
	{
        List<AchievementBase> list_TakenAchievements = all_Tasks.ToList();


        foreach (Transform child in transform) Destroy(child.gameObject);

      
        for (int i = 0; i < totalTaskToActivate; i++)
		{
          int randomIndex = Random.Range(0, list_TakenAchievements.Count);

            AchievementBase current = Instantiate(list_TakenAchievements[randomIndex], transform);
            all_current_ActivatedTasks[i] = current;
            list_TakenAchievements.RemoveAt(randomIndex);
		}

        SetAchivementTargets();
	}

    private void SetAchivementTargets()
	{
        for(int i = 0; i < all_current_ActivatedTasks.Length; i++)
		{
            all_current_ActivatedTasks[i].SetTaskCompletionTarget();
		}

        UIManager.Instance.ui_HomeScreen.SetDailyTaskPanel();
	}

    private void SetDailyTaskEndTime()
	{
        dt_DailyTaskResetTime = DateTime.Now.Add(new TimeSpan(timeInHours, timeInMinutes, timeInSeconds));
        PlayerPrefs.SetString(DailyTaskPlayerPrefKeys.key_DailyTaskEndTime, dt_DailyTaskResetTime.ToBinary().ToString());
    }

    private void SaveDailyTaskData()
	{
        // first save the chosen task index
        for(int i = 0; i < all_current_ActivatedTasks.Length; i++)
		{
            int taskIndex = all_current_ActivatedTasks[i].taskIndex;
            PlayerPrefs.SetInt(DailyTaskPlayerPrefKeys.key_ActiveTaskIndex + "" + i, taskIndex);
        }

        // Reset and save current progress, target, status for each task;
        for(int i = 0; i < all_current_ActivatedTasks.Length; i++)
		{
            int currentTarget = GetTaskTarget(i);
            PlayerPrefs.SetInt(DailyTaskPlayerPrefKeys.key_CurrentTargetForTask + "" + i, currentTarget);

            int currentProgress = GetTaskCurrentProgress(i);
            PlayerPrefs.SetInt(DailyTaskPlayerPrefKeys.key_CurrentProgressForTask + "" + i, currentProgress);

            PlayerPrefs.SetInt(DailyTaskPlayerPrefKeys.key_TaskRewardClaimedStatus + "" + i, 0);
        }

	}

    public TimeSpan GetCurrentTimeLeft()
    {
        return dt_DailyTaskResetTime - DateTime.Now;
    }

    public void ClaimRewardFromTheTask(int _index)
	{
        UpdateCurrentTaskPointsValue(all_current_ActivatedTasks[_index].achievementPoints); // update task completion points value
        all_current_ActivatedTasks[_index].hasClaimedTheTaskReward = true;
        // Get Reward
        UpdateRewardOfAchiveMent( _index);

        UpdateTaskRewardClaimStatus(_index);
	}

    private void UpdateRewardOfAchiveMent(int index) {

        DailyRewardType current = all_current_ActivatedTasks[index].rewardType;

        switch (current) {
            case DailyRewardType.Coins:
                DataManager.Instance.IncresedCoin(all_current_ActivatedTasks[index].rewardValue);
                break;
            case DailyRewardType.Gems:
                DataManager.Instance.Incresedgems(all_current_ActivatedTasks[index].rewardValue);
                break;
            case DailyRewardType.SkipIts:
                DataManager.Instance.IncresedSkipIt(all_current_ActivatedTasks[index].rewardValue);
                break;
            case DailyRewardType.Chest:
                break;
            default:
                break;
        }

        DataManager.Instance.IncreasedGameLevelPoint(all_current_ActivatedTasks[index].levelPoints);
    }

    public int GetRequiredPointsForReward()
	{
        return pointsRequiredToGetReward;
	}

    public int GetCurrentPointsReachedForReward()
	{
        return currentPointsUserHas;
	}

    public string GetTaskDetail(int _index)
	{
        return all_current_ActivatedTasks[_index].str_AchievementDescription;
	}

    public int GetTaskCurrentProgress(int _index)
	{
        return all_current_ActivatedTasks[_index].GetTaskCurrentProgress();
    }

    public int GetTaskTarget(int _index)
    {
        return all_current_ActivatedTasks[_index].GetTaskTarget();
    }

    public int GetTaskRewardValue(int _index)
    {
        return all_current_ActivatedTasks[_index].rewardValue;
    }

    public int GetTaskCompletionLevelPoints(int _index)
    {
        return all_current_ActivatedTasks[_index].levelPoints;
    }

    public int GetTaskCompletionAchievementPoints(int _index)
    {
        return all_current_ActivatedTasks[_index].achievementPoints;
    }

    public bool GetTaskCompletionStatus(int _index)
	{
        return all_current_ActivatedTasks[_index].hasCompletedTask;
	}

    public bool GetTaskRewardClaimStatus(int _index)
	{
        return all_current_ActivatedTasks[_index].hasClaimedTheTaskReward;
    }

    // SAVE AN UPDATE DATA FROM LOCAL DATABASE FUNCTIONS BELOW

    public void SaveTaskProgress(int _taskindex, int _progress)
	{
        int orderIndex = 0;
        for(int i = 0; i < all_current_ActivatedTasks.Length; i++)
		{
            if (all_current_ActivatedTasks[i].taskIndex == _taskindex)
			{
                orderIndex = i;
                break;
            }
		}

        PlayerPrefs.SetInt(DailyTaskPlayerPrefKeys.key_CurrentProgressForTask + "" + orderIndex, _progress);
	}

   
    public void UpdateCurrentTaskPointsValue(int _amount)
	{
        currentPointsUserHas += _amount;
        if (currentPointsUserHas >= pointsRequiredToGetReward)
		{
            currentPointsUserHas = currentPointsUserHas -  pointsRequiredToGetReward;
            isClaimPointTask = true;
            PlayerPrefs.SetInt(DailyTaskPlayerPrefKeys.key_IsCllaimedPintTask, isClaimPointTask ? 1 : 0);
		}

       
        PlayerPrefs.SetInt(DailyTaskPlayerPrefKeys.key_CurrentTaskPoints, currentPointsUserHas);
        UIManager.Instance.ui_HomeScreen.SetDailyTaskPanel();
    }

    public void ClaimPointTaskComplteted() {

        isClaimPointTask = false;
        int index = Random.Range(0, all_chestShopScriptableObjects.Length);
        UIManager.Instance.ui_ChestOpping.ActavetedShopBaseChest
                        (all_chestShopScriptableObjects[index], index);
        PlayerPrefs.SetInt(DailyTaskPlayerPrefKeys.key_IsCllaimedPintTask, isClaimPointTask ? 1 : 0);
        UIManager.Instance.ui_HomeScreen.SetDailyTaskPanel();
    }
    

    public void UpdateTaskRewardClaimStatus(int _index)
	{
        PlayerPrefs.SetInt(DailyTaskPlayerPrefKeys.key_TaskRewardClaimedStatus + "" + _index, 1);
    }

    public void ShowTaskBar() {
        UIManager.Instance.ui_taskProgress.ActivetedTaskProgersPanel(list_TaskShowndata);
    }

    public void AddShownList(taskShowData task) {
        list_TaskShowndata.Add(task);
    }

    public void ResetListData() {
        list_TaskShowndata.Clear();
    }

    public void skipTask(int indexOfSkip) {

        List<AchievementBase> list_Current = new List<AchievementBase>();
        for (int i = 0; i < all_Tasks.Length; i++) {

            if (!all_current_ActivatedTasks.Contains(all_Tasks[i])) {
                list_Current.Add(all_Tasks[i]);
            }
        }

        int RandomTask = Random.Range(0, list_Current.Count);
        Destroy(all_current_ActivatedTasks[indexOfSkip].gameObject);
        AchievementBase currentSpwn = Instantiate(list_Current[RandomTask], transform);
        all_current_ActivatedTasks[indexOfSkip] = currentSpwn;
        all_current_ActivatedTasks[indexOfSkip].SetTaskCompletionTarget();
        SaveDailyTaskData();
        UIManager.Instance.ui_HomeScreen.SetDailyTaskPanel();
        
    }

    public Sprite DailyGetTaskRewardSprite(int i) {

        Sprite currentSprite = null;
        switch (all_current_ActivatedTasks[i].rewardType) {
            case DailyRewardType.Coins:
                currentSprite = DataManager.Instance.sprite_Coin;
                break;
            case DailyRewardType.Gems:
                currentSprite = DataManager.Instance.sprie_Gems;
                break;
            case DailyRewardType.SkipIts:
                currentSprite = DataManager.Instance.sprite_skipIts;
                break;
            case DailyRewardType.Chest:
                break;
            default:
                break;
        }

        return currentSprite;
    }
}

[System.Serializable]
public struct taskShowData {

    public string taskName;
    public int prevousValue;
    public int UpdateValue;
    public int targetValue;
}