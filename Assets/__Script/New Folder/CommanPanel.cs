using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommanPanel : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI txt_SkipIts;
    [SerializeField] private TextMeshProUGUI txt_Gold;
    [SerializeField] private TextMeshProUGUI txt_Gems;

    [Header("Player Profile")]
    [SerializeField] private TextMeshProUGUI txt_PlayerName;
    // [SerializeField] private Slider slider_PlayerLevel;
    [SerializeField] private TextMeshProUGUI txt_Slidervalue;
    [SerializeField] private Image img_PlayerIcone;
    [SerializeField] private TextMeshProUGUI txt_PanelLevel;


    private void OnEnable() {
        DataManager.Instance.UpDateCurrency  += SetCommanrPanel;
        SetCommanrPanel();
    }

    private void OnDisable() {
        DataManager.Instance.UpDateCurrency -= SetCommanrPanel;
    }

    private void SetCommanrPanel() {
        txt_PlayerName.text = DataManager.Instance.playerName;
        //slider_PlayerLevel.maxValue = DataManager.Instance.nextLevelUnlocked;
        //slider_PlayerLevel.value = DataManager.Instance.currentValue;
        txt_Slidervalue.text = DataManager.Instance.currentValue.ToString();
        txt_PanelLevel.text = DataManager.Instance.GameLevel.ToString();
        txt_Gems.text = DataManager.Instance.Gems.ToString();
        txt_Gold.text = DataManager.Instance.coins.ToString();
        txt_SkipIts.text = DataManager.Instance.skipIts.ToString();
    }



    public void OnClick_AddSkipIts() {

        AudioManager.insatance.PlayBtnClickSFX();
        ActvetedShopPanel(UIManager.Instance.ui_ShopScreen.flt_SkipItsPostion);

    }

    public void OnClick_AddGold() {

        AudioManager.insatance.PlayBtnClickSFX();
        ActvetedShopPanel(UIManager.Instance.ui_ShopScreen.flt_GoldPostion);

    }

    public void OnClick_AddGems() {

        AudioManager.insatance.PlayBtnClickSFX();
        ActvetedShopPanel(UIManager.Instance.ui_ShopScreen.flt_GemsPostion);
    }

    private void ActvetedShopPanel(float postion) {
        UIManager.Instance.ui_MenuSelectionScreen.OnClick_Menu(0);
        UIManager.Instance.ui_ShopScreen.gameObject.SetActive(true);
        UIManager.Instance.ui_ShopScreen.SetPostion(postion);
        UIManager.Instance.ui_HomeScreen.gameObject.SetActive(false);
      
    }
}
