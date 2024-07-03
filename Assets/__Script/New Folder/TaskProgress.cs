using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TextCore.LowLevel;
using System;
using System.Linq;

public class TaskProgress : MonoBehaviour {


    [SerializeField] private RectTransform obj_Main;
    
    [SerializeField] private TextMeshProUGUI txt_IncrementData;
    [SerializeField] private Slider slider_Task;
    [SerializeField] private TextMeshProUGUI txt_taskName;
    [SerializeField] private float flt_TaskPanelTopToDownTime;
    [SerializeField] private float flt_SliderUpdateTime;
    [SerializeField] private float flt_TaskeDownTopTime;
    private float flt_StartPostion;


    public void ActivetedTaskProgersPanel(List<taskShowData> data) {

        this.gameObject.SetActive(true);
        flt_StartPostion = obj_Main.anchoredPosition.y;
        obj_Main.anchoredPosition = new Vector2(obj_Main.anchoredPosition.x, 1000);
        StartCoroutine(StartAnimation(data));

    }

    private IEnumerator StartAnimation(List<taskShowData> data) {

        for (int i = 0; i < data.Count; i++) {
            // Set data
            txt_IncrementData.text  =  "+" + (data[i].UpdateValue - data[i].prevousValue).ToString();
            slider_Task.value = data[i].prevousValue;
            slider_Task.maxValue = data[i].targetValue;
            txt_taskName.text = data[i].taskName;

            obj_Main.DOAnchorPosY(flt_StartPostion, flt_TaskPanelTopToDownTime);

            yield return new WaitForSeconds(flt_TaskPanelTopToDownTime);

            float slider_StartValue = slider_Task.value;
            float flt_CurrentTime = 0;
            while (flt_CurrentTime < 1 ) {

                flt_CurrentTime += Time.deltaTime / flt_SliderUpdateTime;
                slider_Task.value = Mathf.Lerp(slider_StartValue, data[i].UpdateValue, flt_CurrentTime);
                yield return null;

            }

            yield return new WaitForSeconds(0.5f);

            obj_Main.DOAnchorPosY(1000, flt_TaskeDownTopTime);

            yield return new WaitForSeconds(flt_TaskeDownTopTime);


        }
        obj_Main.anchoredPosition = new Vector2(obj_Main.anchoredPosition.x, flt_StartPostion);
        DailyTaskManager.Instance.ResetListData();
        this.gameObject.SetActive(false);
      
    }
}
