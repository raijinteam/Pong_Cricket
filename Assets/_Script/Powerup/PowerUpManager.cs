using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PowerUpManager : MonoBehaviour {


    public static PowerUpManager Instance;

   
    [field: SerializeField] public PowerUpFielder PowerUpFielder { get; private set; }
   
    [field: SerializeField] public PowerUpSpeedShot PowerUpSpeedShot { get; private set; }
 
    [field: SerializeField] public PowerUpTheWall PowerUpTheWall { get; private set; }
    [field: SerializeField] public PowerupRandomizer PowerupRandomizer { get; private set; }
    [field: SerializeField] public PowerUpPinBall PowerUpPinBall { get; private set; }
    [field: SerializeField] public PowerupBallSplit PowerupBallSplit { get; private set; }
 

   
  


    [Header("All PowerupHandler")]
    [SerializeField] private Powerup[] all_Powerup;
    private GameObject hasPlayerPowerupActiveted = null;
    private GameObject hasPlayerAIPowerupActiveted = null;
    [SerializeField] private List<Powerup> list_Spawner = new List<Powerup>();

    [SerializeField] private GameObject obj_Powerup;
    [SerializeField] private float flt_CurrentTime;
    [SerializeField] private float flt_TimeForTwoPowerupSpawn;

    public Action<AbilityType, bool> ActivetedPower;
    public Action DeactvetedPowerup;
    public Action<int> boundryBonusActiveted;
    public Action boundryBonusDeActiveted;




    private void Start() {
        for (int i = 0; i < all_Powerup.Length; i++) {
            all_Powerup[i].gameObject.SetActive(true);
        }
    }


    private void Update() {

        //if (!GameManager.Instance.IsGameRunning) {
        //    return;
        //}
        //flt_CurrentTime += Time.deltaTime;
        //if (flt_CurrentTime >= flt_TimeForTwoPowerupSpawn) {
        //    SpawnPowerup();
        //    flt_CurrentTime = 0;
        //}
    }

    private void SpawnPowerup() {
        Instantiate(obj_Powerup, new Vector3(Random.Range(-4, 4), Random.Range(-6, 6), 0), Quaternion.identity);
    }

    public void PowerupCollected(bool isPlayer, PlayerState _currentState,GameObject Powerup) {

        if (isPlayer) {

            if (hasPlayerPowerupActiveted != null) {
                if (!hasPlayerPowerupActiveted.gameObject.activeSelf) {
                    hasPlayerPowerupActiveted = null;
                }
                else {
                    return;
                }
            }

        }
        else {

            if (hasPlayerAIPowerupActiveted != null) {
                if (!hasPlayerAIPowerupActiveted.gameObject.activeSelf) {
                    hasPlayerAIPowerupActiveted = null;
                }
                else {
                    return;
                }
            }
        }

        Destroy(Powerup);
        SpawnPowerup(isPlayer, _currentState);
    }

    private void SpawnPowerup(bool _IsPlayerCollected, PlayerState _currentState) {
        list_Spawner.Clear();
       
        for (int i = 0; i < all_Powerup.Length; i++) {

            if (all_Powerup[i].gameObject.activeSelf) {
                continue;
            }
            else if (all_Powerup[i].PowerupStatus == MyPowerUp.Both) {
                list_Spawner.Add(all_Powerup[i]);
            }
            else if(_currentState == PlayerState.BatsMan) {
                list_Spawner.Add(all_Powerup[i]);
            }
            else if (_currentState == PlayerState.Bowler) {
                list_Spawner.Add(all_Powerup[i]);
            }
        }

        int Index = Random.Range(0, list_Spawner.Count); 
        ActivetedPowerUp(list_Spawner[Index].myType, _IsPlayerCollected);
        if (_IsPlayerCollected) {
            hasPlayerPowerupActiveted = list_Spawner[Index].gameObject;
        }
        else {
            hasPlayerAIPowerupActiveted = list_Spawner[Index].gameObject;
        }
    }

    private void Awake() {
        Instance = this;
    }

    public void ActivetedPowerUp(AbilityType powerup,bool isplayer) {

        if (isplayer) DailyTaskManager.ActvetedPowerup?.Invoke();
        ActivetedPower?.Invoke(powerup, isplayer);
    }

    public void InningsChanged() {

        DeactvetedPowerup?.Invoke();
        
    }

}
