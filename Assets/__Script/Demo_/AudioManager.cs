using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager insatance;

    [SerializeField] public AudioSource audioSource_BgMusic;
    [SerializeField] public AudioSource audioSource_BtnClick;
    [SerializeField] public AudioSource audioSource_UnLockedAndUpgrade;
    [SerializeField] public AudioSource audioSource_BagOpenesSound;
    [SerializeField] public AudioSource audioSource_CardShowSound;
    [SerializeField] public AudioSource audioSource_LevelUp;

    private void Awake() {
        insatance = this;
    }


    public void SetMusicData() {
        if (DataManager.Instance.IsMusic) {
            audioSource_BgMusic.Play();
        }
        else {
            audioSource_BgMusic.Stop();
        }
    }

    public void PlayBtnClickSFX() {
        if (!DataManager.Instance.IsSound) {
            return;
        }
        audioSource_BtnClick.Play();
    }

    public void Play_UnLockedOrUpgradeSfx() {
        if (!DataManager.Instance.IsSound) {
            return;
        }
        audioSource_UnLockedAndUpgrade.Play();
    }

    public void PlayBagOpenedSfx() {
        if (!DataManager.Instance.IsSound) {
            return;
        }
        audioSource_BagOpenesSound.Play();
    }

    public void PlayCardShowSFX() {
        if (!DataManager.Instance.IsSound) {
            return;
        }
        audioSource_CardShowSound.Play();
    }
}
