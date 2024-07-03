
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour
{

    [Header("PlayerData")]
    [SerializeField] private TextMeshProUGUI txt_PlayerName;
    [SerializeField] private Image img_Player;
    [SerializeField] private TextMeshProUGUI txt_PriceOfLevelPLayer;
    [SerializeField] private RectTransform obj_WinPlayer;
    [SerializeField] private RectTransform obj_LoosePlayer;

    [Header("PlayerAI")]
    [SerializeField] private TextMeshProUGUI txt_PlayerAi;
    [SerializeField] private Image img_PlayerAi;
    [SerializeField] private TextMeshProUGUI  txt_PriceofLevelPlayerAI;
    [SerializeField] private RectTransform Obj_WinPlayerAi;
    [SerializeField] private RectTransform obj_LoosePlayerAI;


   

   
   

    private void Start() {

        if (GameManager.Instance.HasPlayerWon) {
            
            DataManager.Instance.WonGame();
            ChestManager.Instance.RewardChestIfPossible();
            

        }
        else {
            DataManager.Instance.LoseGame();
          
        }

        obj_LoosePlayer.gameObject.SetActive(!GameManager.Instance.HasPlayerWon);
        obj_WinPlayer.gameObject.SetActive(GameManager.Instance.HasPlayerWon);
        Obj_WinPlayerAi.gameObject.SetActive(!GameManager.Instance.HasPlayerWon);
        obj_LoosePlayerAI.gameObject.SetActive(GameManager.Instance.HasPlayerWon);

        // set player Data
        txt_PlayerName.text = DataManager.Instance.playerName;
        img_Player.sprite = DataManager.Instance.img_PlayerSprite;
      
        txt_PriceOfLevelPLayer.text =
            LevelManager.Instance.GetLevelEntryFee(LevelManager.Instance.currentLevelIndex).ToString();


        // set Player Ai Data

        txt_PlayerAi.text = DataManager.Instance.playerNameAI;
        img_PlayerAi.sprite = DataManager.Instance.img_PlayerSpriteAi;
      

        txt_PriceOfLevelPLayer.text =
            LevelManager.Instance.GetLevelEntryFee(LevelManager.Instance.currentLevelIndex).ToString();

        DailyTaskManager.Instance.ShowTaskBar();

      
       
        AdsManager.instance.ShowInterstialAds();
        

    }
    
    public void OnClick_OnReloadBtn() {
        SceneManager.LoadScene(0);
    }
    public void OnClick_RestrartGameClick() {
        DataManager.Instance.RestratGame();
    }
}
