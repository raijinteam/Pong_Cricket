using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panel_SerachingPlayer : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI txt_GameLevelPlayer;
    [SerializeField] private TextMeshProUGUI txt_PlayerName;
    [SerializeField] private TextMeshProUGUI txt_LevelPrice;
    [SerializeField] private Image imgPlayer;


    [Header("Opononet")]
    [SerializeField] private TextMeshProUGUI txt_GameLevelAI;
    [SerializeField] private SlotMachine slotmcahine;
    [SerializeField] private RectTransform rect_Oppnonent;
    [SerializeField] private TextMeshProUGUI txt_Player2Name;
    [SerializeField] private TextMeshProUGUI txt_LevelPrice2;

    

    private void OnEnable() {
        txt_GameLevelAI.gameObject.SetActive(false);
        txt_GameLevelPlayer.text = (DataManager.Instance.GameLevel + 1).ToString();
        imgPlayer.sprite = DataManager.Instance.img_PlayerSprite;
        txt_PlayerName.text = DataManager.Instance.playerName;
        txt_LevelPrice.text = LevelManager.Instance.GetLevelEntryFee(LevelManager.Instance.currentLevelIndex).ToString();
        slotmcahine.StartSpinng();
        rect_Oppnonent.gameObject.SetActive(false);
    }

    public void StopSlote() {

        txt_GameLevelAI.gameObject.SetActive(true);
        DataManager.Instance.SetAiLevel();
        rect_Oppnonent.gameObject.SetActive(true);
        txt_Player2Name.text = CharacterName.instance.GenerateRandomName();

        txt_LevelPrice2.text = LevelManager.Instance.GetLevelEntryFee
                                (LevelManager.Instance.currentLevelIndex).ToString();
        txt_GameLevelAI.text = (DataManager.Instance.GameLevelAI + 1).ToString();
        DataManager.Instance.playerNameAI = txt_Player2Name.text;
        DataManager.Instance.img_PlayerSpriteAi = slotmcahine.GetShownsprite();
       

        StartCoroutine(Delayof_Player());

    }

    private IEnumerator   Delayof_Player() {

        yield return new WaitForSeconds(3);
        this.gameObject.SetActive(false);
        GameManager.Instance.StartGame();
        
    }
}
