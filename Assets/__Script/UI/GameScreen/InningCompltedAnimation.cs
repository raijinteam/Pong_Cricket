using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InningCompltedAnimation : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI txt_Timer;
    [SerializeField] private RectTransform[] rectTransforms;
    [SerializeField] private float flt_Animation;
    [SerializeField] private float flt_PopPupTime;
    



    private void OnEnable() {

        StartCoroutine(SummaryAnimation());
    }

    private IEnumerator SummaryAnimation() {
        txt_Timer.text = "3";
        for (int i = 0; i < rectTransforms.Length; i++) {
            rectTransforms[i].localScale = Vector3.zero;
        }

        for (int i = 0; i < rectTransforms.Length; i++) {

            rectTransforms[i].DOScale(1, flt_Animation).SetEase(Ease.OutBack);
            yield return new WaitForSeconds(flt_Animation / 2);
        }
        yield return new WaitForSeconds(flt_Animation/ 2);

        for (int i = 3; i > 0; i--) {

            txt_Timer.text = i.ToString();
            yield return new WaitForSeconds(1);
        }


        for (int i = 0; i < rectTransforms.Length; i++) {

            rectTransforms[i].DOScale(0, flt_Animation/2).SetEase(Ease.OutBack);
           
        }

      
        yield return new WaitForSeconds(flt_Animation/2);
       
        GameManager.Instance.SummeryPanelShownUser();
        this.gameObject.SetActive(false);
    }
}

