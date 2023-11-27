using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PowerUpManager : MonoBehaviour {


    public static PowerUpManager Instance;

    [SerializeField] private Powerup2X powerup2X;
   
    [SerializeField] private PowerUpFreeze powerUpFreeze;
    [SerializeField] private PowerUpFielder powerUpFielder;
    public PowerUpFielder PowerUpFielder { get { return powerUpFielder; } }

    [SerializeField] private PowerUpInvincible powerUpInvincible;
    public PowerUpInvincible PowerUpInvincible { get { return powerUpInvincible; } }
    [SerializeField] private PowerUpSpeedShot powerUpSpeedShot;
    public PowerUpSpeedShot PowerUpSpeedShot { get { return powerUpSpeedShot; } }

    [SerializeField] private PowerUpSlowMotion powerupSlowMotion;
    [SerializeField] private PowerUpTheWall powerUpTheWall;
    public PowerUpTheWall PowerUpTheWall { get { return powerUpTheWall; } }
    [SerializeField] private PowerUpPaddleExtenSion powerUpPaddleExtenSion;
    [SerializeField] private PowerupGambler powerupGambler;
    [SerializeField] private PowerupRandomizer powerupRandomizer;
    [SerializeField] private PowerUpPinBall powerUpPinBall;
    public PowerUpPinBall PowerUpPinBall { get { return powerUpPinBall; } }


    [SerializeField] private PowerupBallSplit powerupBallSplit;
    public PowerupBallSplit PowerupBallSplit { get { return powerupBallSplit; } }

    public PowerupRandomizer PowerupRandomizer { get { return powerupRandomizer; } }
    [SerializeField] private PowerUpBlock powerUpBlock;

    [SerializeField] private PowerupStickyShot powerupStickyShot;
    public PowerupStickyShot PowerupStickyShot { get { return powerupStickyShot; } }


    [Header("All PowerupHandler")]
    [SerializeField] private Powerup[] all_Powerup;
    private GameObject hasPlayerPowerupActiveted = null;
    private GameObject hasPlayerAIPowerupActiveted = null;
    [SerializeField] private List<Powerup> list_Spawner = new List<Powerup>();

    [SerializeField] private GameObject obj_Powerup;
    [SerializeField] private float flt_CurrentTime;
    [SerializeField] private float flt_TimeForTwoPowerupSpawn;


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

    public void ActivetedPowerUp(PowerUpType powerup,bool isplayer) {

        switch (powerup) {

            case PowerUpType.Powerup2X:
                ActivatedPowerUp2X();
                break;
            case PowerUpType.Freez:
                ActivatedPowerUpFreez(isplayer);
                break;
            case PowerUpType.Fielder:
                ActivatedPowerUpFielder(isplayer);
                break;
            case PowerUpType.Invicible:
                ActivatedPowerUpInvicible(isplayer);
                break;
            case PowerUpType.SpeedShot:
                ActivatedPowerUpSpeedShot(isplayer);
                break;
            case PowerUpType.SlowMotion:
                ActivatedPowerUpSlowMotion(isplayer);
                break;
            case PowerUpType.TheWall:
                ActivatedPowerUpTheWall(isplayer);
                break;
            case PowerUpType.PaddleExtension:
                ActivatedPowerUpPaddleExtension(isplayer);
                break;
            case PowerUpType.BallSplit:
                ActivatedPowerUpBallSplit(isplayer);
                break;
            case PowerUpType.PinBallPaddle:
                ActivatedPowerPinBallPaddle(isplayer);
                break;
            case PowerUpType.GamblerRunner:
                ActivatedPowerUpGamblerRunner();
                break;
            case PowerUpType.Randomizer:
                ActivatedPowerUpRandomizer(isplayer);
                break;
            case PowerUpType.Block:
                ActivatedPowerUpBlock(isplayer);
                break;
            case PowerUpType.StickyShot:
                ActivatedPowerUpStickyShot(isplayer);
                break;
            default:
                break;
        }
    }

    private void ActivatedPowerUpStickyShot(bool isplayer) {
        powerupStickyShot.ActivateStickyShotPowerUp(isplayer);
    }

    private void ActivatedPowerPinBallPaddle(bool isplayer) {
        Debug.Log("PinBallPaddle  Powerup Active");
        powerUpPinBall.ActivatePinBallPaddlePowerUp(isplayer);
    }
    private void ActivatedPowerUpBallSplit(bool isplayer) {
        Debug.Log("BallSplit  Powerup Active");
        powerupBallSplit.ActivatedBallSplitPowerUp(isplayer);
    }

    private void ActivatedPowerUpTheWall(bool isplayer) {
        Debug.Log("TheWall  Powerup Active");
        powerUpTheWall.ActivatedTheWallPowerUp(isplayer);
    }

    private void ActivatedPowerUpInvicible(bool isplayer) {
        Debug.Log("Invisible  Powerup Active");
        powerUpInvincible.ActivatedInvicliblePowerUp(isplayer);
    }

    private void ActivatedPowerUpBlock(bool isplayer) {
        Debug.Log("PowerUpBlock Powerup Active");
        powerUpBlock.ActivateBlockPowerUp(isplayer);
    }

    private void ActivatedPowerUpRandomizer(bool isplayer) {
        Debug.Log("PowerUpRandomizer Powerup Active");
        powerupRandomizer.ActivateRandomizerPowerUp(isplayer);
    }

    private void ActivatedPowerUpGamblerRunner() {
        Debug.Log("PowerUpGambler Powerup Active");
        powerupGambler.ActivateGamblerRunnerPowerUp();
    }

    private void ActivatedPowerUpPaddleExtension(bool isplayer) {
        Debug.Log("PaddleExtension Powerup Active");
        powerUpPaddleExtenSion.ActivatePaddleExtensionPowerUp(isplayer);
    }

    private void ActivatedPowerUpSlowMotion(bool isplayer) {
        Debug.Log("SlowMotion Powerup Active");
        powerupSlowMotion.ActivateSlowMotionPowerUp(isplayer);
    }

    private void ActivatedPowerUpSpeedShot(bool isplayer) {
        Debug.Log("SpeedShot Powerup Active");
        powerUpSpeedShot.ActivateSpeedShotPowerUp(isplayer);
    }

    private void ActivatedPowerUpFielder(bool isplayer) {
        powerUpFielder.ActivateFielderPowerUp(isplayer);
    }

    private void ActivatedPowerUpFreez(bool isplayer) {
        Debug.Log("Freez Powerup Active");
        powerUpFreeze.ActivateFreezePowerUp(isplayer);
    }

    private void ActivatedPowerUp2X() {
        Debug.Log("Powerup 2x Active");
        powerup2X.Activate2XPowerUp();
    }


    public void InningsChanged() {

        if (powerup2X.gameObject.activeSelf) {
            powerup2X.DeActivePower();
        }
        if (powerUpFreeze.gameObject.activeSelf) {
            powerUpFreeze.DeActivePower();
        }
        if (powerUpFielder.gameObject.activeSelf) {
            powerUpFielder.DeActivePower();
        }
        if (powerUpInvincible.gameObject.activeSelf) {
            powerUpInvincible.DeActivePower();
        }
        if (powerUpSpeedShot.gameObject.activeSelf) {
            powerUpSpeedShot.DeActivePower();
        }
        if (powerupSlowMotion.gameObject.activeSelf) {
            powerupSlowMotion.DeActivePower();
        }
        if (powerUpTheWall.gameObject.activeSelf) {
            powerUpTheWall.DeActivePower();
        }
        if (powerUpPaddleExtenSion.gameObject.activeSelf) {
            powerUpPaddleExtenSion.DeActivePower();
        }
        if (powerupBallSplit.gameObject.activeSelf) {
            powerupBallSplit.DeActivePower();
        }
        if (powerUpPinBall.gameObject.activeSelf) {
            powerUpPinBall.DeActivePower();
        }
        if (powerupGambler.gameObject.activeSelf) {
            powerupGambler.DeActivePower();
        }
        if (powerupRandomizer.gameObject.activeSelf) {
            powerupRandomizer.DeActivePower();
        }
        if (powerUpBlock.gameObject.activeSelf) {
            powerUpBlock.DeActivePower();
        }
        
    }

    

}
