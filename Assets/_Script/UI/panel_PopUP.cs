using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;
using DG.Tweening;

public class panel_PopUP : MonoBehaviour {

    [SerializeField] private Transform rect_Glow;
    [SerializeField] public RectTransform[] all_transforms;
    [SerializeField] private TextMeshProUGUI txt_Message;
    [SerializeField] private float flt_AnimationTime;
    [SerializeField] private float flt_RotationSpeed;
 

    public void ActvetedPanel(float flt_ShowmTime , string _Messeage) {
        txt_Message.text = _Messeage;
        for (int i = 0; i < all_transforms.Length; i++) {
            all_transforms[i].localScale = Vector3.zero;
        }
        this.gameObject.SetActive(true);
        StartCoroutine(PanelSpawn(flt_ShowmTime));
       
    }

    private void Update() {

        rect_Glow.Rotate(Vector3.forward * flt_RotationSpeed*Time.deltaTime);
    }

    private IEnumerator PanelSpawn(float flt_ShowmTime) {
        for (int i = 0; i < all_transforms.Length; i++) {
            all_transforms[i].DOScale(1, flt_AnimationTime);
            yield return new WaitForSeconds ( flt_AnimationTime/ 2);
        }
        yield return new WaitForSeconds(flt_AnimationTime/2);
        yield return new WaitForSeconds(flt_ShowmTime);

        for (int i = 0; i < all_transforms.Length; i++) {
            all_transforms[i].DOScale(0, flt_AnimationTime/2);
            yield return new WaitForSeconds(flt_AnimationTime / 4);
        }

        yield return new WaitForSeconds(flt_AnimationTime / 4);
        yield return new WaitForEndOfFrame();
        Destroy(this.gameObject);
    }
}
