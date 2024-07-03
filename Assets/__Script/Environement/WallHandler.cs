using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WallHandler : MonoBehaviour {


    [Header("All_Collider")]

    [SerializeField] private GameObject obj_Win;
    [SerializeField] private SpriteRenderer bg;

   
    [field : SerializeField] public Transform batsmanleft { get; private set; }
    [field : SerializeField] public Transform batsmanright { get; private set; }
    [field :SerializeField] public Transform bowlerleft { get; private set; }
    [field : SerializeField ] public  Transform bowlerright { get; private set; }

   



    [Header("Collder_Runner")]
    [SerializeField] private SpriteRenderer bg_Shown;
    [SerializeField] private Collder_Runner[] all_RunnerCollider;
    private List<Collder_Runner> list_ActivatedRunner = new List<Collder_Runner>();



    private void Start() {

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


    public void SetBg() {

        bg_Shown.sprite = LevelManager.Instance.GetLevelIcon(LevelManager.Instance.currentLevelIndex);

       
    }

  

    public void ActivetedBlock(int no_OfBlock) {
        for (int i = 0; i < no_OfBlock; i++) {

            bool isSpawn = false;
            while (!isSpawn) {
                int index = Random.Range(0, all_RunnerCollider.Length);
                if (list_ActivatedRunner.Contains(all_RunnerCollider[index])) {
                    isSpawn = false;
                }
                else {
                    isSpawn = true;
                    all_RunnerCollider[index].ActivetedBlock();
                    list_ActivatedRunner.Add(all_RunnerCollider[index]);
                }
            }
           
        }
    }

    public void DeActivetedBlock() {
        for (int i = 0; i < list_ActivatedRunner.Count; i++) {
            list_ActivatedRunner[i].DeActivetedBlock();
        }
    }

    public void PlayWinEffect(bool v) {

        obj_Win.gameObject.SetActive(v);
    }
}
