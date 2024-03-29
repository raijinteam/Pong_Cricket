
using UnityEngine;

public class PowerUpBoundryBonus : Powerup {

    private float flt_ActiveTime;
    private int RunIncreased;
    private float flt_CurrentTime;


    private void Update() {
        if (!isPowerupActive) {
            return;
        }
        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime >= flt_ActiveTime) {
            DeActivtedMyPowerup();
        }
    }



    public override void ActivtedMyPowerup(AbilityType type, bool Isplayer) {

        if (myType != type) {
            return;
        }
        isPowerupActive = true;
        flt_CurrentTime = 0;
        int index = AbilityManager.Instance.GetAbilityCurrentLevelWithType(myType);
        flt_ActiveTime = AbilityManager.Instance.GetAbliltyData(myType).all_PropertyOneValues[index];
        RunIncreased = ((int)AbilityManager.Instance.GetAbliltyData(myType).all_PropertyTwoValues[index]);
        PowerUpManager.Instance.boundryBonusActiveted?.Invoke(RunIncreased);
       
    }

    public override void DeActivtedMyPowerup() {

        isPowerupActive = false;
        PowerUpManager.Instance.boundryBonusDeActiveted?.Invoke();
    }
}

