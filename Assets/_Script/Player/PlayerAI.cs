using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : MonoBehaviour {


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
    private float flt_MinCalmpValue = -3;  //Left Clamp Value
    private float flt_MaxClampValue = 3;  // Right Clamp Value

    // force CalculationData
    private float flt_DistanceBetweenCenterToEdgeOfPaddle = 0.5f;   // this is Max Vaue  when Ball Distance;

   




    // Ai Chasing
    public bool playerHitBall;
    public bool shouldChasing = true;
    private float flt_CahsingRange = 0;

   

    // Ai Rotation Data

    private float flt_RotateRange = 8;   // this Is Range when Roation Calculation Start
    private float flt_MinLeftAngle = 10;  // Min Target angle in LeftSide;

    

    private float flt_MaxLeftAngle = 60;  // Max Target Angle in LeftSide
    private float flt_MaxRightAngle = 170; //Min Target angle in RightSide;
    private float flt_MinRightAngler = 120;  //Min Target angle in RightSide;

   

    private bool isPaddleRotate = false;  // Status Of Paddle Rotating Or not
 
    private void Start() {
      
        flt_CurrentPaddleMovementSpeed = flt_PaddleMovementSpeed;
        flt_CurrentPaddleRoationSpeed = flt_PaddleRoationSpeed;
        shouldChasing = true;
        playerHitBall = false;
        SetCurrentTargetOffset();
    }

    public void SetValueOfClampPosition() {
        WallHandler Current = GameManager.Instance.wallHandler;
        if (MyState == PlayerState.BatsMan) {
            flt_MinCalmpValue = Current.batsmanleft.position.x + transform.localScale.x / 2;
            flt_MaxClampValue = Current.batsmanright.position.x - transform.localScale.x / 2;
        }
        else {
            flt_MinCalmpValue = Current.bowlerleft.position.x + transform.localScale.x / 2;
            flt_MaxClampValue = Current.bowlerright.position.x - transform.localScale.x / 2;
        }
    }


    private void Update() {

        if (!GameManager.Instance.IsGameRunning) {
            return;
        }

        HandlingFreezePowerUp();

        PlayerAiMotion();
    }

    private void PlayerAiMotion() {



        if (MyState == PlayerState.BatsMan && GameManager.Instance.CurrentGameBallTransform.position.y > transform.position.y) {

            // MyPlayer Is Batsman And Ball Is Above So Not woking Motion
            return;
        }
        else if (MyState == PlayerState.Bowler && GameManager.Instance.CurrentGameBallTransform.position.y < transform.position.y) {

            // My Player bowller And Ball Is Down Side so Not working Motion
            return;
        }
        else if (isFreezPostion) {
            return;   // freeez PowwerUp Active And Freez Postion
        }

        if (MyState == PlayerState.BatsMan) {

            BatManRotationProcedure();
        }

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

        targetPosition.x = GameManager.Instance.CurrentGameBallTransform.position.x + currentRandomedOffset;
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

            if (PowerUpManager.Instance.PowerupBallSplit.gameObject.activeSelf && PowerUpManager.Instance.PowerupBallSplit.list_ActivaterBall.Count != 0) {

                targetBall = PowerUpManager.Instance.PowerupBallSplit.list_ActivaterBall[0].transform;
               
            }
        }

        if (targetBall == null ) {

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
        transform.position = Vector3.Lerp(transform.position, targetPosition, flt_CurrentPaddleMovementSpeed * Time.deltaTime*0.5f);
        float x_Postion = transform.position.x;
        x_Postion = Mathf.Clamp(x_Postion, flt_MinCalmpValue, flt_MaxClampValue);
        transform.position = new Vector3(x_Postion, transform.position.y, transform.position.z);
    }

    private void HandlingChasing() {
        if (!shouldChasing && playerHitBall) {

            if (MyState == PlayerState.Bowler) {

                if (GameManager.Instance.CurrentGameBallTransform.transform.position.y < flt_CahsingRange) {
                    shouldChasing = true;
                    playerHitBall = false;
                }
            }
            else {
                if (GameManager.Instance.CurrentGameBallTransform.transform.position.y > flt_CahsingRange) {
                    shouldChasing = true;
                    playerHitBall = false;
                }
            }
        }
        else {

            if (MyState == PlayerState.Bowler) {

                if (GameManager.Instance.CurrentGameBallTransform.transform.position.y > flt_CahsingRange) {
                    shouldChasing = false;
                }



            }
            else {
                if (GameManager.Instance.CurrentGameBallTransform.transform.position.y < flt_CahsingRange) {
                    shouldChasing = false;
                }


            }
        }

       

    }

    private void BatManRotationProcedure() {

        //if (!shouldChasing) {
        //    return;
        //}

        // Range Caluculation

        float flt_Disatnce = Mathf.Abs(Vector2.Distance(transform.position, GameManager.Instance.CurrentGameBallTransform.position));


        // check Paddle Rotrating Or Not And Range 
        // After Chweckinh Get target Angle
        if (!isPaddleRotate && flt_Disatnce < flt_RotateRange) {

            isPaddleRotate = true;
            float flt_Angle = 0;
            if (GameManager.Instance.CurrentGameBallTransform.position.x - transform.position.x > 0) {
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
   

    void RotatePLayer() {
        
       
        if (Mathf.Abs(NormalizeAngle(target.localEulerAngles.z) - transform.localEulerAngles.z) <10) {
            
            //Target and Current Angle Diffrent < 10 so , stop This;
            return;
        }


        // Rotate towards the target with a fixed speed (counter-clockwise)
        float step =  flt_CurrentPaddleRoationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.forward, step);   
    }

    // Ensure that the angle is in the range [0, 360)
    float NormalizeAngle(float angle) {
        angle %= 360.0f;
        if (angle < 0)
            angle += 360.0f;
        return angle;
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
        
        if (isSpeedShotPowerUpActive) {
            flt_BallForce = PowerUpManager.Instance.PowerUpSpeedShot.GetShotSpeedIncreaseValue(flt_BallForce);
        }

      
        if (IsBallSplitPowerupActive) {
            spawnBall();
        }

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

    public void SetPlayerState(PlayerState _myState) {
        this.MyState = _myState;
        SetValueOfClampPosition();
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





    // Power Up Handler
    [Header("Freez PowerUp")]
    [SerializeField]private bool isFreezPowerUpActive;  // Status Of Player Freez PowerUp
    [SerializeField]private bool isFreezPostion; // Status Of Freez Postion Or Not
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
        else if (flt_CurrentTimeForFreezPowerUp > flt_IntervalTime && !isFreezPostion) {
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



   
    public void ActivateSlowMotionPowerup(float _flt_PersantageOfSlowMotion) {

      
        flt_CurrentPaddleRoationSpeed -= flt_CurrentPaddleRoationSpeed * _flt_PersantageOfSlowMotion * 0.01f;
        flt_CurrentPaddleMovementSpeed -= flt_CurrentPaddleMovementSpeed * _flt_PersantageOfSlowMotion * 0.01f;
    }

    public void DeActivateSlowMotionPowerup() {

        flt_CurrentPaddleMovementSpeed = flt_PaddleMovementSpeed;
        flt_CurrentPaddleRoationSpeed = flt_PaddleRoationSpeed;      
    }


    // Ball Split
    [SerializeField] private bool IsBallSplitPowerupActive;
    [SerializeField] private SmallBallMotion prefab_SmallBall;
    [SerializeField] private int NoOfBall;

    private void spawnBall() {
      
        for (int i = 0; i < NoOfBall; i++) {
          
            if (MyState == PlayerState.BatsMan) {
                SmallBallMotion current = Instantiate(prefab_SmallBall, transform.position + new Vector3(Random.Range(-5, 5), -1, 0), transform.rotation);
                current.SetRandomVelocityOfBall(new Vector3(Random.Range(-3,3),-3,0));
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

        transform.localScale += Vector3.one * _ScaleIncrease;
        SetValueOfClampPosition();
    }

    public void ResetScale(float _ScaleIncrease) {
        transform.localScale -= Vector3.one * _ScaleIncrease;
        SetValueOfClampPosition();
    }

    #endregion
}

