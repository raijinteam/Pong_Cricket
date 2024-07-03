using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collder_Runner : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private Sprite sprite_Run;
    [SerializeField]private bool isMaxRun;
    [SerializeField] private int runValue;
    [SerializeField] private float flt_PersantageScaleValue;
    [SerializeField] private float flt_PersantageOffest;
    [SerializeField] private int currentRunValue;

    private BoxCollider2D myBoxCollider;
    [SerializeField] private GameObject body;


    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private SpriteRenderer sr_No;
    

    private float flt_Height;

    private bool isBlocked;
    private bool isActivtedBonus;
    private int bonus;


    private void OnEnable() {
        //PowerUpManager.Instance.boundryBonusActiveted += ActivetedBonus;
        //PowerUpManager.Instance.boundryBonusDeActiveted += DeActivetedBouns;
    }
    private void OnDisable() {
        //PowerUpManager.Instance.boundryBonusActiveted -= ActivetedBonus;
        //PowerUpManager.Instance.boundryBonusDeActiveted -= DeActivetedBouns;
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

            // testing Purpose
            //if (isMaxRun) {
            //    if (isBlocked) {
            //        return 0;
            //    }
            //    else if (isActivtedBonus) {
            //        return (runValue + bonus);
            //    }
            //    else {
            //        return runValue;
            //    }
            //}
            //else {
            //    if (GameManager.Instance.CurrentGamePlayer.MyState == PlayerState.BatsMan) {
            //        return 6;
            //    }
            //    else {
            //        return 0;
            //    }
            //}

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

    Color color_Defualt = Color.white;
    private Coroutine color_Coro;

    public void ChangeColor() {
        


        if (color_Coro == null) {
            StartCoroutine(ChangeColorCortine(color_Defualt));
        }
        
    }

    private IEnumerator ChangeColorCortine(Color color) {


        Debug.Log("Change Color");
        if (sr != null) {
            sr.gameObject.SetActive(true);
            sr.color = Color.yellow;
        }

        sr_No.gameObject.SetActive(true);

        yield return new WaitForSeconds(1);
        if (sr != null) {
            sr.color = color;
            sr.gameObject.SetActive(false);
        }
        sr_No.gameObject.SetActive(false);
        color_Coro = null;
    }

    public void ActivetedBlock() {
        body.gameObject.SetActive(true);
        isBlocked = true;
    }
    public void DeActivetedBlock() {
        body.gameObject.SetActive(false);
        isBlocked = false;
    }

    public void BallTouch() {

        if (GameManager.Instance.CurrentGamePlayer.MyState == PlayerState.Bowler) {
            return;
        }

        if (runValue == 4) {
            DailyTaskManager.boundryBlaster?.Invoke();
            DailyTaskManager.BackToBackHatTrickBoundry?.Invoke(true);
        }
        else if (runValue == 6) {
            DailyTaskManager.sixBlaster?.Invoke();
            DailyTaskManager.BackToBackHatTrickBoundry?.Invoke(true);
        }
        else if (runValue == 2) {
            DailyTaskManager.DoubleScoreget?.Invoke();
            DailyTaskManager.BackToBackHatTrickBoundry?.Invoke(false);
        }
        else if (runValue == 1) {
            DailyTaskManager.singleScoreGet?.Invoke();
            DailyTaskManager.BackToBackHatTrickBoundry?.Invoke(false);
        }
        else if (runValue == 10) {

            Debug.Log("hOME cLICK");
            DailyTaskManager.homeRunGet?.Invoke();
            DailyTaskManager.BackToBackHatTrickBoundry?.Invoke(false);
        }
    }

    public Sprite GetNoSprite() {
        return sprite_Run;
    }
}
