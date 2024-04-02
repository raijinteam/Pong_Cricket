using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class SlotMachine : MonoBehaviour {

    [SerializeField] private RectTransform[] all_Rect;
    [SerializeField]private bool isSpining = false;
    [SerializeField] private int flt_MotionDistance;
    [SerializeField] private float flt_MotionTime;
    [SerializeField] private float flt_ChanegPostion;
    [SerializeField] private float flt_ShownPostion;
    [SerializeField] private float flt_startPostion;
    [SerializeField] private int flt_Diffrent = 1000;
    [SerializeField] private float flt_MoveStart = 50;

    [SerializeField] private int noofRound;
    
    [SerializeField] private int currentRound;
    [SerializeField] private int ShownIndex;
    [SerializeField] private bool isCompltedTotalRound;
    
    



    private void Update() {
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            StartSpinng();
        }
        
    }

    public Sprite GetShownsprite() {
        return all_Rect[ShownIndex].GetComponent<Image>().sprite;
    }

    public void StartSpinng() {
        if (isSpining) {
            return;
        }
        isSpining = true;
        currentRound = 0;
        flt_startPostion = all_Rect[ShownIndex].anchoredPosition.y;
        isCompltedTotalRound = false;

        
        ShownIndex = Random.Range(0, all_Rect.Length);
        
        StartCoroutine(MoveUpThenAnimationstart());  
       
    }

    private IEnumerator  MoveUpThenAnimationstart() {
        for (int i = 0; i < all_Rect.Length; i++) {
            all_Rect[i].DOAnchorPosY(all_Rect[i].anchoredPosition.y + flt_MoveStart, 0.3f).SetEase(Ease.OutBack); 
        }

        yield return new WaitForSeconds(0.3f);
      
        StartAnimation();
    }

    private IEnumerator DelayDownMotionAnamition() {


        yield return new WaitForEndOfFrame();

        int X = ShownIndex;
        int Value = 0;
        for (int i = 0; i < all_Rect.Length; i++) {

            all_Rect[X].DOAnchorPosY(Value, 0.75f).SetEase(Ease.OutBack);
         
            Debug.Log("SetPostion" + Value);
            Value += flt_Diffrent;
            X++;
            if (X >= all_Rect.Length) {
                X = 0;
            }

        }
       

        yield return new WaitForSeconds(0.75f);
        yield return new WaitForEndOfFrame();
        UIManager.Instance.ui_PanelSerachingPlayer.StopSlote();
        


    }

    private void StartAnimation() {
        Sequence sq = DOTween.Sequence();
       
        sq.AppendCallback(MoveAllRect).AppendInterval(flt_MotionTime).AppendCallback(CheckCondition);
            
            
    }

   

    private void MoveAllRect() {
        if (!isSpining) {
            return;
        }
        for (int i = 0; i < all_Rect.Length; i++) {
            all_Rect[i].DOAnchorPosY(all_Rect[i].anchoredPosition.y - flt_MotionDistance, flt_MotionTime).SetEase(Ease.Linear);
        }
    }
    private void CheckCondition() {


        // change Postion
        for (int i = 0; i < all_Rect.Length; i++) {
            if (all_Rect[i].anchoredPosition.y <= flt_ChanegPostion) {
                
                if ( i == 0) {

                   
                    all_Rect[i].anchoredPosition = new Vector2(all_Rect[i].anchoredPosition.x , all_Rect[all_Rect.Length - 1].anchoredPosition.y + flt_Diffrent) ;
                   
                }
                else {
                    all_Rect[i].anchoredPosition = new Vector2(all_Rect[i].anchoredPosition.x, all_Rect[i-1].anchoredPosition.y + flt_Diffrent);
                }
            }
        }

        if (isCompltedTotalRound) {

            // Check ShownPostion
            if (Mathf.Abs(all_Rect[ShownIndex].anchoredPosition.y - flt_ShownPostion) < flt_MotionDistance / 2) {
                isSpining = false;
               StartCoroutine(DelayDownMotionAnamition());
               
              
            }
           

        }
        else {
            // Check Round
            Debug.Log("CheckRound" + Mathf.Abs(all_Rect[ShownIndex].anchoredPosition.y - flt_startPostion));
          
            if (Mathf.Abs(all_Rect[ShownIndex].anchoredPosition.y - flt_startPostion) < flt_MotionDistance / 2) {

                currentRound++;
                Debug.Log("IncreasedRound");

                if (currentRound >= noofRound) {
                    isCompltedTotalRound = true;
                }
            }
          
           
        }

        DOTween.KillAll();
      

        if (isSpining) {
            StartAnimation();
        }
        else {
           
        }
        
        
       
    }

   
}

