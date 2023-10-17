using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI txt_GameResult;


    public void SetResult(string message) {
        txt_GameResult.text = message;
    }
    public void OnClick_OnReloadBtn() {
        SceneManager.LoadScene(0);
    }
}
