using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PowerUpInvincible : Powerup {

    [SerializeField] private float flt_ActiveTime;   // Max Time
    private float flt_CurrentTime;

    private void Update() {
        if (!GameManager.Instance.IsGameRunning) {
            return;
        }
        if (!isPowerupActive) {
            return;
        }
        TimeHandlerInviciblePowerUp();
    }

    private void TimeHandlerInviciblePowerUp() {
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
        GameManager.Instance.ballMovement.ActivateInvisiblePowerup(Isplayer);
    }

    public override void DeActivtedMyPowerup() {
        isPowerupActive = false;
        GameManager.Instance.ballMovement.DisableInvisiblePowerup();
    }
}
