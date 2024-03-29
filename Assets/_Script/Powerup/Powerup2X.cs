using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup2X : Powerup {

   
    
    private float flt_ActiveTime = 5;  // Max Time To Run PowerUp
    private float flt_CurrentTime;  //Current Runing Time

   

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
        isPowerupActive = true;
        flt_CurrentTime = 0;
        GameManager.Instance.Powerup2XActive();
    }

    public override void DeActivtedMyPowerup() {
        isPowerupActive = false;
        GameManager.Instance.Powerup2XDeActived();
    }
}
