using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {

    public MyPowerUp PowerupStatus;
    public PowerUpType myType;
}

[System.Serializable]
public enum MyPowerUp {
    BatsMan,
    Bowlwer,Both,
}

