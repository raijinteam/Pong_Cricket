using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fielder : MonoBehaviour {

    public PlayerState fielderState;
    private float flt_ballforce;

    public void SetFielderData(float _flt_Force, PlayerState _myState) {
        fielderState = _myState;
        flt_ballforce = _flt_Force;

    }

    public float GetFielderForce() {
        return flt_ballforce;
    }
}
