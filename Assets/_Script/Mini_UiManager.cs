using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mini_UiManager : MonoBehaviour {

    public static Mini_UiManager instance;


    [field: SerializeField] public mini_HomeScreen mini_HomeScreen { get; private set; }
    [field: SerializeField] public mini_GameInformation mini_GameInformation { get; private set; }
    [field : SerializeField] public Mini_GameScreen Mini_GameScreen { get; private set; }
    [field : SerializeField] public Mini_GameOverScreen mini_GameOver { get; private set; }
   

    private void Awake() {

        instance = this;
    }
}
