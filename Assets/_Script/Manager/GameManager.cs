using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Prefab")]
    [SerializeField] private BallMovment prefab_Ball;    // Ball Prefab To Spawn Ball
    [SerializeField] private Player prefab_Player;   //Player Prefab to Spawn Player
    [SerializeField] private PlayerAI prefab_PlayerAI;  // PlayerAI Prefab to Spawn Player
   
    [Header("Game Data")]
    [SerializeField] private bool isGameRunning;   // State  Game Is Runnig Or Not
    [SerializeField] private int CurrentRun;        // GameBats Man Run
    [SerializeField] private int ChasingRun;        // Game Cahhinsg Run
    [SerializeField] private int MaxWicket = 5;   // Total No of Wicket

  

    [SerializeField] private int CurrentWicket;             // Current Wicket
    [SerializeField] private int maxRun = 10;               // Max Run Value
    [SerializeField] private float flt_CurrnetGameTime;     // Gameing Time
    [SerializeField] private float flt_MaxGameTime = 120;    // Max Time Game Start
    [SerializeField] private bool isCompltedFirstInning;   // Status of First Paddle Complte Inning


    [Header("Script Refrence")]
    public WallHandler wallHandler;               // Set Boundry
    [SerializeField] private GameScreenUI gameScreenUI;             // Ui Handler
    [SerializeField] private GameOverUI gameOverUI;                 // GameOver Screen


    private BallMovment ball;
    private Player currentPlayer;
    private PlayerAI currentPlayerAI;

    // Property For Current above Object
    public Transform CurrentGameBall { get {return ball.transform; } }
    public BallMovment ballMovement { get {return ball; } }



    public Player CurrentGamePlayer { get { return currentPlayer; } }
    public PlayerAI CurrentGamePlayerAI { get { return currentPlayerAI; } }
    public bool IsGameRunning { get { return isGameRunning; } }

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
    private float flt_WicketDelayTime = 1;
    private float flt_MaxRunDelayTime = 1;
    private float flt_InningCompltedDelay = 1;
    private float flt_SummeryShowTime = 1;
    private float flt_SummeryAfterShowDelay = 1;
    private float flt_PlayerChangeDelay = 1;
   




    private void Awake() {
        Instance = this;
    }

    private void Start() {
        StartGame();
    }

    private void Update() {
        TimeHandler();
    }

    public void StartGame() {
        // Collected All Data
       
        isCompltedFirstInning = false;
        //Spawn Ball , Player , Player AI
        SpawnGameElements();
        //Set Boundry As Per Screen
        wallHandler.SetBg();
      
        // Set player State And potion
        SetPlayerStateAndPostion();
        // Start Innings;
        StartInnings();
        ball.SetRandomVelocityOfBall();
    }

   

    private void SpawnGameElements() {

        // Spawn Ball
        ball = Instantiate(prefab_Ball, prefab_Ball.transform.position, prefab_Ball.transform.rotation);
        // Spawn Player
        currentPlayer = Instantiate(prefab_Player, prefab_Player.transform.position, prefab_Player.transform.rotation);
        //Spawn PlayerAI
        currentPlayerAI = Instantiate(prefab_PlayerAI, prefab_PlayerAI.transform.position, prefab_PlayerAI.transform.rotation);
    }

    


 
    private void SetPlayerStateAndPostion() {

        CalculationAsTopAndBottamPostionAsPerScreen();

        // 50% Cahnce To Player Bat First
        int Index = Random.Range(0, 2);
        if (Index ==0) {

            currentPlayer.SetPlayerState(PlayerState.BatsMan);
            currentPlayer.transform.position = batsmanPostion;
            currentPlayerAI.SetPlayerState(PlayerState.Bowler);
            currentPlayerAI.transform.position = bowlerPostion;
            gameScreenUI.SetPlayerName("Player");
        }
        else {
            currentPlayer.SetPlayerState(PlayerState.Bowler);
            currentPlayer.transform.position = bowlerPostion;
            currentPlayerAI.SetPlayerState(PlayerState.BatsMan);
            currentPlayerAI.transform.position = batsmanPostion;
            gameScreenUI.SetPlayerName("PlayerAI");
        }

    }

    private void CalculationAsTopAndBottamPostionAsPerScreen() {
        float screenAspect = Screen.width/ Screen.height;
        float CameraHeight = Camera.main.orthographicSize * 2;
        float cameraWidth = CameraHeight * screenAspect;
        batsmanPostion = new Vector3(0, (CameraHeight / 2) - flt_CameraOffetBats, 0);
        bowlerPostion =  new Vector3(0, -(CameraHeight / 2) + flt_CameraOffetBowler, 0);
    }

    private void StartInnings() {
        isGameRunning = true;
       
        
        flt_CurrnetGameTime = 0;
        CurrentWicket = 0;
        CurrentRun = 0;
        gameScreenUI.SetScore(CurrentRun, CurrentWicket);
    }


    // Time Calculation For Game Time
    private void TimeHandler() {
       
        if (!isGameRunning) {
            return;
        }
        flt_CurrnetGameTime += Time.deltaTime;
        if (flt_CurrnetGameTime >= flt_MaxGameTime) {
            CompletedInnings();
        }
    }


    // New Ball Spawning
    private void SetnewBall() {
        isGameRunning = true;
        ball.SetRandomVelocityOfBall();

    }

    


    private void StopGame() {
        
        isGameRunning = false;
        ball.Resetball();
        currentPlayerAI.shouldChasing = true;
        currentPlayerAI.playerHitBall = false;
        ball.gameObject.SetActive(false);
    }

  



    // Complete Innnig
    private void CompletedInnings() {
       

        if (!isCompltedFirstInning) {

            // Inning is Not Complted so Get Oppsotie Team Player Start New Game
            
            isCompltedFirstInning = true;
            StartCoroutine(First_InningComplted());
        }
        else {

            // If Both Player And Ai Complete Innning Get Game Result
            StartCoroutine(ChekingGameComplted());
           
        }

    }

    private IEnumerator ChekingGameComplted() {
        StopGame();
        yield return new WaitForSeconds(flt_InningCompltedDelay);
        CheckingGameResult();
    }

    private IEnumerator First_InningComplted() {
        StopGame();
        yield return new WaitForSeconds(flt_InningCompltedDelay);

        Debug.Log("InningCompltedDelay" + flt_InningCompltedDelay);
        if (currentPlayer.MyState == PlayerState.BatsMan) {
            gameScreenUI.ShowSummeryScreen(flt_SummeryShowTime, currentPlayer.name,currentPlayerAI.name, CurrentRun);
        }
        else {
            gameScreenUI.ShowSummeryScreen(flt_SummeryShowTime, currentPlayerAI.name, currentPlayer.name, CurrentRun);
        }
       
        yield return new WaitForSeconds(flt_SummeryShowTime);
        Debug.Log("SummeryShowTime" + flt_SummeryShowTime);
        CompletedFirstInnings();
        yield return new WaitForSeconds(flt_PlayerChangeDelay);
        Debug.Log("flt_PlayerChangeDelay" + flt_PlayerChangeDelay);
        SetnewBall();
        StartInnings();



    }

    private void CheckingGameResult() {
      

        // Stop Game
        gameScreenUI.gameObject.SetActive(false);
        gameOverUI.gameObject.SetActive(true);
       



        if (CurrentRun > ChasingRun) {
            // We chase Target Batman is Player So Win
            if (currentPlayer.MyState == PlayerState.BatsMan) {
                gameOverUI.SetResult("You Win Inning");
            }
            else {
                gameOverUI.SetResult("You Lose Inning");
            }

        }
        else {
            // if Target Not Chase And Player Bowller So Player Win
            if (currentPlayer.MyState == PlayerState.Bowler) {
                gameOverUI.SetResult("You Win Inning");
            }
            else {
                gameOverUI.SetResult("You Lose Inning");
            }
        }
        
    }

    private void CompletedFirstInnings() {

        
       // first Inning Complte        
        if (currentPlayer.MyState == PlayerState.BatsMan) {

            // Player As BatsMan So Do Bowlling
            currentPlayer.SetPlayerState(PlayerState.Bowler);
            currentPlayer.transform.position = bowlerPostion;
            currentPlayer.transform.rotation = Quaternion.identity;
        }
        else {

            // PLayer as Bowlling So Batting
            currentPlayer.SetPlayerState(PlayerState.BatsMan);
            currentPlayer.transform.position = batsmanPostion;
            currentPlayer.transform.rotation = Quaternion.identity;
            gameScreenUI.SetPlayerName("Player");
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
            gameScreenUI.SetPlayerName("PlayerAI");
        }

        // Get RunTarget
        ChasingRun = CurrentRun;
        PowerUpManager.Instance.InningsChanged();
        
    }


    public void IncreasedWicket() {

        CurrentWicket++;
        gameScreenUI.SetScore(CurrentRun, CurrentWicket);
       
        if (CurrentWicket >= MaxWicket) {
            CompletedInnings();
        }
        else if (isGamblerPowerupActive) {
            CompletedInnings();
        }
        else {

            StartCoroutine(Delay_WicketCorotine());
            
        }

    }

    private IEnumerator Delay_WicketCorotine() {
        StopGame();
        yield return new WaitForSeconds(flt_WicketDelayTime);
        SetnewBall();
    }

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
        gameScreenUI.SetScore(CurrentRun, CurrentWicket);

        if (_myRunValue == maxRun && isGameRunning) {

            if (isCompltedFirstInning && CurrentRun > ChasingRun) {
                CompletedInnings();
            }
            else {
                StartCoroutine(Delay_OfMaxRunCorotine());
            }
            
        }
        else {
            if (isCompltedFirstInning && CurrentRun > ChasingRun) {
                CompletedInnings();
            }
        }
        
        

    }

    private IEnumerator Delay_OfMaxRunCorotine() {

        StopGame();
        Debug.Log("DelayofMaxRun" + flt_MaxRunDelayTime);
        yield return new WaitForSeconds(flt_MaxRunDelayTime);
        SetnewBall();
       
       
    }

    public void DecreasedRun(int _myRunValue) {
        CurrentRun -= _myRunValue;
        Debug.Log(" Run" + _myRunValue);
        Debug.Log(" CurrentRun" + CurrentRun);
        if (CurrentRun <0) {
            CurrentRun = 0;
        }
        gameScreenUI.SetScore(CurrentRun, CurrentWicket);
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
}


[System.Serializable]
public enum PowerUpType {

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
    StickyShot,

}
