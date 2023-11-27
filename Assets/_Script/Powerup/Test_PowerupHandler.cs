using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_PowerupHandler : MonoBehaviour
{
    [SerializeField] private PowerUpType CurrentPowerUp;

    private void Update() {

        if (Input.GetKeyDown(KeyCode.Backspace)) {
            StartPlayerPowerUp();
        }
        if (Input.GetKeyDown(KeyCode.RightShift)) {
            StartPlayerAIPowerUp();
        }
       
    }

   
    private void StartPlayerAIPowerUp() {

        PowerUpManager.Instance.ActivetedPowerUp(CurrentPowerUp,false);
    }

    private void StartPlayerPowerUp() {
        PowerUpManager.Instance.ActivetedPowerUp(CurrentPowerUp, true);
    }
}
