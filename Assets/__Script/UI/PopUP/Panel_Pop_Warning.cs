using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Panel_Pop_Warning : MonoBehaviour {

    [SerializeField] private RectTransform rect_Target;
    [SerializeField] private TextMeshProUGUI txt_Message;
    [SerializeField] private float flt_StartPostion;
    [SerializeField] private float flt_EndPostion;


    [SerializeField] private float flt_ShownTime;
    [SerializeField] private float flt_AnimtionTime;


    public void ActvetedPopUp(string Message) {
        this.gameObject.SetActive(true);
        txt_Message.text = Message;
        rect_Target.anchoredPosition = new Vector2(flt_StartPostion, rect_Target.anchoredPosition.y);

        Sequence sq = DOTween.Sequence();
        sq.Append(rect_Target.DOAnchorPosX(0, flt_AnimtionTime).SetEase(Ease.OutBack));
        sq.AppendInterval(flt_ShownTime);
        sq.Append(rect_Target.DOAnchorPosX(flt_EndPostion, flt_AnimtionTime / 2).SetEase(Ease.InBack));
        sq.AppendCallback(() => {

            Destroy(this.gameObject);
        });
    }
}
