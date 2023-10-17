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
    [SerializeField] private float flt_CurrnetGameTime;
    [SerializeField] private float flt_MaxGameTime = 120;
    [SerializeField] private bool isCompltedFirstInning;


    [Header("Script Refrence")]
    [SerializeField] private WallHandler wallHandler;  // Set Boundry
    [SerializeField] private GameScreenUI gameScreenUI;
    [SerializeField] private GameOverUI gameOverUI;

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
        SpawnGameEnvirontment();
        //Set Boundry As Per Screen
        wallHandler.SetAllColliderAsPerScreen();
        wallHandler.SetRunnerColllider();
        // Set player State And potion
        SetPlayerStateAndPostion();
        // GameStartProcdure;
        GameStartProcdure();


    }

   

    private void SpawnGameEnvirontment() {

        // Spawn Ball
        ball = Instantiate(prefab_Ball, prefab_Ball.transform.position, prefab_Ball.transform.rotation);
        // Spawn Player
        currentPlayer = Instantiate(prefab_Player, prefab_Player.transform.position, prefab_Player.transform.rotation);
        //Spawn PlayerAI
        currentPlayerAI = Instantiate(prefab_PlayerAI, prefab_PlayerAI.transform.position, prefab_PlayerAI.transform.rotation);
    }

    


  
    private void SetPlayerStateAndPostion() {

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

    private void GameStartProcdure() {
        isGameStart = true;
       
        ball.SetRandomVelocityOfBall();
        flt_CurrnetGameTime = 0;
        CurrentWicket = 0;
        CurrentRun = 0;
        gameScreenUI.SetScore(CurrentRun, CurrentWicket);
    }

    private void TimeHandler() {
        if (!isGameStart) {
            return;
        }
        flt_CurrnetGameTime += Time.deltaTime;
        if (flt_CurrnetGameTime > flt_MaxGameTime) {
            CompletedInnings();
        }
    }

   

    public void IncreasedWicket() {

        CurrentWicket++;
        gameScreenUI.SetScore(CurrentRun, CurrentWicket);
        SetnewBall();
        if (CurrentWicket >= MaxWicket) {
            CompletedInnings();
        }
    }

    private void SetnewBall() {
        ball.gameObject.SetActive(false);
        Destroy(ball.gameObject);
        ball = Instantiate(prefab_Ball, prefab_Ball.transform.position, prefab_Ball.transform.rotation);
        ball.SetRandomVelocityOfBall();

    }

    

    public void IncreasedRun(int _myRunValue) {
        CurrentRun += _myRunValue;
        
        if (_myRunValue == 6 && isGameStart) {
            Debug.Log("Sixer And New Ball Spawn");
            SetnewBall();
        }
        gameScreenUI.SetScore(CurrentRun, CurrentWicket);
        if (isCompltedFirstInning && CurrentRun > ChasingRun) {
            CompletedInnings();
        }
        
    }

    private void CompletedInnings() {
        isGameStart = false;
        if (!isCompltedFirstInning) {
            CompletedFirstInnings();
            isCompltedFirstInning = true;
        }
        else {
            CheckingGameResult();
        }

    }

    private void CheckingGameResult() {
      
        gameScreenUI.gameObject.SetActive(false);
        gameOverUI.gameObject.SetActive(true);
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
       
        if (currentPlayer.MyState == PlayerState.BatsMan) {
            currentPlayer.SetPlayerState(PlayerState.Bowler);
            currentPlayer.transform.position = bowlerPostion;
            currentPlayer.transform.rotation = Quaternion.identity;
        }
        else {
            currentPlayer.SetPlayerState(PlayerState.BatsMan);
            currentPlayer.transform.position = batsmanPostion;
            currentPlayer.transform.rotation = Quaternion.identity;
            gameScreenUI.SetPlayerName("Player");
        }

        if (CurrentGamePlayerAI.MyState == PlayerState.BatsMan) {
            CurrentGamePlayerAI.SetPlayerState(PlayerState.Bowler);
            CurrentGamePlayerAI.transform.position = bowlerPostion;
            CurrentGamePlayerAI.transform.rotation = Quaternion.identity;
        }
        else {
            CurrentGamePlayerAI.SetPlayerState(PlayerState.BatsMan);
            CurrentGamePlayerAI.transform.position = batsmanPostion;
            CurrentGamePlayerAI.transform.rotation = Quaternion.identity;
            gameScreenUI.SetPlayerName("PlayerAI");
        }
        ChasingRun = CurrentRun;
        SetnewBall();
        GameStartProcdure();
    }
}
