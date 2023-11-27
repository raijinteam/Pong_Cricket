using System.Collections;
using System.Collections.Generic;
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
        TimecalCulation();
    }

    private void TimecalCulation() {
        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime > flt_ActiveTime) {
            DeActivePower();
        }
    }


    // End Of PowerUp Precedure
    public void DeActivePower() {

        if (hasPlayerActivatedPowerup) {

            GameManager.Instance.CurrentGamePlayer.DeActivetedSpeedShotPowerUp();
        }
        else {

            GameManager.Instance.CurrentGamePlayerAI.DeActivetedSpeedShotPowerUp();
        }
        this.gameObject.SetActive(false);
    }

    // Get Spped of Ball
    public float GetShotSpeedIncreaseValue(float _baseSpeed) {
        float currentSpeed = _baseSpeed;

        currentSpeed += currentSpeed * 0.01f * flt_PersantageOfShotAmmount;
        
        return currentSpeed;
    }

    



    // This Powerup Work Both
    public void ActivateSpeedShotPowerUp(bool isplayer) {

        //Player Shot Increased Ammount
        if (isplayer) {

            GameManager.Instance.CurrentGamePlayer.ActivetedSpeedShotPowerUp();

        }
        else {

            GameManager.Instance.CurrentGamePlayerAI.ActivetedSpeedShotPowerUp();

        }

        hasPlayerActivatedPowerup = isplayer;
        flt_CurrentTime = 0;
        this.gameObject.SetActive(true);
    }

}
