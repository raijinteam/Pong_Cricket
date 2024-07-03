using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupStickyShot : MonoBehaviour {


    [SerializeField] private float flt_ActiveTime;
    private float flt_CurrentTime;
   

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



    // This Powerup Work Both
    public void ActivateStickyShotPowerUp(bool isplayer) {

        //Player Shot Increased Ammount
        if (isplayer) {

            if (GameManager.Instance.CurrentGamePlayer.MyState == PlayerState.BatsMan) {
                flt_CurrentTime = 0;
                this.gameObject.SetActive(true);
            }

        }
        else {

            if (GameManager.Instance.CurrentGamePlayerAI.MyState == PlayerState.BatsMan) {
                flt_CurrentTime = 0;
                this.gameObject.SetActive(true);
            }

        }

    }
    // End Of PowerUp Precedure
    public void DeActivePower() {

        this.gameObject.SetActive(false);
    }
}