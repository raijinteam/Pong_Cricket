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
    private bool isActivtedBonus;
    private int bonus;


    private void OnEnable() {
        PowerUpManager.Instance.boundryBonusActiveted += ActivetedBonus;
        PowerUpManager.Instance.boundryBonusDeActiveted += DeActivetedBouns;
    }
    private void OnDisable() {
        PowerUpManager.Instance.boundryBonusActiveted -= ActivetedBonus;
        PowerUpManager.Instance.boundryBonusDeActiveted -= DeActivetedBouns;
    }


    private void ActivetedBonus(int bonus) {
        if (runValue == 4) {
            isActivtedBonus = true;
            this.bonus = bonus;
        }
        else if (runValue == 6) {
            isActivtedBonus = true;
            this.bonus = bonus;
        }
       
    }
    private void DeActivetedBouns() {
        isActivtedBonus = false;
    }

   

    // Property
    public int MyRunValue {

        get {
            if (isBlocked) {
                return 0;
            }
            else if (isActivtedBonus) {
                return (runValue + bonus);
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
