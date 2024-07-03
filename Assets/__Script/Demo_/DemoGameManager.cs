using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoGameManager : MonoBehaviour {


    public static DemoGameManager instance;

    [SerializeField] private float topPostion;
    [SerializeField] private float bottamPostion;
    [SerializeField] private SpriteRenderer sr;
    public Transform clamp;


    public DemoPlayer demoPlayer;
    public DemoBallMotion demoBALL;
    



    public float flt_CameraHeight;
    public float flt_CameraWidhth;

    [field : SerializeField]public DemoPlayerAi CurrentGamePlayerAI { get; internal set; }

    private void Awake() {
        instance = this;
    }



    private void Start() {

        SetBg();
    }

    private void SetBg() {


        float flt_aspectRatio = (float)Screen.width / Screen.height;
        flt_CameraHeight = Camera.main.orthographicSize * 2;
        flt_CameraWidhth = flt_aspectRatio * flt_CameraHeight;

        sr.transform.localScale = new Vector3(flt_CameraWidhth / sr.bounds.size.x, flt_CameraHeight / sr.bounds.size.y, 1);

        demoPlayer.transform.position = new Vector3(0, topPostion, 0);

       
    }

    public void LoadScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
   
}
