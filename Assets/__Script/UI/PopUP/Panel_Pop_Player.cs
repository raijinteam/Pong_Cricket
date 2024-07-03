using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_Pop_Player : MonoBehaviour {

    [SerializeField] private RectTransform RectMain;
    [SerializeField] private RectTransform Rect_Bat;
    [SerializeField] private RectTransform Rect_Bawller;



    public void SpwanPopUP(bool isBatsman, float flt_Time) {



        Rect_Bat.gameObject.SetActive(isBatsman);
        Rect_Bawller.gameObject.SetActive(!isBatsman);

        RectTransform panel = isBatsman ? Rect_Bat : Rect_Bawller;

        panel.anchoredPosition = new Vector2(2500, 0);
        RectMain.anchoredPosition = new Vector2(-2500, 0);

        Sequence sq = DOTween.Sequence();

        sq.Append(RectMain.DOAnchorPos(Vector3.zero, flt_Time / 4).SetEase(Ease.OutBack));
        sq.Join(panel.DOAnchorPos(Vector3.zero, flt_Time / 4).SetEase(Ease.OutBack));
        sq.AppendInterval(flt_Time / 2);
        sq.Append(RectMain.DOAnchorPos(new Vector3(2500, 0, 0), flt_Time / 4).SetEase(Ease.InBack));
        sq.Join(panel.DOAnchorPos(new Vector3(-2500, 0, 0), flt_Time / 4).SetEase(Ease.InBack));
        sq.AppendCallback(() => Destroy(this.gameObject));



    }
}
