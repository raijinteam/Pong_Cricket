using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpTheWall : MonoBehaviour {

    [SerializeField] private GameObject prefab_Wall;
    [SerializeField] private int NoOfShot;   // Max Shot When PowerUp Active
    [SerializeField] private float flt_ActiveTime;
    private int CurrentShot;
    private float flt_CurrrentTime;
    private bool hasPlayerActivatedPowerup;
    private Transform currenWall;
   

    


    // End Of PowerUp Precedure
    private void DeActivePower() {

        Destroy(currenWall);
        this.gameObject.SetActive(false);
    }

   
    public void IncreaseNumberOfShots() {

        CurrentShot++;
        if (CurrentShot >= NoOfShot) {
            DeActivePower();
        }
    }
    private void Update() {
        flt_CurrrentTime += Time.deltaTime;
        if (flt_CurrrentTime > flt_ActiveTime) {
            DeActivePower();
        }
    }

    // This Powerup Work Both
    public void ActivatedTheWallPowerUp(bool isplayer) {

        
        //Player Shot Increased Ammount
        if (isplayer) {

            spawnWall(GameManager.Instance.CurrentGamePlayer.MyState);

        }
        else {

            spawnWall(GameManager.Instance.CurrentGamePlayerAI.MyState);

        }

        hasPlayerActivatedPowerup = isplayer;
        CurrentShot = 0;
        this.gameObject.SetActive(true);
    }


    private void spawnWall(PlayerState Mystate) {

        float AspectRatio = (float)Screen.width / Screen.height;
        float cameraHeight = Camera.main.orthographicSize * 2;
        float cameraWidth = AspectRatio * cameraHeight;

        Transform Current_Wall = Instantiate(prefab_Wall, Vector3.zero, Quaternion.identity).transform;
        if (Mystate == PlayerState.BatsMan) {

           
            Current_Wall.localPosition = new Vector3(0, (cameraHeight / 2) - 2, 0);
            Current_Wall.localScale = new Vector3(cameraWidth, 1, 0);
        }
        else {
           
            Current_Wall.localPosition = new Vector3(0, (-cameraHeight / 2) + 2, 0);
            Current_Wall.localScale = new Vector3(cameraWidth, 1, 0);
        }
    }

   
   

}
