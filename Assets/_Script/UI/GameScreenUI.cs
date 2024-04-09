using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;



public class GameScreenUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI txt_Score;
    [SerializeField] private TextMeshProUGUI txt_Wicket;
    

    [SerializeField]public GameObject obj_ShowSummryPanel;
    [SerializeField] private TextMeshProUGUI txt_SummeryText;

    [Header("PlayerData")]
    [SerializeField] private TextMeshProUGUI txt_PlayerName;
    [SerializeField] private Image img_Player;
    [SerializeField] private TextMeshProUGUI txt_PlayerLevel;

    [Header("PlayerAI")]
    [SerializeField] private TextMeshProUGUI txt_PlayerAi;
    [SerializeField] private Image img_PlayerAi;
    [SerializeField] private TextMeshProUGUI txt_PlayerAILevel;


    private void OnEnable() {

        // set player Data
        txt_PlayerName.text = DataManager.Instance.playerName;
        img_Player.sprite = DataManager.Instance.img_PlayerSprite;
        txt_PlayerLevel.text = (DataManager.Instance.GameLevel + 1).ToString();

        // set Player Ai Data

        txt_PlayerAi.text = DataManager.Instance.playerNameAI;
        img_PlayerAi.sprite = DataManager.Instance.img_PlayerSpriteAi;
        txt_PlayerAILevel.text = (DataManager.Instance.GameLevelAI + 1).ToString();
        
    }

    public void SetScore(int Run, int Wicket) {
        txt_Score.text = Run .ToString();
        txt_Wicket.text = Wicket.ToString();
    }


    public void ShowSummeryScreen(float flt_SummeryTime, string BatsManTeamn, string BowlwerTeam, int GameRun) {
        obj_ShowSummryPanel.gameObject.SetActive(true);
        txt_SummeryText.text = BatsManTeamn + " Makes " + GameRun + " Run  Now " + BowlwerTeam + " want " + GameRun + " run ";

    }


    public void SetRoationData(bool isRotate) {
      

        GameManager.Instance.CurrentGamePlayer.BtnClick(isRotate);
    }
}
