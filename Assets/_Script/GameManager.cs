using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Prefab")]
    [SerializeField] private BallMovment prefab_Ball;  
    [SerializeField] private Player prefab_Player;
    [SerializeField] private PlayerAI prefab_PlayerAI;
   
    [Header("Game Data")]
    [SerializeField] private bool isGameStart;
    [SerializeField] private int CurrentRun;
    [SerializeField] private int ChasingRun;
    [SerializeField] private int MaxWicket = 5;

  

    [SerializeField] private int CurrentWicket;
    [SerializeField] private int maxRun = 10;
    [SerializeField] private float flt_CurrnetGameTime;
    [SerializeField] private float flt_MaxGameTime = 120;
    [SerializeField] private bool isCompltedFirstInning;


    [Header("Script Refrence")]
    [SerializeField] private WallHandler wallHandler;  // Set Boundry
    [SerializeField] private GameScreenUI gameScreenUI;
    [SerializeField] private GameOverUI gameOverUI;

    [Header("PowerupData")]
    private bool isPowerUp2XActive;

   

    private BallMovment ball;
    private Player currentPlayer;
    private PlayerAI currentPlayerAI;

    // Property For Current above Object
    public Transform CurrentGameBall { get {return ball.transform; } }
    public Player CurrentGamePlayer { get { return currentPlayer; } }
    public PlayerAI CurrentGamePlayerAI { get { return currentPlayerAI; } }
    public bool IsGameStart { get { return isGameStart; } }

    // BatsManPostion
    private Vector3 batsmanPostion = new Vector3(0, 8, 0);
    private Vector3 bowlerPostion = new Vector3(0, -8, 0);

   

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
        wallHandler.SetAllColliderAsPerScreen();  //Set Boundry As Per Screen
        wallHandler.SetRunnerColllider();  //Set Boundry As Per Screen
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
        batsmanPostion = new Vector3(0, (CameraHeight / 2) - 3, 0);
        bowlerPostion =  new Vector3(0, -(CameraHeight / 2) + 3, 0);
    }

    private void StartInnings() {
        isGameStart = true;
       
        
        flt_CurrnetGameTime = 0;
        CurrentWicket = 0;
        CurrentRun = 0;
        gameScreenUI.SetScore(CurrentRun, CurrentWicket);
    }


    // Time Calculation For Game Time
    private void TimeHandler() {
        if (!isGameStart) {
            return;
        }
        flt_CurrnetGameTime += Time.deltaTime;
        if (flt_CurrnetGameTime >= flt_MaxGameTime) {
            CompletedInnings();
        }
    }

   


    
   


    // New Ball Spawning
    private void SetnewBall() {
        ball.Resetball();
        currentPlayerAI.shouldChasing = true;
        currentPlayerAI.playerHitBall = false;
        ball.gameObject.SetActive(false);
        ball.SetRandomVelocityOfBall();

    }

  



    // Complete Innnig
    private void CompletedInnings() {
        isGameStart = false;

        if (!isCompltedFirstInning) {

            // Inning is Not Complted so Get Oppsotie Team Player Start New Game
            CompletedFirstInnings();
            isCompltedFirstInning = true;
        }
        else {

            // If Both Player And Ai Complete Innning Get Game Result
            CheckingGameResult();
        }

    }

    private void CheckingGameResult() {
      

        // Stop Game
        gameScreenUI.gameObject.SetActive(false);
        gameOverUI.gameObject.SetActive(true);
        ball.Resetball();
        ball.gameObject.SetActive(false);



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
        SetnewBall();
        StartInnings();
    }


    public void IncreasedWicket() {

        CurrentWicket++;
        gameScreenUI.SetScore(CurrentRun, CurrentWicket);
       
        if (CurrentWicket >= MaxWicket) {
            CompletedInnings();
        }
        else if (isAllOutPowerup) {
            CompletedInnings();
        }
        else {
            SetnewBall();
        }

    }

    // Run Incresed
    public void IncreasedRun(int _myRunValue) {


        // 2X PowerupAcivted So Run Is 2*Run
        if (isPowerUp2XActive) {
            CurrentRun += _myRunValue * 2;
        }
        else if (isRunMultiplierPowerup) {
            CurrentRun += _myRunValue * flt_RunMultyPlier;
        }
        else {
            CurrentRun += _myRunValue * 2;
        }


        if (_myRunValue == maxRun && isGameStart) {
            Debug.Log("Max Run" + maxRun);
            SetnewBall();
        }
        gameScreenUI.SetScore(CurrentRun, CurrentWicket);
        if (isCompltedFirstInning && CurrentRun > ChasingRun) {
            CompletedInnings();
        }

    }


    // 2X Powerup Handler
    public  void Powerup2XActive() {
        isPowerUp2XActive = true;
    }
    public void Powerup2XDeActived() {
        isPowerUp2XActive = false;
    }

    // GameBler 

    [Header("Gamber Data")]
    [SerializeField] private bool isRunMultiplierPowerup;
    [SerializeField] private bool isAllOutPowerup;
    private int flt_RunMultyPlier;
    public void PowerupGameblerRunMultyplierActive(int _flt_RunMultiplier) {
        isRunMultiplierPowerup = true;
        this.flt_RunMultyPlier = _flt_RunMultiplier;
    }

    public void PowerupGameblerAlloutActive() {
        isAllOutPowerup = true;
    }

    public void PowerupGameblerRunMultyplierDeActive() {
        isRunMultiplierPowerup = false;
    }

    public void PowerupGameblerAlloutDeActive() {
        isAllOutPowerup = false;
    }
}


[System.Serializable]
public enum PowerUpType {
    Powerup2X,
    Freez,
    Fielder,
    Invicible,
    FireBall,
    SpeedShot,
    SlowMotion,
    TheWall,
    PaddleExtension,
    BallSplit,
    PinBallPaddle,
    GamblerRunner,
    Randomizer,
    Block,

}
