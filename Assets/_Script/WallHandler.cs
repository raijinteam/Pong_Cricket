using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHandler : MonoBehaviour {


    [Header("All_Collider")]
    [SerializeField] private BoxCollider2D collider_Left;
    [SerializeField] private BoxCollider2D collider_Right;
    [SerializeField] private BoxCollider2D collider_Top;
    [SerializeField] private BoxCollider2D collider_Bottam;

    [SerializeField] private BoxCollider2D[] all_RunnerCollider_Left;
    [SerializeField] private BoxCollider2D[] all_RunnerCollider_Right;
    [SerializeField] private BoxCollider2D runnerCollider_Top;
    [SerializeField] private BoxCollider2D runnerCollider_Bottam;

    [SerializeField] private SpriteRenderer bg;




    public void SetAllColliderAsPerScreen() {

        float screenAspect = (float)Screen.width / Screen.height;
        float cameraHeight = Camera.main.orthographicSize * 2;
        float cameraWidth = cameraHeight * screenAspect;


        collider_Top.transform.position = new Vector3(0, (cameraHeight / 2) + 0.5f);
        collider_Top.transform.localScale = new Vector3(cameraWidth, 1, 1);

        collider_Bottam.transform.position = new Vector3(0, (-cameraHeight / 2) - 0.5f);
        collider_Bottam.transform.localScale = new Vector3(cameraWidth, 1, 1);

        collider_Left.transform.position = new Vector3((-cameraWidth / 2) - 0.5f, 0);
        collider_Left.transform.localScale = new Vector3(1, cameraHeight, 1);

        collider_Right.transform.position = new Vector3((cameraWidth / 2) + 0.5f, 0);
        collider_Right.transform.localScale = new Vector3(1, cameraHeight, 1);
    }

    public void SetRunnerColllider() {
        float screenAspect = (float)Screen.width / Screen.height;
        float cameraHeight = Camera.main.orthographicSize * 2;
        float cameraWidth = cameraHeight * screenAspect;
        SetLeftRunnerCollider(cameraWidth);
        SetRightRunnerCollider(cameraWidth);
        SetTopRunnerCollider(cameraHeight, cameraWidth);
        SetBottamRunnerCollider(cameraHeight, cameraWidth);

    }

    public void SetBg() {

        float worldScreenHeight = Camera.main.orthographicSize * 2;

        // world width is calculated by diving world height with screen heigh
        // then multiplying it with screen width
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        // to scale the game object we divide the world screen width with the
        // size x of the sprite, and we divide the world screen height with the
        // size y of the sprite
        bg.transform.localScale = new Vector3(
            worldScreenWidth / bg.sprite.bounds.size.x,
            worldScreenHeight / bg.sprite.bounds.size.y, 1);
    }

    private void SetBottamRunnerCollider(float cameraHeight, float cameraWidth) {
        runnerCollider_Bottam.transform.position = new Vector3(0, (-cameraHeight / 2) + 0.5f);
        runnerCollider_Bottam.transform.localScale = new Vector3(cameraWidth, 1, 1);

    }

    private void SetTopRunnerCollider(float cameraHeight,float cameraWidth) {
        runnerCollider_Top.transform.position = new Vector3(0, (cameraHeight / 2) - 0.5f);
        runnerCollider_Top.transform.localScale = new Vector3(cameraWidth, 1, 1);
    }

    private void SetRightRunnerCollider(float cameraWidth) {
        for (int i = 0; i < all_RunnerCollider_Right.Length; i++) {
            all_RunnerCollider_Right[i].transform.localPosition = new Vector3((cameraWidth / 2) - 0.5f, 
                            all_RunnerCollider_Right[i].transform.localPosition.y);
        }
    }

    private void SetLeftRunnerCollider(float _cameraWidth) {

        for (int i = 0; i < all_RunnerCollider_Left.Length; i++) {
            all_RunnerCollider_Left[i].transform.localPosition = new Vector3((-_cameraWidth / 2) + 0.5f,
                                all_RunnerCollider_Left[i].transform.localPosition.y);
        }
    }
}
