using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PowerupRandomizer : MonoBehaviour {

    [SerializeField] private float flt_ActiveTime = 5;  // Max Time To Run PowerUp
    [SerializeField] private int scoreSpotCount;  // No Of Spawn Score Count
    private float flt_CurrentTime;  //Current Runing Time

    [Header("Spawner")]
    [SerializeField] private ScorePoint prefab_ScorePoint;
    private List<ScorePoint> list_ScorePoinInScreen = new List<ScorePoint>();
    private float flt_MinXpostion;
    private float flt_maxXpostion;
    private float flt_MinYPostion;
    private float flt_MaxYPostion;


   
    private void SetPostion() {
        float aspectRation = (float)Screen.width / Screen.height;
        float cameraHeight = Camera.main.orthographicSize* 2;
        float cameraWidth = aspectRation * cameraHeight;
        flt_MinXpostion = (-cameraWidth / 2) + 1;
        flt_maxXpostion = (cameraWidth / 2) - 1;
        flt_MinYPostion = -(cameraHeight / 2) + 3.5f;
        flt_MaxYPostion = (cameraHeight / 2) - 3.5f;
    }

    private void Update() {
        if (!GameManager.Instance.IsGameStart) {
            return;
        }
        PowerUpTimeCalculation();
    }




    // End Of PowerUp Precedure
    public void DeActivePower() {
        Debug.Log("RandomizerDeactvated");
        for (int i = 0; i < list_ScorePoinInScreen.Count; i++) {
            Destroy(list_ScorePoinInScreen[i].gameObject);
        }
        list_ScorePoinInScreen.Clear();
        this.gameObject.SetActive(false);
    }


    public void DestroyPoint(ScorePoint current) {
        list_ScorePoinInScreen.Remove(current);
        Destroy(current.gameObject);
    }


    private void PowerUpTimeCalculation() {

        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime > flt_ActiveTime) {

            DeActivePower();
        }
    }


    // This Powerup Work Both
    public void ActivateRandomizerPowerUp(bool isplayer) {
        this.gameObject.SetActive(true);
        SetPostion();
        list_ScorePoinInScreen.Clear();
        if (isplayer) {

            if (GameManager.Instance.CurrentGamePlayer.MyState == PlayerState.BatsMan) {
                SapwnScorePoint(true);
            }
            else {
                SapwnScorePoint(false);
            }

        }
        else {

            if (GameManager.Instance.CurrentGamePlayerAI.MyState == PlayerState.BatsMan) {
                SapwnScorePoint(true);
            }
            else {
                SapwnScorePoint(false);
            }

        }

        flt_CurrentTime = 0;
        

    }

    private void SapwnScorePoint(bool _IsPostive) {

        for (int i = 0; i < scoreSpotCount; i++) {
            ScorePoint current = Instantiate(prefab_ScorePoint, GetRandomPostion(), Quaternion.identity);
            list_ScorePoinInScreen.Add(current);
            current.SetData(GetRandomValue(_IsPostive));
        }
    }

    private int GetRandomValue(bool isPostive) {
        int index = Random.Range(0, 4);
        int MyValue = 0;
        switch (index) {
            case 0:
                MyValue = 1;
                break;
            case 1:
                MyValue = 2;
                break;
            case 2:
                MyValue = 4;
                break;
            default:
                MyValue = 6;
                break;
        }
        if (!isPostive) {
            MyValue = -MyValue;
        }

        return MyValue;
    }

    private Vector3 GetRandomPostion() {
        float x = Random.Range(flt_MinXpostion, flt_maxXpostion);
        float y = Random.Range(flt_MinYPostion, flt_MaxYPostion);
        return new Vector3(x, y, 0);
    }
}
