using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour {


    public static PowerUpManager Instance;

    [SerializeField] private Powerup2X powerup2X;
   
    [SerializeField] private PowerUpFreeze powerUpFreeze;
    [SerializeField] private PowerUpSpeedShot powerUpSpeedShot;
    public PowerUpSpeedShot PowerUpSpeedShot { get { return powerUpSpeedShot; } }

    [SerializeField] private PowerUpSlowMotion powerupSlowMotion;
    [SerializeField] private PowerUpPaddleExtenSion powerUpPaddleExtenSion;
    [SerializeField] private PowerupGambler powerupGambler;
    [SerializeField] private PowerupRandomizer powerupRandomizer;

    public PowerupRandomizer PowerupRandomizer { get { return powerupRandomizer; } }


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
                break;
            case PowerUpType.Invicible:
                break;
            case PowerUpType.FireBall:
                break;
            case PowerUpType.SpeedShot:
                ActivatedPowerUpSpeedShot(isplayer);
                break;
            case PowerUpType.SlowMotion:
                ActivatedPowerUpSlowMotion(isplayer);
                break;
            case PowerUpType.TheWall:
                break;
            case PowerUpType.PaddleExtension:
                ActivatedPowerUpPaddleExtension(isplayer);
                break;
            case PowerUpType.BallSplit:
                break;
            case PowerUpType.PinBallPaddle:
                break;
            case PowerUpType.GamblerRunner:
                ActivatedPowerUpGamblerRunner(isplayer);
                break;
            case PowerUpType.Randomizer:
                ActivatedPowerUpRandomizer(isplayer);
                break;
            case PowerUpType.Block:
                break;
            default:
                break;
        }
    }

    private void ActivatedPowerUpRandomizer(bool isplayer) {
        Debug.Log("PowerUpGambler Powerup Active");
        powerupRandomizer.ActivateRandomizerPowerUp(isplayer);
    }

    private void ActivatedPowerUpGamblerRunner(bool isplayer) {
        Debug.Log("PowerUpGambler Powerup Active");
        powerupGambler.ActivateGamblerRunnerPowerUp(isplayer);
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

    private void ActivatedPowerUpFreez(bool isplayer) {
        Debug.Log("Freez Powerup Active");
        powerUpFreeze.ActivateFreezePowerUp(isplayer);
    }

    private void ActivatedPowerUp2X() {
        Debug.Log(" Powerup 2x Active");
        powerup2X.Activate2XPowerUp();
    }


    public void InningsChanged() {

        if (powerup2X.gameObject.activeSelf) {
            powerup2X.DeActivePower();
        }
        // Freez PowerUp CaryForword
        // SpeedShot Powerup CaryForword
        //Powerup SlowMotion CarryForword
        // Powerup PaddleExtenSion CarryForword
        if (powerupGambler.gameObject.activeSelf) {
            powerupGambler.DeActivePower();
        }
        if (powerupRandomizer.gameObject.activeSelf) {
            powerupRandomizer.DeActivePower();
        }
    }

    

}
