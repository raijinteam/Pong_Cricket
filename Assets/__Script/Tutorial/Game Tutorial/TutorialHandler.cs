using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TutorialHandler : MonoBehaviour {


    public static TutorialHandler instance;

  
    [SerializeField] private Tutorial_PopUpMessage tutorial_PopUpMessage;
    [SerializeField] private tutorial_Player player;
    [SerializeField] private Tutorial_PlayerAI playerAI;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Collder_Runner[] all_RunnerCollider;
    [SerializeField] private GameObject wicket;
    [SerializeField] private TutorialBall tutorial_Ball;


    [SerializeField] private int middleColliderRun;
    [SerializeField] private int PlayermaxTouch;
    [SerializeField] private int CurrentTouch = 0;
    [field: SerializeField] public Tutorial_State CurrentTutorialState { get; private set; }
    [field: SerializeField] public TutorialBall CurrentBall { get; private set; }
    [SerializeField] int CurrentRun;
    private int maxRun = 1;




  
    
    [field: SerializeField] public GameObject panel_MainMenu { get; set; }
    [field : SerializeField] public Tutorial_UI ui_Tutorial { get; set; }
    [field : SerializeField] public  Tutorial_HomeScreen ui_HomeScreen { get; private set; }
   
    [field: SerializeField] public Tutorial_SloteTime ui_SloteTime { get; private set; }
   



   
    
   

  

   

   


    public Action<Tutorial_State> tutorialChange;
    

    private void Awake() {
        instance = this;
    }

   
    private void Start() {


        panel_MainMenu.SetActive(false);
        ui_Tutorial.gameObject.SetActive(true);

        SetBgAspreScreen();
        player.SetClampPostion();
        wicket.gameObject.SetActive(false);
        DisableAllRunnerCollider();
        ChangeTutorial(Tutorial_State.learnHorizonatlMovement);

    }

   

   

    public void SpawnBall() {

        if (CurrentTutorialState == Tutorial_State.LearnBowling) {
            CurrentBall = Instantiate(tutorial_Ball, new Vector3(Random.Range(-3,3), 0, 0), transform.rotation, transform);
        }
        else if (CurrentTutorialState == Tutorial_State.LearnScoreingSytem) {
            CurrentBall = Instantiate(tutorial_Ball, new Vector3(Random.Range(-3, 3), 0, 0), transform.rotation, transform);
        }
        else {
            CurrentBall = Instantiate(tutorial_Ball, new Vector3(Random.Range(-3, 3), -13, 0), transform.rotation, transform);
        }
       
        CurrentBall.setRandomDirection();

    }

    private void SetBgAspreScreen() {

        float worldScreenHeight = Camera.main.orthographicSize * 2;

        // world width is calculated by diving world height with screen heigh
        // then multiplying it with screen width
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        // to scale the game object we divide the world screen width with the
        // size x of the sprite, and we divide the world screen height with the
        // size y of the sprite
        sr.transform.localScale = new Vector3(
            worldScreenWidth / sr.sprite.bounds.size.x,
            worldScreenHeight / sr.sprite.bounds.size.y, 1);
    }

    private void DisableAllRunnerCollider() {
        for (int i = 0; i < all_RunnerCollider.Length; i++) {
            all_RunnerCollider[i].gameObject.SetActive(false);
           
        }
    }

    public void ChangeTutorial(Tutorial_State _TutorailState) {

        CurrentTutorialState = _TutorailState;
        
        tutorialChange?.Invoke(_TutorailState);
        switch (_TutorailState) {
            case Tutorial_State.learnHorizonatlMovement:
                SpawnHorizonatlMotionMesseage();
                break;
            case Tutorial_State.learnRotationMotion:
                SpawnRotationMotionMesseage();
                break;
            case Tutorial_State.learnMiddleofRun:
                SpawnMiddleBallMaxForceMesseage();
                break;
            case Tutorial_State.LearnScoreingSytem:
                SpawnScoringExplainedMesseage();
                break;
            case Tutorial_State.LearnBowling:
                SpawnBowlingExplainedMesseage();
                break;
            default:
                break;
        }
       
       
    }

   

    private void SpawnHorizonatlMotionMesseage() {
      
        Tutorial_PopUpMessage current = Instantiate(tutorial_PopUpMessage, transform.position, transform.rotation, ui_Tutorial.transform);
        current.SetPopupMessgae("Swipe Screen And Paddle Move",false);
    }

    public void PlayerHorizontalHitTouch() {

        CurrentTouch++;
        Destroy(CurrentBall.gameObject);
        if (CurrentTouch >= PlayermaxTouch) {
            CurrentTouch = 0;
            ChangeTutorial(Tutorial_State.learnRotationMotion);
        }
        else {
            CurrentBall = Instantiate(tutorial_Ball, new Vector3(Random.Range(-3, 3), -13, 0), transform.rotation, transform);
            CurrentBall.setRandomDirection();
        }
    }

    private void SpawnRotationMotionMesseage() {

        Tutorial_PopUpMessage current = Instantiate(tutorial_PopUpMessage, transform.position, transform.rotation, ui_Tutorial.transform);
        current.SetPopupMessgae("Clilck btn Paddle Rotate",false);
        CurrentTouch = 0;
    }

    public void PlayerHitRotation() {
        CurrentTouch++;
        Destroy(CurrentBall.gameObject);
        if (CurrentTouch >= PlayermaxTouch) {
            CurrentTouch = 0;
            ChangeTutorial(Tutorial_State.learnMiddleofRun);
        }
        else {
            CurrentBall = Instantiate(tutorial_Ball, new Vector3(Random.Range(-3, 3), -13, 0), transform.rotation, transform);
            CurrentBall.setRandomDirection();
        }
    }

    private void SpawnMiddleBallMaxForceMesseage() {

        Tutorial_PopUpMessage current = Instantiate(tutorial_PopUpMessage, transform.position, transform.rotation, ui_Tutorial.transform);
        current.SetPopupMessgae("middle Panel Touch", false);
        CurrentTouch = 0;

    }

    public void MiddleHitBall() {
        CurrentTouch++;
        Destroy(CurrentBall.gameObject);
        if (CurrentTouch >= PlayermaxTouch) {
            CurrentTouch = 0;
            ChangeTutorial(Tutorial_State.LearnBowling);
        }
        else {
            CurrentBall = Instantiate(tutorial_Ball, new Vector3(Random.Range(-3, 3), -13, 0), transform.rotation, transform);
            CurrentBall.setRandomDirection();
        }

    }


    private void SpawnBowlingExplainedMesseage() {

        CurrentTouch = 0;
        wicket.gameObject.SetActive(false);
        DisableAllRunnerCollider();
        player.SetBollwerPostion();
        playerAI.gameObject.SetActive(true);
        playerAI.SetClampPostion();
        Tutorial_PopUpMessage current = Instantiate(tutorial_PopUpMessage, transform.position, transform.rotation, ui_Tutorial.transform);
        current.SetPopupMessgae("Learn Ball", false);
    }

    public void PlayerBowlingTouch() {
        CurrentTouch++;
       
        if (CurrentTouch >= PlayermaxTouch) {
            CurrentTouch = 0;
            ChangeTutorial(Tutorial_State.LearnScoreingSytem);
            Destroy(CurrentBall.gameObject);
        }
      


    }



    private void SpawnScoringExplainedMesseage() {

        Tutorial_PopUpMessage current = Instantiate(tutorial_PopUpMessage, transform.position, transform.rotation, ui_Tutorial.transform);
        current.SetPopupMessgae("Score Shown",false);

        CurrentRun = 0;

        for (int i = 0; i < all_RunnerCollider.Length; i++) {

            all_RunnerCollider[i].gameObject.SetActive(true);

        }
        playerAI.SetBollwerPostion();
        player.SetClampPostion();
        wicket.SetActive(true);

    }

    public void IncreasedRun(int Run) {
        this.CurrentRun += Run;
        if (CurrentRun >= maxRun) {

            
            Destroy(CurrentBall.gameObject);
            Debug.Log("Start Game");
            Tutorial_PopUpMessage current = Instantiate(tutorial_PopUpMessage, transform.position, transform.rotation, ui_Tutorial.transform);
            current.SetPopupMessgae("Satrt Game", true);
           
           
        }
    }

  

  
   

   


    public void ClickToTapToConitue() {

        switch (CurrentTutorialState) {

            case Tutorial_State.learnHorizonatlMovement:
                CurrentBall = Instantiate(tutorial_Ball, new Vector3(Random.Range(-3, 3), -13, 0), transform.rotation, transform);
                CurrentBall.setRandomDirection();
                break;
            case Tutorial_State.learnRotationMotion:
                CurrentBall = Instantiate(tutorial_Ball, new Vector3(Random.Range(-3, 3), -13, 0), transform.rotation, transform);
                CurrentBall.setRandomDirection();
                break;
            case Tutorial_State.learnMiddleofRun:
                CurrentBall = Instantiate(tutorial_Ball, new Vector3(Random.Range(-3, 3), -13, 0), transform.rotation, transform);
                CurrentBall.setRandomDirection();
                break;
            case Tutorial_State.LearnScoreingSytem:
                CurrentBall = Instantiate(tutorial_Ball, new Vector3(Random.Range(-3, 3), 0, 0), transform.rotation, transform);
                CurrentBall.setRandomDirection();
                break;
            case Tutorial_State.LearnBowling:
                CurrentBall = Instantiate(tutorial_Ball, new Vector3(Random.Range(-3, 3), 0, 0), Quaternion.identity,transform);
                CurrentBall.setRandomDirection();
                break;
            default:
                break;
        }
    }


    public void Startgame() {

        player.gameObject.SetActive(false);
        playerAI.gameObject.SetActive(false);
        sr.gameObject.SetActive(false);
        panel_MainMenu.SetActive(true);
        ui_Tutorial.gameObject.SetActive(false);
        UIManager.Instance.panel_MainMenu.SetActive(true);
        DataManager.Instance.ShownGameTutorial();
        Destroy(this.gameObject);

    }
    public void CompletedTutorial() {
        Destroy(this.gameObject);
        UIManager.Instance.panel_MainMenu.SetActive(true);
    }


}

public enum Tutorial_State {

    learnHorizonatlMovement,
    learnRotationMotion,
    learnMiddleofRun,
    LearnScoreingSytem,
    LearnBowling
}
