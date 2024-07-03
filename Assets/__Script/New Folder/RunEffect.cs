using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RunEffect : MonoBehaviour {



    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private float flt_AnimationTime;
    [SerializeField] private float flt_LerpTime;



    public void SetTargetPostion(Sprite Run) {
        sr.sprite = Run;

        transform.localScale = Vector3.zero;
        Sequence sq = DOTween.Sequence();

        sq.Append(transform.DOScale(Vector3.one, flt_AnimationTime).SetEase(Ease.OutBack));
        sq.Append(transform.DOMove(GameManager.Instance.ScoreTarget.position, flt_LerpTime).SetEase(Ease.InBack));
        sq.Join(transform.DOScale(new Vector3(0.25f, 0.25f, 0.25f), flt_LerpTime).SetEase(Ease.InBack));
        sq.AppendCallback(() => { GameManager.Instance.ShowRun(); });
        sq.AppendCallback(() => Destroy(this.gameObject));


    }
}
