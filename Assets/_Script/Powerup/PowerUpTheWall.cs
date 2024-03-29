using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpTheWall : Powerup {

   
    [SerializeField] private float flt_ActiveTime; // max Time to Active Time
    private float flt_CurrrentTime;   // Curren Time for this Powerup


    public Transform currenWall;   // Current Spawn Wall
    [SerializeField] private GameObject prefab_Wall;   // Prefab_Of Wall 
    public bool hasPlayerActivatedPowerup;   // Player get This Powerup


    // InvisiblePowerup
    public bool isInvicilbleActiveted;   // Status Of Invicible Powerup


    private void Update() {
        if (!GameManager.Instance.IsGameRunning) {
            return;
        }
        if (!isPowerupActive) {
            return;
        }
        TimeCalculation();
    }
    private void TimeCalculation() {
        flt_CurrrentTime += Time.deltaTime;
        if (flt_CurrrentTime > flt_ActiveTime) {
            DeActivtedMyPowerup();
        }
    }

    private void spawnWall(PlayerState Mystate) {

        float AspectRatio = (float)Screen.width / Screen.height;
        float cameraHeight = Camera.main.orthographicSize * 2;
        float cameraWidth = AspectRatio * cameraHeight;

        currenWall = Instantiate(prefab_Wall, Vector3.zero, Quaternion.identity).transform;
        if (Mystate == PlayerState.BatsMan) {


            currenWall.localPosition = new Vector3(0, (cameraHeight / 2) - 2, 0);
            currenWall.localScale = new Vector3(cameraWidth, 1, 0);
        }
        else {

            currenWall.localPosition = new Vector3(0, (-cameraHeight / 2) + 2, 0);
            currenWall.localScale = new Vector3(cameraWidth, 1, 0);
        }
    }


  


   
    public override void ActivtedMyPowerup(AbilityType type, bool Isplayer) {
        if (myType != type) {
            return;
        }
        isPowerupActive = true;
        int index = AbilityManager.Instance.GetAbilityCurrentLevelWithType(myType);
        flt_ActiveTime = AbilityManager.Instance.GetAbliltyData(myType).all_PropertyOneValues[index];
     
        //Player Shot Increased Ammount
        if (Isplayer) {

            spawnWall(GameManager.Instance.CurrentGamePlayer.MyState);

        }
        else {

            spawnWall(GameManager.Instance.CurrentGamePlayerAI.MyState);

        }

        hasPlayerActivatedPowerup = Isplayer;
        flt_CurrrentTime = 0;
    }

    public override void DeActivtedMyPowerup() {
        isPowerupActive = false;
        if (currenWall != null) {
            Destroy(currenWall.gameObject);
        }
       

    }
}
