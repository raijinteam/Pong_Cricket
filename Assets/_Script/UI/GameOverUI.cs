using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour
{

    [Header("PlayerData")]
    [SerializeField] private TextMeshProUGUI txt_PlayerName;
    [SerializeField] private Image img_Player;
    [SerializeField] private TextMeshProUGUI txt_PlayerLevel;
    [SerializeField] private RectTransform rect_WinnerPlayer;

    [Header("PlayerAI")]
    [SerializeField] private TextMeshProUGUI txt_PlayerAi;
    [SerializeField] private Image img_PlayerAi;
    [SerializeField] private TextMeshProUGUI txt_PlayerAILevel;
    [SerializeField] private RectTransform rect_PlayerAiwinner;

   
   

    private void Start() {

        if (GameManager.Instance.HasPlayerWon) {
            
            DataManager.Instance.WonGame();
            ChestManager.Instance.RewardChestIfPossible();
            rect_PlayerAiwinner.gameObject.SetActive(false);
            rect_WinnerPlayer.gameObject.SetActive(true);

        }
        else {
            DataManager.Instance.LoseGame();
            rect_PlayerAiwinner.gameObject.SetActive(true);
            rect_WinnerPlayer.gameObject.SetActive(false);
        }
      

        // set player Data
        txt_PlayerName.text = DataManager.Instance.playerName;
        img_Player.sprite = DataManager.Instance.img_PlayerSprite;
        txt_PlayerLevel.text = (DataManager.Instance.GameLevel + 1).ToString();

        // set Player Ai Data

        txt_PlayerAi.text = DataManager.Instance.playerNameAI;
        img_PlayerAi.sprite = DataManager.Instance.img_PlayerSpriteAi;
        txt_PlayerAILevel.text = (DataManager.Instance.GameLevelAI + 1).ToString();

        DailyTaskManager.Instance.ShowTaskBar();
        

    }
    
    public void OnClick_OnReloadBtn() {
        SceneManager.LoadScene(0);
    }
}
