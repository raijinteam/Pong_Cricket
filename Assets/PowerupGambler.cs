using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupGambler : MonoBehaviour {

    [SerializeField] private float flt_ActiveTime = 5;  // Max Time To Run PowerUp
    [SerializeField] private int runMultiPlier;  
    private float flt_CurrentTime;  //Current Runing Time
    
    


    private void Update() {
        if (!GameManager.Instance.IsGameStart) {
            return;
        }
        PowerUpTimeCalculation();
    }




    // End Of PowerUp Precedure
    public void DeActivePower() {

        GameManager.Instance.PowerupGameblerRunMultyplierDeActive();
        GameManager.Instance.PowerupGameblerAlloutDeActive();
        this.gameObject.SetActive(false);
    }


    private void PowerUpTimeCalculation() {

        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime > flt_ActiveTime) {

            DeActivePower();
        }
    }


    // This Powerup Work Both
    public void ActivateGamblerRunnerPowerUp(bool isplayer) {

       
        if (isplayer) {

            if (GameManager.Instance.CurrentGamePlayer.MyState == PlayerState.BatsMan) {
                GameManager.Instance.PowerupGameblerRunMultyplierActive(runMultiPlier);
            }
            else {
                GameManager.Instance.PowerupGameblerAlloutActive();
            }

        }
        else {

            if (GameManager.Instance.CurrentGamePlayerAI.MyState == PlayerState.BatsMan) {
                GameManager.Instance.PowerupGameblerRunMultyplierActive(runMultiPlier);
            }
            else {
                GameManager.Instance.PowerupGameblerAlloutActive();
            }

        }

        flt_CurrentTime = 0;
        this.gameObject.SetActive(true);

    }
}


