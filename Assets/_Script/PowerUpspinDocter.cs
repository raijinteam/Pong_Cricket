using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpspinDocter : Powerup {

    [SerializeField] private float flt_ActvetedTime;
    [SerializeField] private int per_SwingForce;
    [SerializeField] private bool hasPlayereActvetdPowerup;
    private float flt_CurrrentTime;

    public override void ActivtedMyPowerup(AbilityType type, bool Isplayer) {

        if (myType != type) {
            return;
        }
        isPowerupActive = true;
        flt_CurrrentTime = 0;
        int index = AbilityManager.Instance.GetAbilityCurrentLevelWithType(myType);
        flt_ActvetedTime = AbilityManager.Instance.GetAbliltyData(myType).all_PropertyOneValues[index];
        per_SwingForce = ((int)AbilityManager.Instance.GetAbliltyData(myType).all_PropertyTwoValues[index]);
        hasPlayereActvetdPowerup = Isplayer;
        if (hasPlayereActvetdPowerup) {
            GameManager.Instance.CurrentGamePlayer.ActivetedSpinDocter(per_SwingForce);
        }
        else {
            GameManager.Instance.CurrentGamePlayerAI.ActivetedSpinDocter(per_SwingForce);
        }

    }

    public override void DeActivtedMyPowerup() {
        isPowerupActive = false;

        if (hasPlayereActvetdPowerup) {
            GameManager.Instance.CurrentGamePlayer.DeActivetedSpinDocter();
        }
        else {
            GameManager.Instance.CurrentGamePlayerAI.DeActivetedSpinDocter();
        }
    }
}
