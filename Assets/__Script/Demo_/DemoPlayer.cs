using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.EventSystems;

public class DemoPlayer : MonoBehaviour {


    [SerializeField] public SpriteRenderer sr;

    [SerializeField] private float flt_Paddlespeed;
    [SerializeField] private float flt_Rotationspeed;

    [SerializeField] private Transform postion;

    public bool IsRotate;

    [SerializeField] private float flt_ClampPostion;

    [field :SerializeField]public PlayerState MyState { get; internal set; }

   

    [SerializeField] private float flt_SenstyVity;
    [SerializeField] private float flt_MaxSwingForce;   // min Swing force
    [SerializeField] private float flt_MinSwingForce;  // max Swing force
    [SerializeField] private float flt_BallMaxForce;   // Min force Apply at ball
    [SerializeField] private float flt_BallMinForce;     // Min force Apply at ball
    private float flt_CurrentBallMaxForce;   // Min force Apply at ball
    private float flt_CurrentBallMinForce;     // Min force Apply at ball
    private float flt_DistanceBetweenCenterToEdgeOfPaddle = 1.2F;

    private float flt_HorizontalInput;

    



    //




    private void Start() {
        flt_ClampPostion = -postion.position.x;
        MyState = PlayerState.BatsMan;
        flt_CurrentBallMinForce = flt_BallMinForce;
        flt_CurrentBallMaxForce = flt_BallMaxForce;
    }



    private void Update() {



#if UNITY_EDITOR

        if (Input.GetKey(KeyCode.Space)) {

            transform.Rotate(Vector3.forward * flt_Rotationspeed * Time.deltaTime, Space.World);
        }

        flt_HorizontalInput = Input.GetAxis("Horizontal");


        transform.Translate(Vector3.right * flt_Paddlespeed * Time.deltaTime * flt_HorizontalInput, Space.World);
        float X = transform.position.x;
        X = Mathf.Clamp(transform.position.x, -flt_ClampPostion, flt_ClampPostion);
        transform.position = new Vector3(X, transform.position.y, transform.position.z);

#else
        SwipeControl();
       

#endif

        PaddleRotation();

    }

    public void BtnClick(bool value) {
        IsRotate = value;
    }

    float flt_TargetPostion;
    float flt_CurrentPostion;
    private Vector2 startTouchPosition;
    private Vector2 moveDirection;
    [SerializeField] private float flt_Delta;



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
                flt_TargetPostion = Mathf.Clamp(flt_TargetPostion, -flt_ClampPostion, flt_ClampPostion);
                flt_CurrentPostion = transform.position.x;
                flt_CurrentPostion = Mathf.Lerp(flt_CurrentPostion, flt_TargetPostion, Time.deltaTime * flt_Paddlespeed);
                transform.position = new Vector2(flt_CurrentPostion, transform.position.y);

            }
   
        }
       
       
    }



   
   

    private void PaddleRotation() {
        if (IsRotate) {
            transform.Rotate(Vector3.forward * flt_Rotationspeed * Time.deltaTime);
        }

    }





   


    public float flt_GetBallForceAsPerCollsionForce(Vector2 point) {
        float CurrntXPoint = Mathf.Abs(point.x);
        Debug.Log($"CurrentXpoint {CurrntXPoint}");
        float flt_BallForce = 0;
        if (CurrntXPoint > flt_DistanceBetweenCenterToEdgeOfPaddle) {
            flt_BallForce = flt_CurrentBallMinForce;
        }
        else {
            flt_BallForce = (CurrntXPoint / flt_DistanceBetweenCenterToEdgeOfPaddle) * (flt_CurrentBallMinForce - flt_CurrentBallMaxForce) + flt_CurrentBallMaxForce;
        }
        Debug.Log("Before" + flt_BallForce);

       
      
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


}


