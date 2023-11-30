using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{

    public PlayerState MyState;


    [Header("Player Data")]
    [SerializeField] private float flt_MaxSwingForce;   // min Swing force
    [SerializeField] private float flt_MinSwingForce;  // max Swing force
    [SerializeField] private float flt_BallMaxForce;   // Min force Apply at ball
    [SerializeField] private float flt_BallMinForce;     // Min force Apply at ball
    private float flt_CurrentBallMaxForce;   // Min force Apply at ball
    private float flt_CurrentBallMinForce;     // Min force Apply at ball
    [SerializeField] private float flt_PaddleRoationSpeed; // paddle Movement Speed
    [SerializeField] private float flt_PaddleMovementSpeed; // Roatation Speed

    private float flt_CurrentPaddleMovementSpeed;
    private float flt_CurrentPaddleRoationSpeed;

    // InputData
    private float flt_HorizontalInput;   //Input Value

    // Clamp data
    private float flt_MinCalmpValue = -3;  //Left Clamp Value
    private float flt_MaxClampValue = 3;  // Right Clamp Value

    // force CalculationData
    private float flt_DistanceBetweenCenterToEdgeOfPaddle = 0.5f;   // Distance between centre of the paddle to edge of the paddle

   
    private void Start() {
      
        flt_CurrentPaddleMovementSpeed = flt_PaddleMovementSpeed;
        flt_CurrentPaddleRoationSpeed = flt_PaddleRoationSpeed;

        flt_CurrentBallMaxForce = flt_BallMaxForce;
        flt_CurrentBallMinForce = flt_BallMinForce;
    }

  

    private void Update() {

        if (!GameManager.Instance.IsGameRunning) {
            return;
        }

        HandlingFreezePowerUp();
        PlayerMotion(); 
    }

    

    private void PlayerMotion() {
        if (isFreezPostion) {
            return;
        }
        UserInput();
        PaddleMotion();
    }

   
    public void SetValueOfClampPosition() {

        WallHandler Current = GameManager.Instance.wallHandler;
        if (MyState == PlayerState.BatsMan) {
            flt_MinCalmpValue = Current.batsmanleft.position.x + transform.localScale.x / 2;
            flt_MaxClampValue = Current.batsmanright.position.x  - transform.localScale.x / 2;
        }
        else {
            flt_MinCalmpValue = Current.bowlerleft.position.x  + transform.localScale.x / 2;
            flt_MaxClampValue = Current.bowlerright.position.x  - transform.localScale.x / 2;
        }
        
    }


    private void PaddleMotion() {

        transform.Translate(Vector3.right * flt_HorizontalInput * flt_CurrentPaddleMovementSpeed * Time.deltaTime, Space.World);
        float x_Postion = transform.position.x;

        x_Postion = Mathf.Clamp(x_Postion, flt_MinCalmpValue, flt_MaxClampValue);

        transform.position = new Vector3(x_Postion, transform.position.y, transform.position.z);
    }

    private void UserInput() {

        flt_HorizontalInput = Input.GetAxis("Horizontal");


        if (MyState == PlayerState.BatsMan) {

            if (Input.GetKey(KeyCode.Space)) {

                RotateClockWisePaddle();
            }
        }

        //if (Input.GetMouseButton(0)) {

        //    if (Input.mousePosition.x < Screen.width / 2f) {
        //        // => left half

        //        flt_HorizontalInput = -1;
        //    }
        //    else {
        //        // => right half
        //        flt_HorizontalInput = 1;
        //    }
        //}
        //else {
        //    flt_HorizontalInput = 0;
        //}
        

    }

   
    private void RotateClockWisePaddle() {

        transform.Rotate(Vector3.forward * flt_CurrentPaddleRoationSpeed * Time.deltaTime);
    }




    public float flt_GetBallForceAsPerCollsionForce(Vector2 point) {
        float CurrntXPoint = Mathf.Abs(point.x);
        float flt_BallForce = 0;
        if (CurrntXPoint > flt_DistanceBetweenCenterToEdgeOfPaddle) {
            flt_BallForce = flt_CurrentBallMinForce;
        }
        else {
            flt_BallForce = (CurrntXPoint / flt_DistanceBetweenCenterToEdgeOfPaddle) * (flt_CurrentBallMinForce - flt_CurrentBallMaxForce) + flt_CurrentBallMaxForce;
        }
        Debug.Log("Before" + flt_BallForce);   
        
        if (IsBallSplitPowerupActive) {
            spawnBall();
        }
        


        Debug.Log("After" + flt_BallForce);
        return flt_BallForce;
    }

    public float flt_GetSwingForceAsPerCollsionForce(Vector2 point) {
        float CurrntXPoint = Mathf.Abs(point.x);
        float flt_BallForce = 0;
        if (CurrntXPoint > flt_DistanceBetweenCenterToEdgeOfPaddle) {
            flt_BallForce = flt_MaxSwingForce;
        }
        else {
            flt_BallForce = (CurrntXPoint / flt_DistanceBetweenCenterToEdgeOfPaddle) * flt_MaxSwingForce;
        }


        if (point.x < 0) {
            flt_BallForce = -flt_BallForce;
        }


        return flt_BallForce;
    }
    public void SetPlayerState(PlayerState _myState) {
        this.MyState = _myState;
        SetValueOfClampPosition();
    }



    // Power Up Handler

    [Header("Freez PowerUp")]
    [SerializeField] private bool isFreezPowerUpActive;  // Status Of Player Freez PowerUp
    [SerializeField] private bool isFreezPostion; // Status Of Freez Postion Or Not
    private float flt_FreezTime;   // Stop Movement Player Time
    private float flt_IntervalTime;  // this Intervale Player Move
    private float flt_CurrentTimeForFreezPowerUp;

    public void ActivateFreezePowerup(float _flt_FreezTime, float _flt_IntervalTime) {
        this.flt_FreezTime = _flt_FreezTime;
        this.flt_IntervalTime = _flt_IntervalTime;
        isFreezPowerUpActive = true;
        flt_CurrentTimeForFreezPowerUp = 0;
        isFreezPostion = true;
    }
    public void DeActivateFreezePowerup() {
        isFreezPowerUpActive = false;
        isFreezPostion = false;
    }
    private void HandlingFreezePowerUp() {
        if (!isFreezPowerUpActive) {
            return;
        }

        flt_CurrentTimeForFreezPowerUp += Time.deltaTime;
        if (flt_CurrentTimeForFreezPowerUp > flt_FreezTime && isFreezPostion) {
            isFreezPostion = false;
            flt_CurrentTimeForFreezPowerUp = 0;
        }
        if (flt_CurrentTimeForFreezPowerUp > flt_IntervalTime && !isFreezPostion) {
            isFreezPostion = true;
            flt_CurrentTimeForFreezPowerUp = 0;
        }

    }


 
    public void ActivetedSpeedShotPowerUp() {

        flt_CurrentBallMinForce = PowerUpManager.Instance.PowerUpSpeedShot.GetShotSpeedIncreaseValue(flt_CurrentBallMinForce);
        flt_CurrentBallMaxForce = PowerUpManager.Instance.PowerUpSpeedShot.GetShotSpeedIncreaseValue(flt_CurrentBallMaxForce);
    }

    public void DeActivetedSpeedShotPowerUp() {

        flt_CurrentBallMaxForce = flt_BallMaxForce;
        flt_CurrentBallMinForce = flt_BallMinForce;
    }


    [Header("SlowMotion PowerUp")]
    [SerializeField] private bool isSlowMotionPowerUpActive;  // Status Of Player SpeedShot PowerUp
    private float flt_PersantageOfSlowMotion;


    public void ActivateSlowMotionPowerup(float _flt_PersantageOfSlowMotion) {

        this.flt_PersantageOfSlowMotion = _flt_PersantageOfSlowMotion;
        flt_CurrentPaddleRoationSpeed -= flt_CurrentPaddleRoationSpeed * flt_PersantageOfSlowMotion * 0.01f;
        flt_CurrentPaddleMovementSpeed -= flt_CurrentPaddleMovementSpeed * flt_PersantageOfSlowMotion * 0.01f;
        isSlowMotionPowerUpActive = true;

    }
    public void DeActivateSlowMotionPowerup() {

        flt_CurrentPaddleMovementSpeed = flt_PaddleMovementSpeed;
        flt_CurrentPaddleRoationSpeed = flt_PaddleRoationSpeed;
        isSlowMotionPowerUpActive = false;
    }


    // Ball Split
    [SerializeField] private bool IsBallSplitPowerupActive;
    [SerializeField] private SmallBallMotion prefab_SmallBall;
    [SerializeField] private int NoOfBall;
   


    private void spawnBall() {
        
        for (int i = 0; i < NoOfBall; i++) {

            if (MyState == PlayerState.BatsMan) {
                SmallBallMotion current = Instantiate(prefab_SmallBall, transform.position + new Vector3(Random.Range(-5, 5), -1, 0), transform.rotation);
                current.SetRandomVelocityOfBall(new Vector3(Random.Range(-3, 3), -3, 0));
            }
            else {
                SmallBallMotion current = Instantiate(prefab_SmallBall, transform.position + new Vector3(Random.Range(-5, 5), 1, 0), transform.rotation);
                current.SetRandomVelocityOfBall(new Vector3(Random.Range(-3, 3), 3, 0));
            }

        }
    }

    public void ActivetedBallSplit(int _NoOfBall) {
        IsBallSplitPowerupActive = true;
        this.NoOfBall = _NoOfBall;
    }
    public void DeActivetedBallSplit() {
        IsBallSplitPowerupActive = false;
    }

    #region Paddle Extension

    public void ExtendPadle(float _ScaleIncrease) {

        transform.localScale +=  Vector3.one * _ScaleIncrease;
        SetValueOfClampPosition();
    }

    public void ResetScale(float _ScaleIncrease) {
        transform.localScale -= Vector3.one * _ScaleIncrease;
        SetValueOfClampPosition();
    }

    #endregion
}

public enum PlayerState {

    BatsMan,
    Bowler,
}

