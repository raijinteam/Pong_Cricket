using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : MonoBehaviour {


    [SerializeField] private Transform target;
    public PlayerState MyState;   // Player State

    [Header("Player Data")] // Get For Player Data
    [SerializeField] private float flt_MaxSwingForce;   // Min Swing Froce
    [SerializeField] private float flt_MinSwingForce;  // Max Swing Force
    [SerializeField] private float flt_BallMaxForce;        // Min force Apply at ball
    [SerializeField] private float flt_BallMinForce;        // Min force Apply at ball
    [SerializeField] private float flt_PaddleRoationSpeed;   // Rotation Speed
    [SerializeField] private float flt_PaddleMovementSpeed;  // Movement Speed

    // InputData
    private float flt_HorizontalInput;   // Input Variable

    // Clamp data
    private float flt_MinCalmpValue = -6;  // Min Calmp Value
    private float flt_MaxClampValue = 6;  // Max Clamp value

    // force CalculationData
    private float flt_MaxPointToTouchBall = 0.5f;   // this is Max Vaue  when Ball Distance;

   

    // Ai Rotation Data

    private float flt_RotateRange = 8;   // this Is Range when Roation Calculation Start
    private float flt_MinLeftAngle = 10;  // Min Target angle in LeftSide;
    private float flt_MaxLeftAngle = 60;  // Max Target Angle in LeftSide
    private float flt_MaxRightAngle = 170; //Min Target angle in RightSide;
    private float flt_MinRightAngler = 120;  //Min Target angle in RightSide;
    private bool isPaddleRotate = false;  // Status Of Paddle Rotating Or not
    [SerializeField] private float rotationSpeed = 5.0f; // Adjust this to control the rotation speed





    private void Update() {

        if (!GameManager.Instance.IsGameStart) {
            return;
        }
        else if (MyState == PlayerState.BatsMan  && GameManager.Instance.CurrentGameBall.position.y > transform.position.y) {

            // MyPlayer Is Batsman And Ball Is Above So Not woking Motion
            return;
        }
        else if (MyState == PlayerState.Bowler && GameManager.Instance.CurrentGameBall.position.y < transform.position.y) {

            // My Player bowller And Ball Is Down Side so Not working Motion
            return;
        } 


        // Geting Input
        UserInput();

        //Motion As Per Input
        PaddleMotion();

       
       
    }

    private void PaddleMotion() {

        transform.Translate(Vector3.right * flt_HorizontalInput * flt_PaddleMovementSpeed * Time.deltaTime, Space.World);
        float x_Postion = transform.position.x;

        x_Postion = Mathf.Clamp(x_Postion, flt_MinCalmpValue, flt_MaxClampValue);

        transform.position = new Vector3(x_Postion, transform.position.y, transform.position.z);
    }

    private void UserInput() {

        Vector3 direction = (GameManager.Instance.CurrentGameBall.position - transform.position).normalized;
        direction = new Vector3(direction.x, 0, 0).normalized;
        if (Mathf.Abs(transform.position.x - GameManager.Instance.CurrentGameBall.position.x) > 1) {
            flt_HorizontalInput = direction.x;
        }
        else {
            flt_HorizontalInput = 0;
        }


        if (MyState == PlayerState.BatsMan) {

            BatManInputProcedure();
        }


    }

    private void BatManInputProcedure() {


        // Range Caluculation

        float flt_Disatnce = Mathf.Abs(Vector2.Distance(transform.position, GameManager.Instance.CurrentGameBall.position));


        // check Paddle Rotrating Or Not And Range 
        // After Chweckinh Get target Angle
        if (!isPaddleRotate && flt_Disatnce < flt_RotateRange) {

            isPaddleRotate = true;
            float flt_Angle = 0;
            if (GameManager.Instance.CurrentGameBall.position.x - transform.position.x > 0) {
                flt_Angle = Random.Range(flt_MinRightAngler, flt_MaxRightAngle);
            }
            else {
                flt_Angle = Random.Range(flt_MinLeftAngle, flt_MaxLeftAngle);
            }

            
            target.localEulerAngles = new Vector3(0, 0, flt_Angle);



        }

        // Out Side range paddle Roatte stop
        if (flt_Disatnce > flt_RotateRange + 1) {
            isPaddleRotate = false;
        }

        RotatePLayer();
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
            flt_BallForce = flt_MaxSwingForce;
        }
        else {
            flt_BallForce = (CurrntXPoint / flt_MaxPointToTouchBall) * flt_MaxSwingForce;
        }

        if (point.x < 0) {
            flt_BallForce = -flt_BallForce;
        }

        return flt_BallForce;
    }

    public void SetPlayerState(PlayerState _myState) {
        this.MyState = _myState;
    }


    // ball Hit Time Procedure

    // ball Hit After some Delay Reset Paddle
    public void BallHitWithPaddle() {
        StartCoroutine(delay_OfResetPaddle());
       
    }
    private IEnumerator delay_OfResetPaddle() {

        yield return new WaitForSeconds(2);
        float index = Random.Range(-5, 5);

        if (transform.eulerAngles.z > 180) {

            target.eulerAngles = new Vector3(0, 0, index);

        }
        else {

            target.eulerAngles = new Vector3(0, 0, 180 + index);

        }
        

    }

   

    void RotatePLayer() {
       
        if (Mathf.Abs(NormalizeAngle(target.localEulerAngles.z) - transform.localEulerAngles.z) <10) {
            
            //Target and Current Angle Diffrent < 10 so , stop This;
            return;
        }

        // Calculate the direction from the current rotation to the target rotation
        Vector3 directionToTarget = target.localEulerAngles - transform.localEulerAngles;

        // Ensure that the angle difference is always positive
        float angleDifference = NormalizeAngle(directionToTarget.z);

        // Rotate towards the target with a fixed speed (counter-clockwise)
        float step =  rotationSpeed*Time.deltaTime;
        transform.Rotate(Vector3.forward, step);
       


    }

    // Ensure that the angle is in the range [0, 360)
    float NormalizeAngle(float angle) {
        angle %= 360.0f;
        if (angle < 0)
            angle += 360.0f;
        return angle;
    }

}

