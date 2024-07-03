using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class tutorial_Player : MonoBehaviour {
    

    [SerializeField] private Transform left_Postion;
    [SerializeField] private Transform right_Postion;

    [SerializeField] private Transform left_Bowler;
    [SerializeField] private Transform right_Bowler;

   

    [SerializeField]private float flt_MovementSpeed = 10;
    [SerializeField] private float flt_RotationSpeed = 10;

    private float flt_HorizontalInput;   //Input Value
    private bool isRotate;

    float flt_TargetPostion;
    float flt_CurrentPostion;
    private Vector2 startTouchPosition;
    private Vector2 moveDirection;
    private float flt_Delta = 4;
    private float flt_SenstyVity = 50;


    private float flt_MinClampPostion;
    private float flt_maxClampPostion;
    private float flt_DistanceBetweenCenterToEdgeOfPaddle = 0.5f;
    private float flt_BallMinForce = 5;
    private float flt_BallMaxForce = 30;
    private float flt_MaxSwingForce = 10;

    public PlayerState MyState { get; set; }

    public void SetClampPostion() {


        flt_MinClampPostion = (left_Postion.position.x + transform.localScale.x / 2);
        flt_maxClampPostion = (right_Postion.position.x - transform.localScale.x / 2);
        transform.position = new Vector3(transform.position.x, Camera.main.orthographicSize - 7.5f, transform.position.z) ;
        MyState = PlayerState.BatsMan;
    }

    public void SetBollwerPostion() {

        flt_MinClampPostion = (left_Bowler.position.x + transform.localScale.x / 2);
        flt_maxClampPostion = (right_Bowler.position.x - transform.localScale.x / 2);
        transform.position = new Vector3(transform.position.x, -Camera.main.orthographicSize + 4.5f, transform.position.z);
        MyState = PlayerState.Bowler;
        isRotate = false;
        transform.rotation = Quaternion.identity;

    }


    private void Update() {
        PaddleMotion();
       
      


    }

    private void PaddleMotion() {

#if UNITY_EDITOR
        flt_HorizontalInput = Input.GetAxis("Horizontal");


        if (Input.GetKey(KeyCode.Space)) {

            isRotate = true;
        }
        else {
            isRotate = false;
        }

        transform.Translate(Vector3.right * flt_HorizontalInput * flt_MovementSpeed * Time.deltaTime, Space.World);
        float x_Postion = transform.position.x;

        x_Postion = Mathf.Clamp(x_Postion, flt_MinClampPostion, flt_maxClampPostion);

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
                flt_TargetPostion = Mathf.Clamp(flt_TargetPostion, flt_MinClampPostion, flt_maxClampPostion);
                flt_CurrentPostion = transform.position.x;
                flt_CurrentPostion = Mathf.Lerp(flt_CurrentPostion, flt_TargetPostion, Time.deltaTime * flt_MovementSpeed);
                transform.position = new Vector2(flt_CurrentPostion, transform.position.y);

            }

        }


    }


    public void BtnClick(bool value) {
        isRotate = value;
    }

    private void RotateClockWisePaddle() {

        if (isRotate) {
            transform.Rotate(Vector3.forward * flt_RotationSpeed * Time.deltaTime);
        }

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

}
