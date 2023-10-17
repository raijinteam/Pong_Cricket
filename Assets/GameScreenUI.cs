using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameScreenUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI txt_Score;
    [SerializeField] private TextMeshProUGUI txt_Name;
    [SerializeField] private TextMeshProUGUI txt_GameStatus;

    public void SetScore(int Run,int Wicket) {
        txt_Score.text = Run + "/" + Wicket;
    }
    public void SetPlayerName(string name ) {
        txt_Name.text = name;
    }
    
}
