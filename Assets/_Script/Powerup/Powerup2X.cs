using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup2X : Powerup {

   
    
    private float flt_ActiveTime = 5;  // Max Time To Run PowerUp
    private float flt_CurrentTime;  //Current Runing Time
   


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
       
        GameManager.Instance.Powerup2XDeActived();
        this.gameObject.SetActive(false);
    }

    // This Powerup Runing Only For BatsMan
    public void Activate2XPowerUp() {

        flt_CurrentTime = 0;
        GameManager.Instance.Powerup2XActive();
        this.gameObject.SetActive(true);

    }


}
