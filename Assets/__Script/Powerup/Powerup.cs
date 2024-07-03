using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Powerup : MonoBehaviour {

    [field : SerializeField]public MyPowerUp PowerupStatus { get;  set; }
    [field: SerializeField] public AbilityType myType { get; set; }
   
    protected bool isPowerupActive;

    private void OnEnable() {
        PowerUpManager.Instance.ActivetedPower += ActivtedMyPowerup;
        PowerUpManager.Instance.DeactvetedPowerup += DeActivtedMyPowerup;
    }



    private void OnDisable() {
        PowerUpManager.Instance.ActivetedPower -= ActivtedMyPowerup;
        PowerUpManager.Instance.DeactvetedPowerup -= DeActivtedMyPowerup;
    }

   public abstract void ActivtedMyPowerup(AbilityType type, bool Isplayer);

    public abstract void DeActivtedMyPowerup(); 
}

[System.Serializable]
public enum MyPowerUp {
    BatsMan,
    Bowlwer,Both,
}

