using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public PlayerState MyState;

    [SerializeField]private bool isAi;

    [Header("Player Data")]
    [SerializeField] private float flt_MaxSwingForce;   // min Swing force
    [SerializeField] private float flt_MinSwingForce;  // max Swing force
    [SerializeField] private float flt_BallMaxForce;   // Min force Apply at ball
    [SerializeField] private float flt_BallMinForce;     // Min force Apply at ball
    [SerializeField] private float flt_PaddleRoationSpeed; // paddle Movement Speed
    [SerializeField] private float flt_PaddleMovementSpeed; // Roatation Speed

    // InputData
    private float flt_HorizontalInput;   //Input Value

    // Clamp data
    private float flt_MinCalmpValue = -3;  //Left Clamp Value
    private float flt_MaxClampValue = 3;  // Right Clamp Value

    // force CalculationData
    private float flt_MaxPointToTouchBall = 0.5f;   




   

    private void Update() {

        if (!GameManager.Instance.IsGameStart) {
            return;
        }

        UserInput();
        PaddleMotion();
    }

    private void PaddleMotion() {

        transform.Translate(Vector3.right * flt_HorizontalInput * flt_PaddleMovementSpeed * Time.deltaTime,Space.World);
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
            if (Input.GetKey(KeyCode.Backspace)) {
                RotateCounterClockWisePaddle();
            }
        }

      
    }

   

    private void RotateCounterClockWisePaddle() {

        transform.Rotate(Vector3.forward * flt_PaddleRoationSpeed * Time.deltaTime);
    }
    private void RotateClockWisePaddle() {

        transform.Rotate(Vector3.back * flt_PaddleRoationSpeed * Time.deltaTime);
    }


    public float flt_GetBallForceAsPerCollsionForce(Vector2 point) {
        float CurrntXPoint = Mathf.Abs(point.x);
        float flt_BallForce = 0;
        if (CurrntXPoint > flt_MaxPointToTouchBall) {
            flt_BallForce = flt_BallMinForce;
        }
        else {
            flt_BallForce = (CurrntXPoint / flt_MaxPointToTouchBall) * (flt_BallMinForce - flt_BallMaxForce) + flt_BallMaxForce;
        }

        return flt_BallForce;
    }

    public float flt_GetSwingForceAsPerCollsionForce(Vector2 point) {
        float CurrntXPoint = Mathf.Abs(point.x);
        float flt_BallForce = 0;
        if (CurrntXPoint > flt_MaxPointToTouchBall) {
            flt_BallForce = flt_MinSwingForce;
        }
        else {
            flt_BallForce = (CurrntXPoint / flt_MaxPointToTouchBall) * (flt_MinSwingForce - flt_MaxSwingForce) + flt_MaxSwingForce;
        }

        if (point.x < 0) {
            flt_BallForce = -flt_BallForce;
        }

        return flt_BallForce;
    }
    public void SetPlayerState(PlayerState _myState) {
        this.MyState = _myState;
    }


} 

public enum PlayerState {

    BatsMan,
    Bowler,
}

