using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PowerupGambler : Powerup {

    [SerializeField] private float flt_ActiveTime = 5;  // Max Time To Run PowerUp
    [SerializeField] private int runMultiPlier;  
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
        runMultiPlier = ((int)AbilityManager.Instance.GetAbliltyData(myType).all_PropertyTwoValues[index]);
        isPowerupActive = true;
        GameManager.Instance.ActivateGamblerPowerup(runMultiPlier);
        flt_CurrentTime = 0;
    }

    public override void DeActivtedMyPowerup() {
        isPowerupActive = false;
        GameManager.Instance.DeActivateGamblerPowerup();
    }
}


