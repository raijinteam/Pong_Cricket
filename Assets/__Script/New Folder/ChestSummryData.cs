using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestSummryData : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI txt_ChestValue;
    [SerializeField] private TextMeshProUGUI txt_ChestName;
    [SerializeField] private Image img_ChestIcone;
    [SerializeField] private Image img_ChestBg;


    public void SetChestSummryPanel(string _ChestValue, string ChestName, Sprite _ChestSprite, Sprite _raretySprite) {


        txt_ChestName.text = ChestName;
        txt_ChestValue.text = _ChestValue;
        img_ChestIcone.sprite = _ChestSprite;
        img_ChestBg.sprite = _raretySprite;
    }
}

