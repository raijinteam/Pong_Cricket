using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class PowerUpFreeze : Powerup {

    [SerializeField]private float flt_ActiveTime = 5;  // Max Time To Run PowerUp
    [SerializeField] private float flt_FreezTime = 1;  //Its FreezTime When Player Stop Moveing
    [SerializeField] private float flt_IntervalTime = 0.5f;   // Its InterVal Time When PLayer Move ;
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
        flt_FreezTime = AbilityManager.Instance.GetAbliltyData(myType).all_PropertyTwoValues[index];
        //Oppsotite Player Freeez
        if (Isplayer) {

            GameManager.Instance.CurrentGamePlayerAI.ActivateFreezePowerup(flt_FreezTime, flt_IntervalTime);

        }
        else {

            GameManager.Instance.CurrentGamePlayer.ActivateFreezePowerup(flt_FreezTime, flt_IntervalTime);

        }

        hasPlayerActivatedPowerup = Isplayer;
        flt_CurrentTime = 0;
    }

    public override void DeActivtedMyPowerup() {
        if (hasPlayerActivatedPowerup) {

            GameManager.Instance.CurrentGamePlayerAI.DeActivateFreezePowerup();
        }
        else {

            GameManager.Instance.CurrentGamePlayer.DeActivateFreezePowerup();
        }
    }
}
