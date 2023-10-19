using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupBallSplit : MonoBehaviour {

    [SerializeField] private int NoOfBall;
    [SerializeField] private int NoOfShot;   // Max Shot When PowerUp Active
    [SerializeField] private float flt_ActiveTime;
  
    private bool hasPlayerActivatedPowerup;
   

    // End Of PowerUp Precedure
    public  void DeActivePower() {

        if (hasPlayerActivatedPowerup) {
            GameManager.Instance.CurrentGamePlayer.DeActivetedBallSplit();
        }
        else {
            GameManager.Instance.CurrentGamePlayerAI.DeActivetedBallSplit();
        }
        this.gameObject.SetActive(false);
    }


   
   

    // This Powerup Work Both
    public void ActivatedBallSplitPowerUp(bool isplayer) {


        //Player Shot Increased Ammount
        if (isplayer) {
            GameManager.Instance.CurrentGamePlayer.ActivetedBallSplit(NoOfBall,NoOfShot);
        }
        else {
            GameManager.Instance.CurrentGamePlayerAI.ActivetedBallSplit(NoOfBall, NoOfShot);
        }

        hasPlayerActivatedPowerup = isplayer;
        
        this.gameObject.SetActive(true);
    }

}
