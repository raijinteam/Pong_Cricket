using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour {

   

    [SerializeField] private SpriteRenderer spriteRenderer;


    [Header("Player Data")]
   
    [SerializeField] private float flt_PaddleRoationSpeed; // paddle Movement Speed
    [SerializeField] private float flt_PaddleMovementSpeed; // Roatation Speed

    private float flt_CurrentPaddleMovementSpeed;
    private float flt_CurrentPaddleRoationSpeed;

    // InputData
    private float flt_HorizontalInput;   //Input Value
    private bool isRotate;

    float flt_TargetPostion;
    float flt_CurrentPostion;
    private Vector2 startTouchPosition;
    private Vector2 moveDirection;
    private float flt_Delta = 4;
    private float flt_SenstyVity = 50;

    // Clamp data
    private float flt_MinCalmpValue = -3;  //Left Clamp Value
    private float flt_MaxClampValue = 3;  // Right Clamp Value


    private PlayerData playerdata;


  

    private void Awake() {
        playerdata = GetComponent<PlayerData>();
    }


    private void Start() {
        flt_CurrentPaddleMovementSpeed = flt_PaddleMovementSpeed;
        flt_CurrentPaddleRoationSpeed = flt_PaddleRoationSpeed;
    }



    public void SetValueOfClampPosition() {


        WallHandler Current = GameManager.Instance.wallHandler;
        if (playerdata.MyState == PlayerState.BatsMan) {
            flt_MinCalmpValue = Current.batsmanleft.position.x + transform.localScale.x / 2;
            flt_MaxClampValue = Current.batsmanright.position.x - transform.localScale.x / 2;
            Debug.Log($"{transform.name} is BatsMan PostionSet");
        }
        else {
            flt_MinCalmpValue = Current.bowlerleft.position.x + transform.localScale.x / 2;
            flt_MaxClampValue = Current.bowlerright.position.x - transform.localScale.x / 2;
            Debug.Log($"{transform.name} is Bawller PostionSet");
        }
        isRotate = false;
        transform.rotation = Quaternion.identity;
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
       
        PaddleMotion();
    }


   


    private void PaddleMotion() {

#if UNITY_EDITOR
        flt_HorizontalInput = Input.GetAxis("Horizontal");


        if (playerdata.MyState == PlayerState.BatsMan) {

            if (Input.GetKey(KeyCode.Space)) {

                isRotate = true;
            }
            else {
                isRotate = false;
            }
        }

        transform.Translate(Vector3.right * flt_HorizontalInput * flt_CurrentPaddleMovementSpeed * Time.deltaTime, Space.World);
        float x_Postion = transform.position.x;

        x_Postion = Mathf.Clamp(x_Postion, flt_MinCalmpValue, flt_MaxClampValue);

        transform.position = new Vector3(x_Postion, transform.position.y, transform.position.z);

#else
        SwipeControl();
#endif

        RotateClockWisePaddle();

       
    }

   
    private void SwipeControl() {

        if (Input.GetMouseButtonDown(0)) {
            startTouchPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        }
        else if (Input.GetMouseButton(0)) {

            Vector2 currentSwipe = new Vector2(Input.mousePosition.x - startTouchPosition.x, 0).normalized;
            float flt_Distance = Mathf.Abs(Vector2.Distance(startTouchPosition, Input.mousePosition));
            startTouchPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            if (flt_Distance > flt_Delta) {
                startTouchPosition = Input.mousePosition;
                moveDirection = currentSwipe * flt_SenstyVity;

                flt_TargetPostion = moveDirection.x;
                flt_TargetPostion = Mathf.Clamp(flt_TargetPostion, flt_MinCalmpValue, flt_MaxClampValue);
                flt_CurrentPostion = transform.position.x;
                flt_CurrentPostion = Mathf.Lerp(flt_CurrentPostion, flt_TargetPostion, Time.deltaTime * flt_CurrentPaddleMovementSpeed);
                transform.position = new Vector2(flt_CurrentPostion, transform.position.y);

            }

        }


    }


    public void BtnClick(bool value) {
        isRotate = value;
    }

    private void RotateClockWisePaddle() {

        if (isRotate) {
            transform.Rotate(Vector3.forward * flt_CurrentPaddleRoationSpeed * Time.deltaTime);
        }
        
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

