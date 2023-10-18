using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPaddleExtenSion : MonoBehaviour {

    [SerializeField] private float flt_ActiveTime = 5;  // Max Time To Run PowerUp
    [SerializeField] private float flt_SizeIncreasedValue = 1;  //
    private float flt_CurrentTime;  //Current Runing Time
    private bool hasPlayerActivatedPowerup;  
    private float flt_PlayerScale;


    private void Update() {
        if (!GameManager.Instance.IsGameStart) {
            return;
        }
        PowerUpTimeCalculation();
    }




    // End Of PowerUp Precedure
    private void DeActivePower() {

        if (hasPlayerActivatedPowerup) {

            GameManager.Instance.CurrentGamePlayer.transform.localScale = new Vector3(flt_PlayerScale,
              GameManager.Instance.CurrentGamePlayer.transform.localScale.y, GameManager.Instance.CurrentGamePlayer.transform.localScale.z);
        }
        else {
            GameManager.Instance.CurrentGamePlayerAI.transform.localScale = new Vector3(flt_PlayerScale,
               GameManager.Instance.CurrentGamePlayerAI.transform.localScale.y, GameManager.Instance.CurrentGamePlayerAI.transform.localScale.z);

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
    public void ActivatePaddleExtensionPowerUp(bool isplayer) {

        // Player Scale Increased
        if (isplayer) {

            flt_PlayerScale = GameManager.Instance.CurrentGamePlayer.transform.localScale.x;
            GameManager.Instance.CurrentGamePlayer.transform.localScale += new Vector3(flt_SizeIncreasedValue, 0, 0);

        }
        else {

            flt_PlayerScale = GameManager.Instance.CurrentGamePlayerAI.transform.localScale.x;
            GameManager.Instance.CurrentGamePlayerAI.transform.localScale += new Vector3(flt_SizeIncreasedValue, 0, 0);


          

        }

        hasPlayerActivatedPowerup = isplayer;
        flt_CurrentTime = 0;
        this.gameObject.SetActive(true);

    }
}
