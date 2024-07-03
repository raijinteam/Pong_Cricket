using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class UISettingScreen : MonoBehaviour {

    [SerializeField] private GameObject sound_On;
    [SerializeField] private GameObject sound_Off;
    [SerializeField] private GameObject viabration_On;
    [SerializeField] private GameObject viabration_Off;


    [Header("Animation Data")]
    [SerializeField] private float flt_AnimationTime;
    [SerializeField] private float flt_ClosedAnimation;
    [SerializeField] private RectTransform rect_main;

    



    private void OnEnable() {
        SetSoundData();
        Panel_Animation.instance.Enable_PopUp(rect_main, flt_AnimationTime);
    }



    private void SetSoundData() {
        sound_On.SetActive(DataManager.Instance.IsSound);
        sound_Off.SetActive(!DataManager.Instance.IsSound);
        viabration_On.SetActive(DataManager.Instance.IsMusic);
        viabration_Off.SetActive(!DataManager.Instance.IsMusic);
    }

    public void OnClick_SoundBtn() {

        AudioManager.insatance.PlayBtnClickSFX();
        DataManager.Instance.SetsoundData();
        SetSoundData();

    }

    public void OnClick_Viabration() {
        DataManager.Instance.SetMusicData();
        SetSoundData();
    }

    public void OnClick_CloseBtn() {
        AudioManager.insatance.PlayBtnClickSFX();
        Panel_Animation.instance.Disable_PopUp(rect_main, flt_AnimationTime ,this.gameObject);
    }
}
