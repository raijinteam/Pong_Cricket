using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AdsManager : MonoBehaviour {


# if UNITY_ANDROID
    string appKey = "1e02589cd";
#elif UNITY_IOS
    string appKey = "";
#endif



    private void Start() {
        IronSource.Agent.validateIntegration();
        IronSource.Agent.init(appKey);
    }

    private void OnEnable() {
        IronSourceEvents.onSdkInitializationCompletedEvent += SdkIntialised;
    }

    private void SdkIntialised() {
        Debug.Log("sdk Intialised___");
    }
    private void OnApplicationPause(bool pause) {
        IronSource.Agent.onApplicationPause(pause);
    }


    #region Banner Ads

    public void LoadBannerAds() {
        IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
    }

    public void DestroyBanner() {

        IronSource.Agent.destroyBanner();
    }
    #endregion


    #region Interstital

    public void LoadInterstitalAds() {
        IronSource.Agent.loadInterstitial();
    }

    public void ShowInterstialAds() {
        if (IronSource.Agent.isInterstitialReady()) {
            IronSource.Agent.showInterstitial();
        }
        else {
            Debug.Log("Not Ready For InterTial");
        }
    }
    #endregion

    #region Reward Ads

    public void LoadRewardAds() {
        IronSource.Agent.loadRewardedVideo();
    }

    public void ShowRewardAds() {
        if (IronSource.Agent.isRewardedVideoAvailable()) {
            IronSource.Agent.showRewardedVideo();
        }
        else {
            Debug.Log(" not Ready  Reward Ads");
        }
    }
    #endregion
}
