using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupBallSplit : Powerup {

    [SerializeField] private int NoOfBall;
    [SerializeField] private float flt_ActiveTime;
    [SerializeField] private SmallBallMotion prefab_SmallBall;
    private float flt_CurrentTime;
    public List<SmallBallMotion> list_ActivaterBall;

    




    private void Update() {
        if (!GameManager.Instance.IsGameRunning) {
            return;
        }
        if (!isPowerupActive) {
            return;
        }
       flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime > flt_ActiveTime) {
            DeActivtedMyPowerup();
        }
    }

    


   
  
   



    private void spawnBall(PlayerState myState) {

        for (int i = 0; i < NoOfBall; i++) {

            if (myState == PlayerState.BatsMan) {
                SmallBallMotion current = Instantiate(prefab_SmallBall, transform.position + new Vector3(Random.Range(-5, 5), -1, 0), transform.rotation);
                current.SetRandomVelocityOfBall(new Vector3(Random.Range(-3, 3), -3, 0));
                list_ActivaterBall.Add(current);
            }
            else {
                SmallBallMotion current = Instantiate(prefab_SmallBall, transform.position + new Vector3(Random.Range(-5, 5), 1, 0), transform.rotation);
                current.SetRandomVelocityOfBall(new Vector3(Random.Range(-3, 3), 3, 0));
                list_ActivaterBall.Add(current);
            }

        }
    }

    public override void ActivtedMyPowerup(AbilityType type, bool Isplayer) {

        if (myType != type) {
            return;
        }

        int index = AbilityManager.Instance.GetAbilityCurrentLevelWithType(myType);
        flt_ActiveTime = AbilityManager.Instance.GetAbliltyData(myType).all_PropertyOneValues[index];
        NoOfBall = ((int)AbilityManager.Instance.GetAbliltyData(myType).all_PropertyTwoValues[index]);
        //Player Shot Increased Ammount
        if (Isplayer) {
            spawnBall(GameManager.Instance.CurrentGamePlayer.MyState);
        }
        else {
            spawnBall(GameManager.Instance.CurrentGamePlayerAI.MyState);
        }
        isPowerupActive = true;
    }

    public override void DeActivtedMyPowerup() {
        Debug.Log("RandomizerDeactvated");
        for (int i = 0; i < list_ActivaterBall.Count; i++) {
            Destroy(list_ActivaterBall[i].gameObject);
        }
        ;list_ActivaterBall.Clear();
        isPowerupActive = false;
    }
}

   
