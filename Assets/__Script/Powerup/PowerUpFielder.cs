using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpFielder : Powerup {

    [SerializeField] private GameObject prefab_Fielder;
    [SerializeField] private float flt_ActiveTime = 5;  // Max Time To Run PowerUp
    [SerializeField] private int  noof_Spawn = 1;  //Max No Of Spawn Fielder
    [SerializeField] private float flt_Force;
    private float flt_CurrentTime;  //Current Runing Time
    public bool hasPlayerActivatedPowerup;

    [Header("ACTIVE FIELDER DATA")]
    public PlayerState fielderState;  // identyfy For Which Fielder State 

    public override void ActivtedMyPowerup(AbilityType type, bool Isplayer) {
        if (myType != type) {
            return;
        }

        int index = AbilityManager.Instance.GetAbilityCurrentLevelWithType(myType);
        flt_ActiveTime = AbilityManager.Instance.GetAbliltyData(myType).all_PropertyOneValues[index];
        noof_Spawn = ((int)AbilityManager.Instance.GetAbliltyData(myType).all_PropertyTwoValues[index]);
        isPowerupActive = true;

        //My Side Spawn Fielder     
        if (Isplayer) {

            // GameManager.Instance.CurrentGamePlayer.SpawnFielder(flt_Force, noof_Spawn);        
            fielderState = GameManager.Instance.CurrentGamePlayer.MyState;
        }
        else {

            fielderState = GameManager.Instance.CurrentGamePlayerAI.MyState;
        }
        SpawnFielder();

        hasPlayerActivatedPowerup = Isplayer;
        flt_CurrentTime = 0;
    }

    public override void DeActivtedMyPowerup() {
        isPowerupActive = false;

        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
    }

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



   


   


   

    private void SpawnFielder() {
       
        for (int i = 0; i < noof_Spawn; i++) {

            GameObject Current = Instantiate(prefab_Fielder, GetRandomPosition(i), Quaternion.identity,transform);
           
            Current.transform.localEulerAngles = GetRotation(Current.transform);       
        }
    }


    

    public float GetFielderForce() {

        return flt_Force;
    }

    public bool CanHitBowl(bool _isHitByBatsman) {

        if (fielderState == PlayerState.BatsMan && _isHitByBatsman) {
            return false;
        }
        else if (fielderState == PlayerState.Bowler && !_isHitByBatsman) {
            return false;
        }

        return true;

    }

    private Vector3 GetRandomPosition(int i) {

        float minX_Postion = 0;
        float minY_Postion = 0;
        float maxX_Postion = 0;
        float maxY_Postion = 0;

      
        float x = 0;

        float AspectRatio = (float)Screen.width / Screen.height;
        float CameraHeight = Camera.main.orthographicSize * 2;
        float CamerWidth = AspectRatio * CameraHeight - 2;

        minX_Postion = (-CamerWidth / 2) + 1;
        maxX_Postion = CamerWidth / 2 - 1;

       
        if (fielderState == PlayerState.BatsMan) {
            minY_Postion = 0;
            maxY_Postion = (CameraHeight / 2) - 3;
        }
        else {
            minY_Postion = (-CameraHeight / 2) + 3;
            maxY_Postion = 0;
        }

        if (i == 0) {
            x = Random.Range(minX_Postion, minX_Postion + CamerWidth / 3);
        }
        else if (i == 1) {
            x = Random.Range(minX_Postion + CamerWidth / 3,maxX_Postion -  (CamerWidth / 3));
        }
        else {
            x = Random.Range((maxX_Postion - CamerWidth / 3), maxX_Postion);
        }

       
        float y = Random.Range(minY_Postion, maxY_Postion);
        //Debug.Log(new Vector3(x, y, 0) + "SetVale");
        return new Vector3(x, y, 0);

    }

    private Vector3 GetRotation(Transform _Fielder) {

        Debug.Log("X" + transform.position.x);
        Vector3 SetEuler = new Vector3();
        if (fielderState == PlayerState.BatsMan) {
            if (_Fielder.position.x <=-2) {
                SetEuler = new Vector3(0, 0, Random.Range(30, 45));
            }
            else if (_Fielder.position.x > -2 && _Fielder.position.x < 2) {
                SetEuler = new Vector3(0, 0, Random.Range(-10, 10));
            }
            else if (_Fielder.position.x >= 2) {
                SetEuler = new Vector3(0, 0, Random.Range(-30, -45));
            } 
        }
        else {
            if (_Fielder.position.x <= -2) {
                SetEuler = new Vector3(0, 0, Random.Range(-30, -45));
            }
            else if (_Fielder.position.x > -2 && _Fielder.position.x < 2) {
                SetEuler = new Vector3(0, 0, Random.Range(-10, 10));
            }
            else if (_Fielder.position.x >= 2) {
                SetEuler = new Vector3(0, 0, Random.Range(30, 45));
            } 
        }
        Debug.Log(SetEuler + "AngleFielder");
        return SetEuler;
    }

  
}
