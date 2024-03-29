using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPinBall : Powerup {

    [SerializeField] private float flt_ActiveTime = 5;  // Max Time To Run PowerUp
    private float flt_CurrentTime;  //Current Runing Time
    public bool hasPlayerActivatedPowerup;

    [Header("Requirment Componenet")]
    [SerializeField] private GameObject prefab_PinBallPadle;  // Prefab Pin Ball 
    [SerializeField] private Transform[] all_BatasManPostion;   // batsman spwn Postion
    [SerializeField] private Transform[] all_BowlerPostioin;    // Bower Spawn Postion
    public List<GameObject> list_PinBallPadddle;  // List Of all Spawn Pin Ball

    [Header("Paddle Data")]
    public PlayerState pinballState;  // identyfy For Which Fielder State 
    private float percentageOfForceToAdd;   // Added Force
  
    private void Update() {
        if (!GameManager.Instance.IsGameRunning) {
            return;
        }
        if (!isPowerupActive) {
            return;
        }
        PowerUpTimeCalculation();
    }

    private void PowerUpTimeCalculation() {

        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime > flt_ActiveTime) {

            DeActivtedMyPowerup();
        }
    }

    private void SpawnPinBallPaddle(PlayerState _MyState, bool isPLayer) {


        // Spawn Pin Ball Paddle And get Data
        // Add List
        hasPlayerActivatedPowerup = isPLayer;

        Transform[] all_SpawnPositions = new Transform[all_BatasManPostion.Length];

        for (int i = 0; i < all_BatasManPostion.Length; i++) {

            if (_MyState == PlayerState.BatsMan) {
                all_SpawnPositions[i] = all_BatasManPostion[i];
            }
            else {
                all_SpawnPositions[i] = all_BowlerPostioin[i];
            }

            GameObject current = Instantiate(prefab_PinBallPadle, all_SpawnPositions[i].position, all_SpawnPositions[i].rotation);
            list_PinBallPadddle.Add(current);
        }
      
        pinballState = _MyState;
    }




   

   



    public float GetPinBallForce(float _BallForece) {
        float currentForce = _BallForece + (0.01f * _BallForece * percentageOfForceToAdd);
        return currentForce;
    }

    public bool CanHitBowl(bool _isHitByBatsman) {

        if (pinballState == PlayerState.BatsMan && _isHitByBatsman) {
            return false;
        }
        else if (pinballState == PlayerState.Bowler && !_isHitByBatsman) {
            return false;
        }

        return true;

    }

    public override void ActivtedMyPowerup(AbilityType type, bool Isplayer) {
        if (myType != type) {
            return;
        }

        int index = AbilityManager.Instance.GetAbilityCurrentLevelWithType(myType);
        flt_ActiveTime = AbilityManager.Instance.GetAbliltyData(myType).all_PropertyOneValues[index];
        percentageOfForceToAdd = AbilityManager.Instance.GetAbliltyData(myType).all_PropertyTwoValues[index];
        isPowerupActive = true;
        hasPlayerActivatedPowerup = Isplayer;
        //My Side Spawn Fielder
        if (Isplayer) {

            SpawnPinBallPaddle(GameManager.Instance.CurrentGamePlayer.MyState, Isplayer);

        }
        else {

            SpawnPinBallPaddle(GameManager.Instance.CurrentGamePlayerAI.MyState, Isplayer);

        }

        flt_CurrentTime = 0;
    }

    public override void DeActivtedMyPowerup() {
        isPowerupActive = false;
        // Romove In list 
        // Destroyed PinBall Paddle
        // Active false
        for (int i = 0; i < list_PinBallPadddle.Count; i++) {
            Destroy(list_PinBallPadddle[i].gameObject);
        }
        list_PinBallPadddle.Clear();
        
    }
}
