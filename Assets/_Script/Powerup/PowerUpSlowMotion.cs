using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSlowMotion : Powerup {

    [SerializeField] private float flt_ActiveTime = 5;  // Max Time To Run PowerUp
    [SerializeField] private float flt_PersantageOfSlowMotion;
    private float flt_CurrentTime;  //Current Runing Time
    private bool hasPlayerActivatedPowerup;


    private void Update() {
        if (!GameManager.Instance.IsGameRunning) {
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


    // This Powerup Work Both
    public void ActivateSlowMotionPowerUp(bool isplayer) {

       
        this.gameObject.SetActive(true);

    }

    

    public override void ActivtedMyPowerup(AbilityType type, bool Isplayer) {
        if (myType != type) {
            return;
        }

        //Oppsotite Player Get Slow Persantage
        if (Isplayer) {

            GameManager.Instance.CurrentGamePlayerAI.ActivateSlowMotionPowerup(flt_PersantageOfSlowMotion);

        }
        else {

            GameManager.Instance.CurrentGamePlayer.ActivateSlowMotionPowerup(flt_PersantageOfSlowMotion);

        }

        int index = AbilityManager.Instance.GetAbilityCurrentLevelWithType(myType);
        flt_ActiveTime = AbilityManager.Instance.GetAbliltyData(myType).all_PropertyOneValues[index];
        flt_PersantageOfSlowMotion = AbilityManager.Instance.GetAbliltyData(myType).all_PropertyTwoValues[index];

        hasPlayerActivatedPowerup = Isplayer;
        flt_CurrentTime = 0;
        isPowerupActive = true;

    }

    public override void DeActivtedMyPowerup() {
        isPowerupActive = false;
        if (hasPlayerActivatedPowerup) {

            GameManager.Instance.CurrentGamePlayerAI.DeActivateSlowMotionPowerup();
        }
        else {

            GameManager.Instance.CurrentGamePlayer.DeActivateSlowMotionPowerup();
        }
    }
}
