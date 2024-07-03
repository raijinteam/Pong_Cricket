using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor;

public class EventBaseBoard : MonoBehaviour {


    [SerializeField] private Vector3 movePostion;
    [SerializeField] private float flt_Scale;
    [SerializeField] private float flt_AnimationTime;
    [SerializeField] private float flt_MoveAnimationTime;

    private SpriteRenderer sr;



    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }







    private void Start() {

        BoardHandler.instance.OnPlayBoardAnimation += BoardHandler_OnPlayBoardAnimation;
        transform.localScale = Vector3.zero;
        this.gameObject.SetActive(false);

        
    }

    private void BoardHandler_OnPlayBoardAnimation(Sprite obj) {

        if (this.gameObject.activeSelf) {
            return;
        }

        this.gameObject.SetActive(true);

        sr.sprite = obj;
        Sequence sq = DOTween.Sequence();

        sq.Append(transform.DOScale(Vector3.one * flt_Scale, flt_AnimationTime)).
            Append(transform.DOLocalMove(movePostion, flt_MoveAnimationTime).SetLoops(8, LoopType.Yoyo)).
            Append(transform.DOScale(Vector3.zero, flt_AnimationTime)).AppendCallback(() => { this.gameObject.SetActive(false); });
    }

   
}
