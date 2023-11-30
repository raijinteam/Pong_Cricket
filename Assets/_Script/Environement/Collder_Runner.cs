using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collder_Runner : MonoBehaviour
{
    [Header("Data")]
    [SerializeField]private bool isMaxRun;
    [SerializeField] private int runValue;
    [SerializeField] private float flt_PersantageScaleValue;
    [SerializeField] private float flt_PersantageOffest;
    [SerializeField] private int currentRunValue;

    private BoxCollider2D myBoxCollider;
    [SerializeField] private GameObject body;

    private float flt_Height;

    private bool isBlocked;

    // Property
    public int MyRunValue {

        get {
            if (isBlocked) {
                return 0;
            }
            else {
                return runValue;
            }  
        } 
    }


   

    public void ActivetedBlock() {
        body.gameObject.SetActive(true);
        isBlocked = true;
    }
    public void DeActivetedBlock() {
        body.gameObject.SetActive(false);
        isBlocked = false;
    }
}
