using DG.Tweening;

using UnityEngine;

public class BoardAnimation : MonoBehaviour {

    [SerializeField] private Sprite[] all_Sprite;
    [SerializeField] private Vector3 flt_MovePostion;
    [SerializeField] private float flt_AnimtionTime;
    private SpriteRenderer sr;


    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }



    private void Start() {

        int index = Random.Range(0, all_Sprite.Length);
        sr.sprite = all_Sprite[index];
        transform.DOLocalMove(flt_MovePostion, flt_AnimtionTime).SetLoops(-1, LoopType.Yoyo);
    }
}
