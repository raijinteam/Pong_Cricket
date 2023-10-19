using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBlock : MonoBehaviour {

    [SerializeField] private float flt_ActiveTime = 5;  // Max Time To Run PowerUp
    [SerializeField] private int  no_OfBlock;
    private float flt_CurrentTime;  //Current Runing Time
   


    private void Update() {
        if (!GameManager.Instance.IsGameStart) {
            return;
        }
        PowerUpTimeCalculation();
    }




    // End Of PowerUp Precedure
    public void DeActivePower() {

        GameManager.Instance.WallBlockDeActivated();
        this.gameObject.SetActive(false);
    }


    private void PowerUpTimeCalculation() {

        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime > flt_ActiveTime) {

            DeActivePower();
        }
    }


    // This Powerup Work BatsMan
    public void ActivateBlockPowerUp(bool isplayer) {

       
        if (isplayer) {

            if (GameManager.Instance.CurrentGamePlayer.MyState == PlayerState.BatsMan) {

                GameManager.Instance.WallBlockActivated(no_OfBlock);
            }
        }
        else {

            if (GameManager.Instance.CurrentGamePlayerAI.MyState == PlayerState.BatsMan) {

                GameManager.Instance.WallBlockActivated(no_OfBlock);
            }
        }
        flt_CurrentTime = 0;
        this.gameObject.SetActive(true);

    }
}
