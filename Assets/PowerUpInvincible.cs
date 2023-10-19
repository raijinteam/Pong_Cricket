using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpInvincible : MonoBehaviour {

    [SerializeField] private int NoOfShot;   // Max Shot When PowerUp Active
  
    private int CurrentShot;
    private bool hasPlayerActivatedPowerup;


    // End Of PowerUp Precedure
    private void DeActivePower() {

        if (hasPlayerActivatedPowerup) {

            GameManager.Instance.CurrentGamePlayer.DeActivetedInvicliblePowerUp();
        }
        else {

            GameManager.Instance.CurrentGamePlayerAI.DeActivetedInvicliblePowerUp();
        }
        this.gameObject.SetActive(false);
    }

    

    public void IncreaseNumberOfShots() {
        Debug.Log("BeforeCurrentShot" + CurrentShot);
        CurrentShot++;
        Debug.Log("CurrentShot" + CurrentShot);
        if (CurrentShot >= NoOfShot) {
            DeActivePower();
        }
    }


    // This Powerup Work Both
    public void ActivatedInvicliblePowerUp(bool isplayer) {

        //PlayerInvicible Increased Ammount
        if (isplayer) {

            GameManager.Instance.CurrentGamePlayer.ActivetedInvicliblePowerUp();

        }
        else {

            GameManager.Instance.CurrentGamePlayerAI.ActivetedInvicliblePowerUp();

        }

        hasPlayerActivatedPowerup = isplayer;
        CurrentShot = 0;
        this.gameObject.SetActive(true);
    }

}
