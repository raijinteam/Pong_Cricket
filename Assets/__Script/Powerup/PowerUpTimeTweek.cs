using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;

public class PowerUpTimeTweek : Powerup {

    [SerializeField] private float flt_TimeReduce;


    public override void ActivtedMyPowerup(AbilityType type, bool Isplayer) {
        if (myType != type) {
            return;
        }

        int index = AbilityManager.Instance.GetAbilityCurrentLevelWithType(myType);
        flt_TimeReduce = AbilityManager.Instance.GetAbliltyData(myType).all_PropertyOneValues[index];
       
        GameManager.Instance.HandlingTimeTweek(Isplayer,flt_TimeReduce);

    }

    public override void DeActivtedMyPowerup() {
       
       
        
    }
}
