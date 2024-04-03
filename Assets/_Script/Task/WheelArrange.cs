using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelArrange : MonoBehaviour {

    [SerializeField] private int noofObj;
    [SerializeField] private GameObject pf_Cube;
    [SerializeField] private float flt_redius;

    private void Start() {
        SetPostion();
        //SetUI();
    }

    private void SetUI() {
        float startAngle = 0;
        float increment_Angle = 360 / noofObj;
        float currentAngle = startAngle;
        float scale = 2 * 3.14f * flt_redius / noofObj;
        for (int i = 0; i < noofObj; i++) {

            Vector3 SpawnPostion = new Vector3(0, flt_redius * Mathf.Sin(currentAngle * Mathf.Deg2Rad), flt_redius * Mathf.Cos(currentAngle * Mathf.Deg2Rad));
            GameObject current = Instantiate(pf_Cube, transform);
            current.GetComponent<RectTransform>().position = SpawnPostion;
           // current.GetComponent<RectTransform>().

           // current.transform.localScale = new Vector3(scale, scale, 0.01f);

            currentAngle += increment_Angle;

        }

    }

    private void SetPostion() {

       
      
        float startAngle = 0;
        float increment_Angle = 360/ noofObj;
        float currentAngle = startAngle;
        float scale = 2 * 3.14f * flt_redius / noofObj;

        for (int i = 0; i < noofObj; i++) {

            Vector3 SpawnPostion = new Vector3(0, flt_redius * Mathf.Sin(currentAngle * Mathf.Deg2Rad), flt_redius * Mathf.Cos(currentAngle*Mathf.Deg2Rad));
            GameObject current =  Instantiate(pf_Cube, SpawnPostion, Quaternion.Euler(currentAngle, 0,0),transform);
            
           current.transform.localScale = new Vector3(scale , scale, 0.01f);
           
            currentAngle += increment_Angle;

        }
    }
}
