using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panel_SerachingPlayer : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI txt_PlayerName;
    [SerializeField] private TextMeshProUGUI txt_LevelPrice;
    [SerializeField] private Image imgPlayer;


    [Header("Opnononet")]
    [SerializeField] private SlotMachine slotmcahine;

    private void OnEnable() {

        //txt_PlayerName.text = DataManager.Instance.playerName;
        //txt_LevelPrice.text = LevelManager.Instance.GetLevelEntryFee(LevelManager.Instance.currentLevelIndex).ToString();
        slotmcahine.StartSpinng();
    }
}
