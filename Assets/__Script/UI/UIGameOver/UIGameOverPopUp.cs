using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;

public class UIGameOverPopUp : MonoBehaviour {

   
    [SerializeField] private RectTransform rect_PanelWin;
    [SerializeField] private RectTransform rect_PanelLoose;

    [SerializeField] private float flt_AnimationTime;
    [SerializeField] private float flt_ShownTime;


    private RectTransform target;


    private void OnEnable() {

        transform.localScale = Vector3.zero;
        target = GameManager.Instance.HasPlayerWon ? rect_PanelWin : rect_PanelLoose;
        rect_PanelLoose.gameObject.SetActive(false);
        rect_PanelWin.gameObject.SetActive(false);
       
        StartCoroutine(Delay_PopUp());
    }
    





    
    private IEnumerator Delay_PopUp() {


        yield return new WaitForSeconds(0.75f);

        target.gameObject.SetActive(true);
      
        transform.DOScale(Vector3.one, flt_AnimationTime).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(flt_ShownTime );
        if (GameManager.Instance.HasPlayerWon) {
            GameManager.Instance.wallHandler.PlayWinEffect(true);
        }
        yield return new WaitForSeconds(flt_ShownTime);

        transform.DOScale(Vector3.zero, flt_AnimationTime).SetEase(Ease.InBack);
        GameManager.Instance.CurrentGamePlayer.PlayScaleDownAnimation();
        GameManager.Instance.CurrentGamePlayerAI.PlayScaleDownAnimation();
        UIManager.Instance.ui_GameScreen.ScaleDownAnimation(flt_AnimationTime );


        yield return new WaitForSeconds(flt_AnimationTime);
        UIManager.Instance.ui_GameScreen.gameObject.SetActive(false);
        UIManager.Instance.ui_GameOver.gameObject.SetActive(true);
        GameManager.Instance.wallHandler.PlayWinEffect(false);
        this.gameObject.SetActive(false);
        
    }
}

