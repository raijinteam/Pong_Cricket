using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class GameScreenUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI txt_Score;
    [SerializeField] private TextMeshProUGUI txt_Name;
    [SerializeField] private TextMeshProUGUI txt_GameStatus;
    [SerializeField] private GameObject obj_ShowSummryPanel;
    [SerializeField] private TextMeshProUGUI txt_SummeryText;

    public void SetScore(int Run,int Wicket) {
        txt_Score.text = Run + "/" + Wicket;
    }
    public void SetPlayerName(string name ) {
        txt_Name.text = name;
    }

    public void ShowSummeryScreen(float flt_SummeryTime,string BatsManTeamn,string BowlwerTeam, int GameRun) {
        obj_ShowSummryPanel.gameObject.SetActive(true);
        txt_SummeryText.text = BatsManTeamn + " Makes " + GameRun + " Run  Now " + BowlwerTeam + " want " + GameRun + " run ";
       
    }
}
