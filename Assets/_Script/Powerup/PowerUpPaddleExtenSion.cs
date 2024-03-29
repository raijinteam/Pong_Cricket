using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPaddleExtenSion : Powerup {

    [SerializeField] private float flt_ActiveTime = 5;  // Max Time To Run PowerUp
    [SerializeField] private float flt_SizeIncreasedValue = 1;  //
    private float flt_CurrentTime;  //Current Runing Time
    private bool hasPlayerActivatedPowerup;  


   

    private void Update() {
        if (!GameManager.Instance.IsGameRunning) {
            return;
        }
        if (!isPowerupActive) {
            return;
        }
        PowerUpTimeCalculation();
    }

    private void PowerUpTimeCalculation() {

        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime > flt_ActiveTime) {

            DeActivtedMyPowerup();
        }
    }





   

    public override void ActivtedMyPowerup(AbilityType type, bool Isplayer) {
        if (myType != type) {
            return;
        }

        int index = AbilityManager.Instance.GetAbilityCurrentLevelWithType(myType);
        flt_ActiveTime = AbilityManager.Instance.GetAbliltyData(myType).all_PropertyOneValues[index];
        flt_SizeIncreasedValue   = AbilityManager.Instance.GetAbliltyData(myType).all_PropertyTwoValues[index];
        flt_CurrentTime = 0;
        // Player Scale Increased
        if (Isplayer) {

            GameManager.Instance.CurrentGamePlayer.ExtendPadle(flt_SizeIncreasedValue);

        }
        else {

            GameManager.Instance.CurrentGamePlayerAI.ExtendPadle(flt_SizeIncreasedValue);


        }

        hasPlayerActivatedPowerup = Isplayer;
        isPowerupActive = true;
    }

    public override void DeActivtedMyPowerup() {
        isPowerupActive = false;

        if (hasPlayerActivatedPowerup) {

            GameManager.Instance.CurrentGamePlayer.ResetScale(flt_SizeIncreasedValue);
        }
        else {
            GameManager.Instance.CurrentGamePlayerAI.ResetScale(flt_SizeIncreasedValue);

        }
    }
}
