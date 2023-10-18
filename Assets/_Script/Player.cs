using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public PlayerState MyState;


    [Header("Player Data")]
    [SerializeField] private float flt_MaxSwingForce;   // min Swing force
    [SerializeField] private float flt_MinSwingForce;  // max Swing force
    [SerializeField] private float flt_BallMaxForce;   // Min force Apply at ball
    [SerializeField] private float flt_BallMinForce;     // Min force Apply at ball
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
        SetValueOfClampPosition();
        flt_CurrentPaddleMovementSpeed = flt_PaddleMovementSpeed;
        flt_CurrentPaddleRoationSpeed = flt_PaddleRoationSpeed;
        
    }



    private void Update() {

        if (!GameManager.Instance.IsGameStart) {
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

   
    private void SetValueOfClampPosition() {
        float screenAspect = (float)Screen.width / Screen.height;
        float cameraHeight = Camera.main.orthographicSize * 2;
        float cameraWidth = cameraHeight * screenAspect;
        flt_MinCalmpValue = (-cameraWidth / 2) + transform.localScale.x / 2;
        flt_MaxClampValue = (cameraWidth / 2) - transform.localScale.x / 2;
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

      
    }

   
    private void RotateClockWisePaddle() {

        transform.Rotate(Vector3.forward * flt_CurrentPaddleRoationSpeed * Time.deltaTime);
    }




    public float flt_GetBallForceAsPerCollsionForce(Vector2 point) {
        float CurrntXPoint = Mathf.Abs(point.x);
        float flt_BallForce = 0;
        if (CurrntXPoint > flt_DistanceBetweenCenterToEdgeOfPaddle) {
            flt_BallForce = flt_BallMinForce;
        }
        else {
            flt_BallForce = (CurrntXPoint / flt_DistanceBetweenCenterToEdgeOfPaddle) * (flt_BallMinForce - flt_BallMaxForce) + flt_BallMaxForce;
        }
        Debug.Log("Before" + flt_BallForce);


        if (isSpeedShotPowerUpActive) {
            flt_BallForce = PowerUpManager.Instance.PowerUpSpeedShot.GetShotSpeedIncreaseValue(flt_BallForce);
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


    [Header("SpeedShot PowerUp")]
    [SerializeField] private bool isSpeedShotPowerUpActive;  // Status Of Player SpeedShot PowerUp
    public void ActivetedSpeedShotPowerUp() {
        isSpeedShotPowerUpActive = true;
    }

    public void DeActivetedSpeedShotPowerUp() {
        isSpeedShotPowerUpActive = false;
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

}

public enum PlayerState {

    BatsMan,
    Bowler,
}

