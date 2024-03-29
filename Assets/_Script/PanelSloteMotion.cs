using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class PanelSloteMotion : MonoBehaviour {

    [SerializeField] private float flt_ChangeLeftPostion;
    [SerializeField] private float flt_ChangeRightPostion;
    [SerializeField] private bool isRightMotion;
    [SerializeField] private float flt_MotionSpeed;
    [SerializeField] private RectTransform arrwo;
    [SerializeField] private Vector3 postion;
    [SerializeField] private float flt_Range = 86;
    [SerializeField] private float[] all_RewardPostion;
    [SerializeField] private Button btn_stop;
    [SerializeField] private ChestopeningUI chestopeningUI;
    private bool isStartAnimation = true;


    private void OnEnable() {
        isRightMotion = true;
        isStartAnimation = true;
        btn_stop.interactable = true;
    }
  
    private void Update() {
        if (!isStartAnimation) {
            return;
        }
        arrwo.anchoredPosition += Vector2.right*flt_MotionSpeed * Time.deltaTime;
        postion = arrwo.position;
        if (isRightMotion) {

            if (arrwo.anchoredPosition.x >= flt_ChangeRightPostion) {
                isRightMotion = false;
                flt_MotionSpeed *= -1;
            }
        }
        else {
            if (arrwo.anchoredPosition.x <=  flt_ChangeLeftPostion) {
                isRightMotion = true;
                flt_MotionSpeed *= -1;
            }

        }
    }

    public void Onclick_StopBtn() {

        btn_stop.interactable = false;
        isStartAnimation = false;


        bool IsgetAllReward = (Mathf.Clamp(arrwo.anchoredPosition.x,-flt_Range,flt_Range)== arrwo.anchoredPosition.x)
                    ? GetAllReward() : Random2XReward();

        if (Mathf.Abs(arrwo.anchoredPosition.x) < 150) {
            Debug.Log(" All 2X Reward");
            chestopeningUI.GetAllReward2X();

        }
        else {
            Debug.Log(" Get Random one  Reward 2X");
            chestopeningUI.GetOneReward2X();

        }


    }
    private bool GetAllReward() {
        Debug.Log("All 2X Reward");
        return true;
    }
    private bool Random2XReward() {
        Debug.Log("Random 2X Reward");
        return true;
    }
}

