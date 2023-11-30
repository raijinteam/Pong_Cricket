using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorial_Player : MonoBehaviour {
    

    [SerializeField] private Transform left_Postion;
    [SerializeField] private Transform right_Postion;

    [SerializeField] private Transform left_Bowler;
    [SerializeField] private Transform right_Bowler;

   

    [SerializeField]private float flt_MovementSpeed = 10;
    [SerializeField] private float flt_RotationSpeed = 10;

    private float flt_HoriZontalInput;
    private bool isRotate;
   
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
        UserInput();
        PaddleMotion();
        PaddleRotation();


    }
    
    public void BtnClick(bool value) {
        isRotate = value;
    }

   

    private  void PaddleRotation() {
        if (isRotate) {
            transform.Rotate(Vector3.forward * flt_RotationSpeed * Time.deltaTime);
        }
      
    }

    private void UserInput() {
        if (isRotate ) {
            return;
        }

       
        if (Input.GetMouseButton(0)) {
            Vector3 screenPostion = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (screenPostion.x > 0) {
                flt_HoriZontalInput = 1;

            }
            else {
                flt_HoriZontalInput = -1;
            }
        }
        else {
            flt_HoriZontalInput = 0;
        }
    }


    private void PaddleMotion() {

        transform.Translate(Vector3.right * flt_MovementSpeed * flt_HoriZontalInput * Time.deltaTime,Space.World);
        float X = transform.position.x;
        X = Mathf.Clamp(X, flt_MinClampPostion, flt_maxClampPostion);
        transform.position = new Vector3(X, transform.position.y, transform.position.z);
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
