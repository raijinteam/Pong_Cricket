using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fielder : MonoBehaviour {

    public PlayerState fielderState;  // identyfy For Which Fielder State 
    private float flt_ballforce;   // Applying Force
    private bool isInvicilbleActiveted;  // IsINvibleSctiveted
    private bool isPLayerFielder;

    public void SetFielderData(float _flt_Force, PlayerState _myState , bool isPLayer) {
        fielderState = _myState;
        flt_ballforce = _flt_Force;
        isPLayerFielder = true;

    }

    public float GetFielderForce() {
        if (isInvicilbleActiveted) {
            GameManager.Instance.CurrentGameBall.GetComponent<BallMovment>().DisableBall(isPLayerFielder);
            PowerUpManager.Instance.PowerUpInvincible.IncreaseNumberOfShots();
        }
        return flt_ballforce;
    }

    public void SetInviclble() {
        isInvicilbleActiveted = true;
    }
    public void SetDeisbleInviclble() {
        isInvicilbleActiveted = false;
    }
}
