using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_Animation : MonoBehaviour  {

    public static Panel_Animation instance;

    private void Awake() {
        instance = this;
    }

    public void Enable_PopUp(Transform rect_Main , float flt_AnimationTime) {

        StartCoroutine(StartAnimation(rect_Main , flt_AnimationTime));
        
    }

    public void Disable_PopUp(Transform rect_Main, float flt_AnimationTime , GameObject _Closed) {

        StartCoroutine(CloseAnimation(rect_Main, flt_AnimationTime , _Closed));

    }



    private  IEnumerator StartAnimation(Transform rect_Main, float flt_AnimationTime) {
        rect_Main.localScale = Vector3.zero;
        rect_Main.DOScaleX(1f, flt_AnimationTime).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(flt_AnimationTime * 0.25f);
        rect_Main.DOScaleY(1f, flt_AnimationTime).SetEase(Ease.OutBack);



    }
    private IEnumerator CloseAnimation(Transform rect_Main, float flt_CloseAnimation ,GameObject _Closed) {

        rect_Main.DOScaleY(0, flt_CloseAnimation).SetEase(Ease.InBack);

        yield return new WaitForSeconds(flt_CloseAnimation * 0.25f);
        rect_Main.DOScaleX(0, flt_CloseAnimation).SetEase(Ease.InBack);

        yield return new WaitForSeconds(flt_CloseAnimation);

        _Closed.gameObject.SetActive(false);

    }
}
