using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpeedShot : MonoBehaviour {

    [SerializeField] private int NoOfShot;   // Max Shot When PowerUp Active
    [SerializeField] private float flt_PersantageOfShotAmmount;
    private int CurrentShot;
    private bool hasPlayerActivatedPowerup;

  
    // End Of PowerUp Precedure
    private void DeActivePower() {

        if (hasPlayerActivatedPowerup) {

            GameManager.Instance.CurrentGamePlayer.DeActivetedSpeedShotPowerUp();
        }
        else {

            GameManager.Instance.CurrentGamePlayerAI.DeActivetedSpeedShotPowerUp();
        }
        this.gameObject.SetActive(false);
    }

    public float GetShotSpeedIncreaseValue(float _baseSpeed) {
        float currentSpeed = _baseSpeed;

        currentSpeed += currentSpeed * 0.01f * flt_PersantageOfShotAmmount;
        IncreaseNumberOfShots();
        return currentSpeed;
    }

   public void IncreaseNumberOfShots() {

        CurrentShot++;
        if (CurrentShot >= NoOfShot) {
            DeActivePower();
        }
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
        CurrentShot = 0;
        this.gameObject.SetActive(true);
    }

}
