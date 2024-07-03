using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;


public class PopUpUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI txt_Message;
  

    
    public void SpwanPopUP(string _Message, float flt_Time) {

        transform.localScale = new Vector3(1, 0, 1);
        Sequence sq = DOTween.Sequence();
        txt_Message.text = _Message;
    
        sq.Append(transform.DOScale(new Vector3(1, 1.1f, 1), flt_Time / 8).SetEase(Ease.OutBack));
        sq.Append(transform.DOScale(new Vector3(1, 0.9f, 1), flt_Time / 8).SetEase(Ease.Linear).SetLoops(6, LoopType.Yoyo));
        sq.Append(transform.DOScale(new Vector3(1, 0, 1), flt_Time / 8).SetEase(Ease.InBack));
        sq.AppendCallback(() => { Destroy(this.gameObject); });

        sq.Play();

    }

}
