using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCollsionHandler : MonoBehaviour {

    [SerializeField] private float flt_MaxSwingForce;   // min Swing force
    [SerializeField] private float flt_MinSwingForce;  // max Swing force
    [SerializeField] private float flt_BallMaxForce;   // Min force Apply at ball
    [SerializeField] private float flt_BallMinForce;     // Min force Apply at ball
    private float flt_CurrentBallMaxForce;   // Min force Apply at ball
    private float flt_CurrentBallMinForce;     // Min force Apply at ball

    // force CalculationData
    private float flt_DistanceBetweenCenterToEdgeOfPaddle = 0.5f;   // Distance between centre of the paddle to edge of the paddle

    // Run Increased
    private bool isRunIncreased;
    private int RunIncreased;

    // spin Docter
    private int per_Swingforce;
    private bool isSpinDocterActiveted;

    // Ball Split
    [SerializeField] private bool IsBallSplitPowerupActive;
    [SerializeField] private SmallBallMotion prefab_SmallBall;
    [SerializeField] private int NoOfBall;

    private PlayerData playerData;



    private void Awake() {
        playerData = GetComponent<PlayerData>();
    }

    public void SetPlayerForceData(int index) {



        flt_BallMinForce = (playerData.MyState == PlayerState.BatsMan) ?
            CharacterManager.Instance.GetCharacterBattingPowerForCurrentLevel(index) :
             CharacterManager.Instance.GetCharacterBowlingPowerForCurrentLevel(index);



           ;
        flt_MinSwingForce = CharacterManager.Instance.GetCharacterSpinForceForCurrentLevel(index);

       

        flt_CurrentBallMaxForce = flt_BallMaxForce;
        flt_CurrentBallMinForce = flt_BallMinForce;

        
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

        if (isRunIncreased) {
            GameManager.Instance.IncreasedRun(RunIncreased);
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

        if (isSpinDocterActiveted) {
            flt_BallForce += flt_BallForce * per_Swingforce * 0.01f;
        }

        return flt_BallForce;
    }





    #region Ball Spilt

    public void ActivetedBallSplit(int _NoOfBall) {
        IsBallSplitPowerupActive = true;
        this.NoOfBall = _NoOfBall;
    }
    public void DeActivetedBallSplit() {
        IsBallSplitPowerupActive = false;
    }

    private void spawnBall() {

        for (int i = 0; i < NoOfBall; i++) {

            if (playerData.MyState == PlayerState.BatsMan) {
                SmallBallMotion current = Instantiate(prefab_SmallBall, transform.position + new Vector3(Random.Range(-5, 5), -1, 0), transform.rotation);
                current.SetRandomVelocityOfBall(new Vector3(Random.Range(-3, 3), -3, 0));
            }
            else {
                SmallBallMotion current = Instantiate(prefab_SmallBall, transform.position + new Vector3(Random.Range(-5, 5), 1, 0), transform.rotation);
                current.SetRandomVelocityOfBall(new Vector3(Random.Range(-3, 3), 3, 0));
            }

        }
    }
    #endregion

    //Spin Docter
    public void ActivetedSpinDocter(int per_SwingForce) {
        isSpinDocterActiveted = true;
        this.per_Swingforce = per_SwingForce;
    }

    public void DeActivetedSpinDocter() {
        isSpinDocterActiveted = false;
    }



    // Run Increased
    public void ActivetedRunIncreased(int runIncreased) {
        isRunIncreased = true;
        this.RunIncreased = runIncreased;
    }

    public void DeActivetedRunIncreased() {
        isRunIncreased = false;

    }


    public void ActivetedSpeedShotPowerUp() {

        flt_CurrentBallMinForce = PowerUpManager.Instance.PowerUpSpeedShot.GetShotSpeedIncreaseValue(flt_CurrentBallMinForce);
        flt_CurrentBallMaxForce = PowerUpManager.Instance.PowerUpSpeedShot.GetShotSpeedIncreaseValue(flt_CurrentBallMaxForce);
    }

    public void DeActivetedSpeedShotPowerUp() {

        flt_CurrentBallMaxForce = flt_BallMaxForce;
        flt_CurrentBallMinForce = flt_BallMinForce;
    }


}
