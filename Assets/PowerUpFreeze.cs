using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpFreeze : MonoBehaviour {

    [SerializeField]private float flt_ActiveTime = 5;  // Max Time To Run PowerUp
    [SerializeField] private float flt_FreezTime = 1;  //Its FreezTime When Player Stop Moveing
    [SerializeField] private float flt_IntervalTime = 0.5f;   // Its InterVal Time When PLayer Move ;
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
          
            GameManager.Instance.CurrentGamePlayerAI.DeActivateFreezePowerup();
        }
        else {
            
            GameManager.Instance.CurrentGamePlayer.DeActivateFreezePowerup();
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
    public void ActivateFreezePowerUp(bool isplayer) {

        //Oppsotite Player Freeez
        if (isplayer) {

            GameManager.Instance.CurrentGamePlayerAI.ActivateFreezePowerup(flt_FreezTime, flt_IntervalTime);
           
        }
        else {

             GameManager.Instance.CurrentGamePlayer.ActivateFreezePowerup(flt_FreezTime, flt_IntervalTime);
            
        }

        hasPlayerActivatedPowerup = isplayer;
        flt_CurrentTime = 0;
        this.gameObject.SetActive(true);
        
    }
}
