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


    private void Update() {
        if (isMaxRun) {
            return;
        }

        myBoxCollider = GetComponent<BoxCollider2D>();
        flt_Height = Camera.main.orthographicSize * 2;

        //myBoxCollider.size = new Vector2(1, flt_Height * 0.01f * flt_PersantageScaleValue);
        //myBoxCollider.offset = new Vector2(0, flt_Height * 0.01f * flt_PersantageOffest);

        transform.localScale = new Vector3(0.5f, flt_Height * 0.01f * flt_PersantageScaleValue,1);
        transform.localPosition = new Vector3(transform.localPosition.x, flt_Height * 0.01f * flt_PersantageOffest,transform.localPosition.z);
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
