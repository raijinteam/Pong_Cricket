using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoPlayerAi : MonoBehaviour {

    [SerializeField] private Transform target;
    private Vector3 targetPosition;

    public PlayerState MyState;   // Player State

    [Header("Player Data")] // Get For Player Data
    [SerializeField] private float flt_MaxSwingForce;   // Min Swing Froce
    [SerializeField] private float flt_MinSwingForce;  // Max Swing Force
    [SerializeField] private float flt_BallMaxForce;        // Min force Apply at ball
    [SerializeField] private float flt_BallMinForce;        // Min force Apply at ball



    [SerializeField] private float flt_PaddleRoationSpeed;   // Rotation Speed


    [SerializeField] private float flt_PaddleMovementSpeed;  // Movement Speed

    private float flt_CurrentPaddleRoationSpeed;
    private float flt_CurrentPaddleMovementSpeed;

    private float centerOfBatOffset = 0.1f;
    private float maxDistanceToCenterOffset = 0.25f;
    private float currentRandomedOffset;

    // Clamp data
    [SerializeField]private float flt_MinCalmpValue = -3;  //Left Clamp Value
    [SerializeField] private float flt_MaxClampValue = 3;  // Right Clamp Value

    // force CalculationData
    private float flt_DistanceBetweenCenterToEdgeOfPaddle = 0.5f;   // this is Max Vaue  when Ball Distance;






    // Ai Chasing
    public bool playerHitBall;
    public bool shouldChasing = true;
    private float flt_CahsingRange = 0;



   

    private void Start() {

        flt_CurrentPaddleMovementSpeed = flt_PaddleMovementSpeed;
        flt_CurrentPaddleRoationSpeed = flt_PaddleRoationSpeed;
        shouldChasing = true;
        playerHitBall = false;
        SetCurrentTargetOffset();
    }

   


    private void Update() {

        PlayerAiMotion();
    }

    private void PlayerAiMotion() {



      

        //Motion As Per Input

        if (shouldChasing) {
            MoveTowardsMainBall();
        }
        else {

            MoveTowardsTargetBall();
        }

        HandlingChasing();
    }

    private void MoveTowardsMainBall() {

        targetPosition.x = DemoGameManager.instance.demoBALL.transform.position.x + currentRandomedOffset;
        targetPosition.y = transform.position.y;
        transform.position = Vector3.Lerp(transform.position, targetPosition, flt_CurrentPaddleMovementSpeed * Time.deltaTime);
        float x_Postion = transform.position.x;
        x_Postion = Mathf.Clamp(x_Postion, flt_MinCalmpValue, flt_MaxClampValue);
        transform.position = new Vector3(x_Postion, transform.position.y, transform.position.z);
    }

    private Transform targetBall;
    bool isReachedTargetPostion = false;

    private void MoveTowardsTargetBall() {

      

        if (targetBall == null) {

            if (!isReachedTargetPostion) {
                targetPosition.x = Random.Range(6, -6);
                isReachedTargetPostion = true;
            }

            if (Mathf.Abs(targetPosition.x - transform.position.x) < 0.5f) {
                isReachedTargetPostion = false;
            }

        }
        else {
            targetPosition.x = targetBall.position.x + currentRandomedOffset;
            isReachedTargetPostion = false;
        }


        targetPosition.y = transform.position.y;
        transform.position = Vector3.Lerp(transform.position, targetPosition, flt_CurrentPaddleMovementSpeed * Time.deltaTime * 0.5f);
        float x_Postion = transform.position.x;
        x_Postion = Mathf.Clamp(x_Postion, flt_MinCalmpValue, flt_MaxClampValue);
        transform.position = new Vector3(x_Postion, transform.position.y, transform.position.z);
    }

    private void HandlingChasing() {
        if (!shouldChasing && playerHitBall) {

            if (MyState == PlayerState.Bowler) {

                if (DemoGameManager.instance.demoBALL.transform.position.y < flt_CahsingRange) {
                    shouldChasing = true;
                    playerHitBall = false;
                }
            }
            else {
                if (DemoGameManager.instance.demoBALL.transform.position.y > flt_CahsingRange) {
                    shouldChasing = true;
                    playerHitBall = false;
                }
            }
        }
        else {

            if (MyState == PlayerState.Bowler) {

                if (DemoGameManager.instance.demoBALL.transform.position.y > flt_CahsingRange) {
                    shouldChasing = false;
                }



            }
            else {
                if (DemoGameManager.instance.demoBALL.transform.position.y < flt_CahsingRange) {
                    shouldChasing = false;
                }


            }
        }



    }

 

    private void SetCurrentTargetOffset() {

        if (MyState == PlayerState.Bowler) {

            int randomRangeIndex = Random.Range(0, 2);
            if (randomRangeIndex == 0) {
                // TRY TO GO FOR POWER
                currentRandomedOffset = Random.Range(-centerOfBatOffset, centerOfBatOffset);
            }
            else {
                // TRY TO GO FOR Swing
                currentRandomedOffset = Random.Range(-maxDistanceToCenterOffset, maxDistanceToCenterOffset);

            }

        }
        else {
            // TRY TO GO FOR POWER
            currentRandomedOffset = Random.Range(-centerOfBatOffset, centerOfBatOffset);
        }



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


   






    public float flt_GetBallForceAsPerCollsionForce(Vector2 point) {
        float CurrntXPoint = Mathf.Abs(point.x);
        float flt_BallForce = 0;
        if (CurrntXPoint > flt_DistanceBetweenCenterToEdgeOfPaddle) {
            flt_BallForce = flt_BallMinForce;
        }
        else {
            flt_BallForce = (CurrntXPoint / flt_DistanceBetweenCenterToEdgeOfPaddle) * (flt_BallMinForce - flt_BallMaxForce) + flt_BallMaxForce;
        }
        Debug.Log("Before Ai Force" + flt_BallForce);

    
     

        Debug.Log("after Ai Force" + flt_BallForce);
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

   


    // ball Hit Time Procedure

    // ball Hit After some Delay Reset Paddle
    public void BallHitWithPaddle() {

        if (MyState == PlayerState.BatsMan) {
            StartCoroutine(delay_OfResetPaddle());
        }

        Debug.Log("Hiii");
        shouldChasing = false;
        SetCurrentTargetOffset();
    }
    public void PLayerHitBall() {
        playerHitBall = true;
    }





   
}
