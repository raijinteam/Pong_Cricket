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
        PowerUpTimeCalculation();
    }

    private void PowerUpTimeCalculation() {

        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime > flt_ActiveTime) {

            DeActivePower();
        }
    }





    // End Of PowerUp Precedure
    public void DeActivePower() {

        if (hasPlayerActivatedPowerup) {

            GameManager.Instance.CurrentGamePlayer.ResetScale(flt_SizeIncreasedValue);
        }
        else {
            GameManager.Instance.CurrentGamePlayerAI.ResetScale(flt_SizeIncreasedValue);

        }
        this.gameObject.SetActive(false);
    }


   

    // This Powerup Work Both
    public void ActivatePaddleExtensionPowerUp(bool isplayer) {

        // Player Scale Increased
        if (isplayer) {

            GameManager.Instance.CurrentGamePlayer.ExtendPadle(flt_SizeIncreasedValue);

        }
        else {

            GameManager.Instance.CurrentGamePlayerAI.ExtendPadle(flt_SizeIncreasedValue);


        }

        hasPlayerActivatedPowerup = isplayer;
        flt_CurrentTime = 0;
        this.gameObject.SetActive(true);

    }
}
