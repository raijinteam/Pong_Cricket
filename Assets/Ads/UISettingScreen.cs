using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISettingScreen : MonoBehaviour {

    [SerializeField] private GameObject sound_On;
    [SerializeField] private GameObject sound_Off;
    [SerializeField] private GameObject viabration_On;
    [SerializeField] private GameObject viabration_Off;
    [SerializeField] private bool isSound;
    [SerializeField] private bool isViabration;



    private void OnEnable() {
        SetSoundData();
    }



    private void SetSoundData() {
        sound_On.SetActive(isSound);
        sound_Off.SetActive(!isSound);
        viabration_On.SetActive(isViabration);
        viabration_Off.SetActive(!isViabration);
    }

    public void OnClick_SoundBtn() {

        isSound = !isSound;
        SetSoundData();

    }

    public void OnClick_Viabration() {
        isViabration = !isViabration;
        SetSoundData();
    }

    public void OnClick_CloseBtn() {
        this.gameObject.SetActive(false);
    }
}
