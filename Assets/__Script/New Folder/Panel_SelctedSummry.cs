using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class Panel_SelctedSummry : MonoBehaviour {

    [SerializeField] private Image img_Icone;
    [SerializeField] private TextMeshProUGUI txt_IconeName;
    [SerializeField] private TextMeshProUGUI txt_CurrentLevel;
    [SerializeField] private TextMeshProUGUI txt_OldValue;
    [SerializeField] private TextMeshProUGUI txt_NewValue;
    [SerializeField] private Button btn_tapContinue;
    [SerializeField] private RectTransform[] all_Rectransform;
    [SerializeField] private float flt_AnimationTime;

    public void ActivtedSummry(int selctedSprite) {

        this.gameObject.SetActive(true);
        img_Icone.sprite = AbilityManager.Instance.GetAbilityIcon(selctedSprite);
        int level = AbilityManager.Instance.GetAbilityCurrentLevel(selctedSprite);
        txt_IconeName.text = AbilityManager.Instance.GetAbilityName(selctedSprite);
        txt_CurrentLevel.text = "CurrentLevel: " + (level + 1);

       
        if (level == 0) {
            txt_OldValue.text = "0";
            txt_NewValue.text = AbilityManager.Instance.GetAbliltyData(selctedSprite).all_PropertyOneValues[level].ToString();
        }

        for (int i = 0; i < all_Rectransform.Length; i++) {
            all_Rectransform[i].transform.localScale = Vector3.zero;
        }

        StartCoroutine(SummryAnimation());
       
    }

    private IEnumerator SummryAnimation() {
        for (int i = 0; i < all_Rectransform.Length; i++) {
            all_Rectransform[i].DOScale(1, flt_AnimationTime);
            yield return new WaitForSeconds(flt_AnimationTime / 2);
        }

    }

    public void Onclick_OntapToContinue() {
        AudioManager.insatance.PlayBtnClickSFX();
        UIManager.Instance.ui_Abilities.CompltedSelctedSummry();
    }
}
