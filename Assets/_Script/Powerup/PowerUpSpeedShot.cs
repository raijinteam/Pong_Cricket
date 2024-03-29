
using UnityEngine;

public class PowerUpSpeedShot : Powerup {

    [SerializeField] private float flt_ActiveTime;
    [SerializeField] private float flt_PersantageOfShotAmmount;
    private float flt_CurrentTime;
    private bool hasPlayerActivatedPowerup;
   

    private void Update() {
        if (!GameManager.Instance.IsGameRunning) {
            return;
        }
        if (!isPowerupActive) {
            return;
        }
        TimecalCulation();
    }

    private void TimecalCulation() {
        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime > flt_ActiveTime) {
            DeActivtedMyPowerup();
        }
    }


    

    // Get Spped of Ball
    public float GetShotSpeedIncreaseValue(float _baseSpeed) {
        float currentSpeed = _baseSpeed;

        currentSpeed += currentSpeed * 0.01f * flt_PersantageOfShotAmmount;
        
        return currentSpeed;
    }

    



  

    public override void ActivtedMyPowerup(AbilityType type, bool isplayer) {
        if (myType != type) {
            return;
        }

        //Player Shot Increased Ammount
        if (isplayer) {

            GameManager.Instance.CurrentGamePlayer.ActivetedSpeedShotPowerUp();

        }
        else {

            GameManager.Instance.CurrentGamePlayerAI.ActivetedSpeedShotPowerUp();

        }
        isPowerupActive = true;
        hasPlayerActivatedPowerup = isplayer;
        int index = AbilityManager.Instance.GetAbilityCurrentLevelWithType(myType);
        flt_ActiveTime = AbilityManager.Instance.GetAbliltyData(myType).all_PropertyOneValues[index];
        flt_PersantageOfShotAmmount = AbilityManager.Instance.GetAbliltyData(myType).all_PropertyTwoValues[index];
        flt_CurrentTime = 0;
    }

    public override void DeActivtedMyPowerup() {
        isPowerupActive = false;
        if (hasPlayerActivatedPowerup) {

            GameManager.Instance.CurrentGamePlayer.DeActivetedSpeedShotPowerUp();
        }
        else {

            GameManager.Instance.CurrentGamePlayerAI.DeActivetedSpeedShotPowerUp();
        }
    }
}
