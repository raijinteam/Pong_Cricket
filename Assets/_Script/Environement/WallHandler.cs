using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WallHandler : MonoBehaviour {


    [Header("All_Collider")]


    [SerializeField] private SpriteRenderer bg;

   
    [field : SerializeField] public Transform batsmanleft { get; private set; }
    [field : SerializeField] public Transform batsmanright { get; private set; }
    [field :SerializeField] public Transform bowlerleft { get; private set; }
    [field : SerializeField ] public  Transform bowlerright { get; private set; }

   



    [Header("Collder_Runner")]
    [SerializeField] private Collder_Runner[] all_RunnerCollider;
    private List<Collder_Runner> list_ActivatedRunner = new List<Collder_Runner>();




   

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
}
