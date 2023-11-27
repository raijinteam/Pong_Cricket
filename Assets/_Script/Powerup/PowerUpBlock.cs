using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBlock : Powerup {

    [SerializeField] private float flt_ActiveTime = 5;  // Max Time To Run PowerUp
    [SerializeField] private int  no_OfBlock;
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

        GameManager.Instance.WallBlockDeActivated();
        this.gameObject.SetActive(false);
    }

    // This Powerup Work BatsMan
    public void ActivateBlockPowerUp(bool isplayer) {

        // Get No of Wall Block
        GameManager.Instance.WallBlockActivated(no_OfBlock);
        
        flt_CurrentTime = 0;
        this.gameObject.SetActive(true);

    }
}
