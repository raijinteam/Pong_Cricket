using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupBallSplit : Powerup {

    [SerializeField] private int NoOfBall;
    [SerializeField] private float flt_ActiveTime;
    [SerializeField] private SmallBallMotion prefab_SmallBall;
    private float flt_CurrentTime;
    public List<SmallBallMotion> list_ActivaterBall;

    



    // End Of PowerUp Precedure
    public void DeActivePower() {

        
        this.gameObject.SetActive(false);
    }

    private void Update() {
        if (!GameManager.Instance.IsGameRunning) {
            return;
        }
       flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime > flt_ActiveTime) {
            DeActivePower();
        }
    }

    // This Powerup Work Both
    public void ActivatedBallSplitPowerUp(bool isplayer) {

        //Player Shot Increased Ammount
        if (isplayer) {
            spawnBall(GameManager.Instance.CurrentGamePlayer.MyState);
        }
        else {
            spawnBall(GameManager.Instance.CurrentGamePlayerAI.MyState);
        }

       
        this.gameObject.SetActive(true);
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

    
}

   
