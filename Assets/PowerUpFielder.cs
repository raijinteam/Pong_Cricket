using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpFielder : MonoBehaviour {

    [SerializeField] private float flt_ActiveTime = 5;  // Max Time To Run PowerUp
    [SerializeField] private int  noof_Spawn = 1;  //Max No Of Spawn Fielder
    [SerializeField] private float flt_Force;
    private float flt_CurrentTime;  //Current Runing Time
    private bool hasPlayerActivatedPowerup;


    private void Update() {
        if (!GameManager.Instance.IsGameStart) {
            return;
        }
        PowerUpTimeCalculation();
    }




    // End Of PowerUp Precedure
    public void DeActivePower() {

        if (hasPlayerActivatedPowerup) {
            GameManager.Instance.CurrentGamePlayer.DestroyFielder();

        }
        else {

            GameManager.Instance.CurrentGamePlayerAI.DestroyFielder();
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
    public void ActivateFielderPowerUp(bool isplayer) {

        //My Side Spawn Fielder
        if (isplayer) {

            GameManager.Instance.CurrentGamePlayer.SpawnFielder(flt_Force, noof_Spawn);

        }
        else {

            GameManager.Instance.CurrentGamePlayerAI.SpawnFielder(flt_Force, noof_Spawn);

        }

        hasPlayerActivatedPowerup = isplayer;
        flt_CurrentTime = 0;
        this.gameObject.SetActive(true);

    }
}
