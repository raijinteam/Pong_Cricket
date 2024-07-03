using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Prefab")]
    [SerializeField] private BallMovment prefab_Ball;    // Ball Prefab To Spawn Ball
    [SerializeField] private PlayerData prefab_Player;   //Player Prefab to Spawn Player
    [SerializeField] private PlayerData prefab_PlayerAI;  // PlayerAI Prefab to Spawn Player
    [SerializeField] private Mini_GameManager mini_GameManager;
    [SerializeField] private RunEffect runEffect;
    public Transform mini_GameSpawner;
    
   
    //[Header("Game Data")]
 
    [SerializeField] private int CurrentRun;        // GameBats Man Run
    [SerializeField] private int ChasingRun;        // Game Cahhinsg Run
    [SerializeField] private int MaxWicket = 5;   // Total No of Wicket
    [SerializeField] private int playerLoosingwicket;
    [SerializeField] private bool isPlayerChaseGame;

  
        
    [SerializeField] private int CurrentWicket;             // Current Wicket
    [SerializeField] private int maxRun = 10;               // Max Run Value

   

    public float flt_CurrnetGameTime;     // Gameing Time
    [SerializeField] private float flt_MaxGameTime = 120;    // Max Time Game Start
    [SerializeField] private bool isCompltedFirstInning;   // Status of First Paddle Complte Inning


    [Header("Script Refrence")]
    public WallHandler wallHandler;               // Set Boundry
    





    [field: SerializeField] public Transform CurrentGameBallTransform { get; private set; }
    [field: SerializeField] public BallMovment ballMovement { get; private set; }
    [field : SerializeField]public PlayerData CurrentGamePlayer { get; private set; }
    [field : SerializeField] public PlayerData CurrentGamePlayerAI { get; private set; }
    [field : SerializeField]public bool IsGameRunning { get; private set; }
    [field: SerializeField] public bool HasPlayerWon { get; private set; }
    [field : SerializeField] public GameObject obj_GameEnvironement { get;  set; }
    [field: SerializeField] public Transform ScoreTarget { get; internal set; }

    [field: SerializeField] public bool IsGameOver { get; private set; }

    //BatsManPostion
    private Vector3 batsmanPostion;
    private Vector3 bowlerPostion;

    [SerializeField] private float flt_CameraOffetBats;
    [SerializeField] private float flt_CameraOffetBowler;



    // Powerup Data
    // --  2X POWERUP  ---
    private bool is2XActive = false;   // status of 2X PowerUp

    [Header("Gamber Powerup Data")]
    [SerializeField] private bool isGamblerPowerupActive;   // Status of Gambler Powerup
    private int flt_GamblerRunMultiplier;               //RunMultiplier    


    // GamePauseData
  
    private float flt_InningCompltedDelay = 1;
    private float flt_SummeryShowTime = 1;
   
    private float flt_PlayerChangeDelay = 1;
    private bool isGetingTimeWarning = false;




    private void Awake() {
        Instance = this;
    }

    

    private void Update() {
        TimeHandler();
    }

    public void InitiateGameProcedure() {

        IsGameOver = false;
        UIManager.Instance.panel_MainMenu.gameObject.SetActive(false);
        UIManager.Instance.ui_GameScreen.gameObject.SetActive(true);

        // Collected All Data
        isCompltedFirstInning = false;
        obj_GameEnvironement.gameObject.SetActive(true);
      
        playerLoosingwicket = 0;
        //Spawn Ball , Player , Player AI
        SpawnGameElements();
        //Set Boundry As Per Screen
       
      
        // Set player State And potion
        SetPlayerStateAndPostion();
        // Start Innings;
        IsGameRunning = false;
        flt_CurrnetGameTime = flt_MaxGameTime;
        CurrentWicket = 0;
        CurrentRun = 0;
        UIManager.Instance.ui_GameScreen.SetScore(CurrentRun, CurrentWicket);
        UIManager.Instance.ui_GameScreen.SetInningData();
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame() {
        isGetingTimeWarning = false;
        ballMovement.EnableBall();
        ballMovement.transform.localScale = Vector3.zero;
        if (CurrentGamePlayer.MyState == PlayerState.BatsMan) {
            UIManager.Instance.SpawnPopupOfPlayer(true, 2);
        }
        else {
            UIManager.Instance.SpawnPopupOfPlayer(false, 2);
        }

        yield return new WaitForSeconds(2);
        ballMovement.transform.DOScale(Vector3.one, 1);
        UIManager.Instance.ui_GameScreen.panel_Counter.startCounter();

    }



    private void SpawnGameElements() {

        // Spawn Ball
        ballMovement = Instantiate(prefab_Ball, prefab_Ball.transform.position, prefab_Ball.transform.rotation);
        CurrentGameBallTransform = ballMovement.transform;
        // Spawn Player
         CurrentGamePlayer = Instantiate(prefab_Player, prefab_Player.transform.position, prefab_Player.transform.rotation);
        CurrentGamePlayer.transform.name = DataManager.Instance.playerName;
        //Spawn PlayerAI
        CurrentGamePlayerAI = Instantiate(prefab_PlayerAI, prefab_PlayerAI.transform.position, prefab_PlayerAI.transform.rotation);
        CurrentGamePlayerAI.transform.name = DataManager.Instance.playerNameAI;
    }

    


 
    private void SetPlayerStateAndPostion() {

        CalculationAsTopAndBottamPostionAsPerScreen();

        // 50% Cahnce To Player Bat First
        int Index = Random.Range(0, 2);
        Index = 0;
        if (Index ==0) {

            isPlayerChaseGame = false;
            CurrentGamePlayer.SetPlayerState(PlayerState.BatsMan);
            CurrentGamePlayer.transform.position = batsmanPostion;
            CurrentGamePlayerAI.SetPlayerState(PlayerState.Bowler);
            CurrentGamePlayerAI.transform.position = bowlerPostion;
           
        }
        else {
            isPlayerChaseGame = true;
            CurrentGamePlayer.SetPlayerState(PlayerState.Bowler);
            CurrentGamePlayer.transform.position = bowlerPostion;
            CurrentGamePlayerAI.SetPlayerState(PlayerState.BatsMan);
            CurrentGamePlayerAI.transform.position = batsmanPostion;
           
        }

    }

    private void CalculationAsTopAndBottamPostionAsPerScreen() {
        float screenAspect = Screen.width/ Screen.height;
        float CameraHeight = Camera.main.orthographicSize * 2;
        float cameraWidth = CameraHeight * screenAspect;
        batsmanPostion = new Vector3(0, (CameraHeight / 2) - flt_CameraOffetBats, 0);
        bowlerPostion =  new Vector3(0, -(CameraHeight / 2) + flt_CameraOffetBowler, 0);
    }

    


    // Time Calculation For Game Time
    private void TimeHandler() {
       
        if (!IsGameRunning) {
            return;
        }
        flt_CurrnetGameTime -= Time.deltaTime;
        if (flt_CurrnetGameTime < 10 && !isGetingTimeWarning) {
            isGetingTimeWarning = true;
            UIManager.Instance.ActvetedWarningMessage("10 Seconds Left!");
        }
        if (flt_CurrnetGameTime <= 0) {
            CompletedInnings();
        }
    }


   
    


    private void StopGame() {
        ballMovement.transform.localScale = Vector3.zero;
        IsGameRunning = false;
        ballMovement.gameObject.SetActive(false);
        ballMovement.Resetball();
        CurrentGamePlayerAI.playerAi.shouldChasing = true;
        CurrentGamePlayerAI.playerAi.playerHitBall = false;

    }

  



    // Complete Innnig
    private void CompletedInnings() {
        if (!IsGameRunning) {
            return;
        }
        IsGameRunning = false;
        ballMovement.PlayBallDestroyerEffect();
        CurrentGamePlayer.player.enabled = false;
        CurrentGamePlayerAI.playerAi.enabled = false;
      
       

        if (!isCompltedFirstInning) {

            // Inning is Not Complted so Get Oppsotie Team Player Start New Game
            
            isCompltedFirstInning = true;
            ChasingRun = CurrentRun + 1;
            CurrentGamePlayer.PlayScaleDownAnimation();
            CurrentGamePlayerAI.PlayScaleDownAnimation();
            StartCoroutine(First_InningComplted());
            
        }
        else {

            // If Both Player And Ai Complete Innning Get Game Result
            IsGameOver = true;
            StopGame();
            CheckingGameResult();

        }

        if (CurrentGamePlayer.MyState == PlayerState.BatsMan) {
            DailyTaskManager.increasedRunWhenGameOver?.Invoke(CurrentRun);
            RewardsManager.Instance.dailyRunsRewardData.AddRunsToThisTask(CurrentRun);
            playerLoosingwicket = CurrentWicket;
        }
        else {
            DailyTaskManager.increaseedWicket?.Invoke(CurrentWicket);
        }

    }

    

    private IEnumerator First_InningComplted() {
        StopGame();
        yield return new WaitForSeconds(flt_InningCompltedDelay);

        Debug.Log("InningCompltedDelay" + flt_InningCompltedDelay);
        isShownEffect50 = false;
        isShownEffect100 = false;

        UIManager.Instance.ui_GameScreen.ShowSummeryScreen(CurrentRun, CurrentWicket);

     
     
   

      

  
    
     

    }

    public void SummeryPanelShownUser() {
        CompletedFirstInnings();

        UIManager.Instance.ui_GameScreen.obj_ShowSummryPanel.SetActive(false);

        CurrentGamePlayer.player.enabled = true;
        CurrentGamePlayerAI.playerAi.enabled = true;

        flt_CurrnetGameTime = flt_MaxGameTime;
        CurrentWicket = 0;
        CurrentRun = 0;
        UIManager.Instance.ui_GameScreen.SetScore(CurrentRun, CurrentWicket);
        UIManager.Instance.ui_GameScreen.SetInningData();
        UIManager.Instance.ui_GameScreen.setchaseRunData(ChasingRun);
        StartCoroutine(StartGame());
    }



    private void CheckingGameResult() {

    
      

        if (CurrentRun > ChasingRun) {
            // We chase Target Batman is Player So Win
            if (CurrentGamePlayer.MyState == PlayerState.BatsMan) {
              
                HasPlayerWon = true;
            }
            else {
               
                HasPlayerWon = false;
            }

        }
        else {
            // if Target Not Chase And Player Bowller So Player Win
            if (CurrentGamePlayer.MyState == PlayerState.Bowler) {
               
                HasPlayerWon = true;

            }
            else {
              
                HasPlayerWon = false;
            }
        }

        // assing taske
        DailyTaskManager.isGamewin?.Invoke(HasPlayerWon);
        if (playerLoosingwicket == 0 && HasPlayerWon) DailyTaskManager.winGameLoosingWicket?.Invoke();
        DailyTaskManager.startMatch?.Invoke();
        if (HasPlayerWon && isPlayerChaseGame) DailyTaskManager.chasematch?.Invoke();

        UIManager.Instance.UiGameOverPopUp.gameObject.SetActive(true);
    }

   

    private void CompletedFirstInnings() {

        
       // first Inning Complte        
        if (CurrentGamePlayer.MyState == PlayerState.BatsMan) {

            // Player As BatsMan So Do Bowlling
            CurrentGamePlayer.SetPlayerState(PlayerState.Bowler);
            CurrentGamePlayer.transform.position = bowlerPostion;
            CurrentGamePlayer.transform.rotation = Quaternion.identity;
        }
        else {

            // PLayer as Bowlling So Batting
            CurrentGamePlayer.SetPlayerState(PlayerState.BatsMan);
            CurrentGamePlayer.transform.position = batsmanPostion;
            CurrentGamePlayer.transform.rotation = Quaternion.identity;
          
        }

        if (CurrentGamePlayerAI.MyState == PlayerState.BatsMan) {

            // PlayerAI As BatsMan So Do Bowlling
            CurrentGamePlayerAI.SetPlayerState(PlayerState.Bowler);
            CurrentGamePlayerAI.transform.position = bowlerPostion;
            CurrentGamePlayerAI.transform.rotation = Quaternion.identity;
        }
        else {
            // PLayerAI as Bowlling So Batting
            CurrentGamePlayerAI.SetPlayerState(PlayerState.BatsMan);
            CurrentGamePlayerAI.transform.position = batsmanPostion;
            CurrentGamePlayerAI.transform.rotation = Quaternion.identity;
           
        }

        // Get RunTarget
        ChasingRun = CurrentRun;
        PowerUpManager.Instance.InningsChanged();
        
    }


    public void IncreasedWicket() {

        CurrentWicket++;
        UIManager.Instance.ui_GameScreen.SetScore(CurrentRun, CurrentWicket);
       
        if (CurrentWicket >= MaxWicket) {
            CompletedInnings();
        }
        else if (isGamblerPowerupActive) {
            CompletedInnings();
        }
        else {

            ballMovement.PlayBallDestroyerEffect();
            StopGame();
            BoardHandler.instance.ActivetedOutpanel();

        }

    }

    private bool isShownEffect100 = false;
    private bool isShownEffect50 = false;

    // Run Incresed
    public void IncreasedRun(int _myRunValue) {


        // 2X PowerupAcivted So Run Is 2*Run
        if (isGamblerPowerupActive) {
            CurrentRun += _myRunValue * 2;
        }
        else if (is2XActive) {
            CurrentRun += _myRunValue * 2;
        }
        else {
            CurrentRun += _myRunValue ;
        }


        // effect
        if (CurrentRun >= 50 && !isShownEffect50) {
            isShownEffect50 = true;
            BoardHandler.instance.Actveted50RunEffect();
        }
        if (CurrentRun >= 100 && !isShownEffect100) {
            isShownEffect100 = true;
            BoardHandler.instance.Actveted100RunEffect();
        }
       

        if (_myRunValue == maxRun && IsGameRunning) {

            if (isCompltedFirstInning && CurrentRun > ChasingRun) {
                CompletedInnings();
            }
            else {
                ballMovement.PlayBallDestroyerEffect();
                StopGame();

                BoardHandler.instance.ActvetedSmash();
            }
            
        }
        else {
            if (isCompltedFirstInning && CurrentRun > ChasingRun) {
                CompletedInnings();
            }
        }
        
        

    }

   

    public void DecreasedRun(int _myRunValue) {
        CurrentRun -= _myRunValue;
        Debug.Log(" Run" + _myRunValue);
        Debug.Log(" CurrentRun" + CurrentRun);
        if (CurrentRun <0) {
            CurrentRun = 0;
        }
        UIManager.Instance.ui_GameScreen.SetScore(CurrentRun, CurrentWicket);
    }


    // 2X Powerup Handler
    public  void Powerup2XActive() {
        is2XActive = true;
    }
    public void Powerup2XDeActived() {
        is2XActive = false;
    }

    // GameBler 

  
    public void ActivateGamblerPowerup(int _flt_RunMultiplier) {
        isGamblerPowerupActive = true;
        this.flt_GamblerRunMultiplier = _flt_RunMultiplier;
    }

    public void DeActivateGamblerPowerup() {
        isGamblerPowerupActive = false;
    }



    public void WallBlockActivated(int no_OfBlock) {
        wallHandler.ActivetedBlock(no_OfBlock);
    }

    public void WallBlockDeActivated() {
        wallHandler.DeActivetedBlock();
    }


    public void StartMinigame() {

        mini_GameSpawner.gameObject.SetActive(true);
        foreach (Transform child in mini_GameSpawner) {

            Destroy(child.gameObject);
        }

        // Mini Game Start Direct
        Instantiate(mini_GameManager, transform.position, transform.rotation , mini_GameSpawner);
    }

    public void HandlingTimeTweek(bool isplayer, float flt_TimeReduce) {
        if (isplayer) {
            if (CurrentGamePlayer.MyState == PlayerState.BatsMan) {
                flt_MaxGameTime += flt_TimeReduce;
            }
            else {
                flt_MaxGameTime -= flt_TimeReduce;
            }
        }
        else {
            if (CurrentGamePlayerAI.MyState == PlayerState.BatsMan) {
                flt_MaxGameTime += flt_TimeReduce;
            }
            else {
                flt_MaxGameTime -= flt_TimeReduce;
            }
        }
    }

    public void SpawnBallWicketTime() {
        if (IsGameOver) {
            return;
        }

        Sequence sq = DOTween.Sequence();
        ballMovement.EnableBall();
        sq.Append(ballMovement.transform.DOScale(Vector3.one, 1)).AppendCallback(GetBallVeloctyAndStartGame);
    }

    public void GetBallVeloctyAndStartGame() {
        Debug.Log("Run Start Game");
        IsGameRunning = true;
        ballMovement.SetRandomVelocityOfBall();
    }

    public  void spawnRunEffect(Sprite Runner, Vector3 position) {
        RunEffect current = Instantiate(runEffect, position, Quaternion.identity);
        current.SetTargetPostion(Runner);
    }

    public void ShowRun() {
        UIManager.Instance.ui_GameScreen.SetScore(CurrentRun, CurrentWicket);
    }
}


[System.Serializable]
public enum AbilityType {

    Powerup2X,
    Freez,
    Fielder,
    Invicible,
    SpeedShot,
    SlowMotion,
    TheWall,
    PaddleExtension,
    BallSplit,
    PinBallPaddle,
    GamblerRunner,
    Randomizer,
    Block,
    TimeTweak,
    RunMaster,
    BoundryBonus,
    spinDocter


}
