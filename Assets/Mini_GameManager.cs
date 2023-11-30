using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Mini_GameManager : MonoBehaviour {

    public static Mini_GameManager instance;

    [Header("Componnent")]
    [SerializeField] private GameObject gameoverPanel;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Min_BallMovement ball;
    [SerializeField] private int spawnCounter;
    [SerializeField] private float flt_GameTime;
    [SerializeField] private float flt_CurrentTime;
    [SerializeField] private LayerMask layer;
    [SerializeField] private GameObject[] all_SpawnItem;
    [SerializeField] private int Coin;
    [field: SerializeField] public Mini_Player player { get; set; }
    [field: SerializeField] public Min_BallMovement CurrentBall { get; set; }
    [field : SerializeField ] public bool isGameStart { get; set; }

    

    private float flt_MinXpSotion;
    private float flt_MaxXpostion;
    private float flt_MinYPostion;
    private float flt_maxYpostion;

   

    private void Awake() {
        instance = this;
    }

    private void Start() {

        StartMiniGame();
        setSpawnPostion();
        player.SetbatsManPostion();
        SpawnNewBall();
        SpawnItem();
        isGameStart = true;
        
    }

    public void SpawnOneCollectable() {

        bool isSpawn = false;
        Vector3 spawnPostion = Vector3.zero;
        while (!isSpawn) {

            int index = Random.Range(0, 100);
            if (index < 30) {
                spawnPostion = new Vector3(flt_MinXpSotion, Random.Range(flt_MinYPostion, flt_maxYpostion), 0);
            }
            else if (index >= 30 && index < 70) {
                spawnPostion = new Vector3(flt_MaxXpostion, Random.Range(flt_MinYPostion, flt_maxYpostion), 0);
            }
            else {
                spawnPostion = new Vector3(Random.Range(flt_MinXpSotion, flt_MaxXpostion), flt_MinYPostion, 0);
            }



            Collider2D[] all_Collider = Physics2D.OverlapCircleAll(spawnPostion, 2, layer);
            if (all_Collider.Length == 0) {
                isSpawn = true;
            }
            else {
                isSpawn = false;
            }

        }

        Instantiate(all_SpawnItem[Random.Range(0, all_SpawnItem.Length)], spawnPostion, Quaternion.identity,transform);
    }

    private void SpawnItem() {

        for (int i = 0; i < spawnCounter; i++) {
            SpawnOneCollectable();


        }
    }

    private void StartMiniGame() {

        SetBgAsperScreen();
    }

    private void SetBgAsperScreen() {

        float worldScreenHeight = Camera.main.orthographicSize * 2;

        // world width is calculated by diving world height with screen heigh
        // then multiplying it with screen width
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        // to scale the game object we divide the world screen width with the
        // size x of the sprite, and we divide the world screen height with the
        // size y of the sprite
        sr.transform.localScale = new Vector3(
            worldScreenWidth / sr.sprite.bounds.size.x,
            worldScreenHeight / sr.sprite.bounds.size.y, 1);

    }

    public void SpawnNewBall() {

        CurrentBall = Instantiate(ball, transform.position, transform.rotation,transform);
        CurrentBall.SetRandomVelocityOfBall();
    }


    private void setSpawnPostion() {
        float CameraHeight = Camera.main.orthographicSize * 2;
        float aspectRatio = (float)Screen.width / Screen.height;
        float CameraWidth = CameraHeight * aspectRatio;
        Debug.Log("CameraWidfth" + CameraWidth);
        Debug.Log("CameraHeighrt" + CameraHeight);

        flt_MinXpSotion =   -CameraWidth/2 + 2;
        flt_MaxXpostion =   CameraWidth/2 - 2;
        flt_MinYPostion = -CameraHeight / 2 + 2;
        flt_maxYpostion = 0;
    }

    private void Update() {

        TimeHandler();
    }

    private void TimeHandler() {
        if (!isGameStart) {
            return;
        }
        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime >= flt_GameTime) {

            GameOver();
        }
    }


    private void GameOver() {
        isGameStart = false;
        gameoverPanel.SetActive(true);
        Destroy(CurrentBall);
    }

    public void Onclick_RestartBtn() {
        Destroy(this.gameObject);
        UIManager.Instance.panel_MainMenu.SetActive(true);
    }

    public void AddCoin() {
        Coin++;
    }
}
