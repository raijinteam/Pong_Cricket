using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpInvincible : Powerup {

    [SerializeField] private float flt_ActiveTime;   // Max Time
    private float flt_CurrentTime;

    private void Update() {
        if (!GameManager.Instance.IsGameRunning) {
            return;
        }
        TimeHandlerInviciblePowerUp();
    }

    private void TimeHandlerInviciblePowerUp() {
        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime > flt_ActiveTime) {
            DeActivePower();
        }
    }


    public void DeActivePower() {

        GameManager.Instance.ballMovement.DisableInvisiblePowerup();
        this.gameObject.SetActive(false);
    }

    // This Powerup Work Both
    public void ActivatedInvicliblePowerUp(bool _hasTakenByPlayer) {

        flt_CurrentTime = 0;
        this.gameObject.SetActive(true);

        GameManager.Instance.ballMovement.ActivateInvisiblePowerup(_hasTakenByPlayer);
    }

}
