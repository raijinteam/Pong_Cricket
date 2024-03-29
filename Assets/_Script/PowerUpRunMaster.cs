using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpRunMaster : Powerup {

    [SerializeField] private float flt_ActiveTime;
    [SerializeField] private int RunIncreased;
  
    private float flt_CurrentTime;
    private bool hasPlayerActivePowerup;



    private void Update() {
        if (!isPowerupActive) {
            return;
        }

        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime >= flt_ActiveTime) {
            DeActivtedMyPowerup();
        }
    }


    public override void ActivtedMyPowerup(AbilityType type, bool Isplayer) {
        if (myType != type) {
            return;
        }
        flt_CurrentTime = 0;
        isPowerupActive = true;
        int index = AbilityManager.Instance.GetAbilityCurrentLevelWithType(myType);
        flt_ActiveTime = AbilityManager.Instance.GetAbliltyData(myType).all_PropertyOneValues[index];
        RunIncreased = ((int)AbilityManager.Instance.GetAbliltyData(myType).all_PropertyTwoValues[index]);
        hasPlayerActivePowerup = Isplayer;
        if (Isplayer) {

            GameManager.Instance.CurrentGamePlayer.ActivetedRunIncreased(RunIncreased);
        }
        else {
            GameManager.Instance.CurrentGamePlayerAI.ActivetedRunIncreased(RunIncreased);
        }


       

        
    }

    public override void DeActivtedMyPowerup() {
        isPowerupActive = false;
        if (hasPlayerActivePowerup) {
            GameManager.Instance.CurrentGamePlayer.DeActivetedRunIncreased();
        }
        else {
            GameManager.Instance.CurrentGamePlayerAI.DeActivetedRunIncreased();
        }
    }
}
