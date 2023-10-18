using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSlowMotion : MonoBehaviour {

    [SerializeField] private float flt_ActiveTime = 5;  // Max Time To Run PowerUp
    [SerializeField] private float flt_PersantageOfSlowMotion;
    private float flt_CurrentTime;  //Current Runing Time
    private bool hasPlayerActivatedPowerup;


    private void Update() {
        if (!GameManager.Instance.IsGameStart) {
            return;
        }
        PowerUpTimeCalculation();
    }




    // End Of PowerUp Precedure
    private void DeActivePower() {

        if (hasPlayerActivatedPowerup) {

            GameManager.Instance.CurrentGamePlayerAI.DeActivateSlowMotionPowerup();
        }
        else {

            GameManager.Instance.CurrentGamePlayer.DeActivateSlowMotionPowerup();
        }
        this.gameObject.SetActive(false);
    }


    private void PowerUpTimeCalculation() {

        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime > flt_ActiveTime) {

            DeActivePower();
        }
    }


    // This Powerup Work Both
    public void ActivateSlowMotionPowerUp(bool isplayer) {

        //Oppsotite Player Get Slow Persantage
        if (isplayer) {

            GameManager.Instance.CurrentGamePlayerAI.ActivateSlowMotionPowerup(flt_PersantageOfSlowMotion);

        }
        else {

            GameManager.Instance.CurrentGamePlayer.ActivateSlowMotionPowerup(flt_PersantageOfSlowMotion);

        }

        hasPlayerActivatedPowerup = isplayer;
        flt_CurrentTime = 0;
        this.gameObject.SetActive(true);

    }
}
