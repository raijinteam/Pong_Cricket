using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI txt_GameResult;

    private void Start() {

        if (GameManager.Instance.HasPlayerWon) {
            DataManager.Instance.WonGame();
            ChestManager.Instance.RewardChestIfPossible();
        }
        else {
            DataManager.Instance.LoseGame();
        }
    }
    public void SetResult(string message) {
        txt_GameResult.text = message;
    }
    public void OnClick_OnReloadBtn() {
        SceneManager.LoadScene(0);
    }
}
