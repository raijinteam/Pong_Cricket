using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class AdsManager : MonoBehaviour {

    public static AdsManager instance;

    private void Awake() {
        instance = this;
    }


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


        //Add AdInfo Interstitial Events
        IronSourceInterstitialEvents.onAdReadyEvent += InterstitialOnAdReadyEvent;
        IronSourceInterstitialEvents.onAdLoadFailedEvent += InterstitialOnAdLoadFailed;
        IronSourceInterstitialEvents.onAdOpenedEvent += InterstitialOnAdOpenedEvent;
        IronSourceInterstitialEvents.onAdClickedEvent += InterstitialOnAdClickedEvent;
        IronSourceInterstitialEvents.onAdShowSucceededEvent += InterstitialOnAdShowSucceededEvent;
        IronSourceInterstitialEvents.onAdShowFailedEvent += InterstitialOnAdShowFailedEvent;
        IronSourceInterstitialEvents.onAdClosedEvent += InterstitialOnAdClosedEvent;


        //Add AdInfo Rewarded Video Events
        IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoOnAdOpenedEvent;
        IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoOnAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
        IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoOnAdClickedEvent;

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

    private void InterstitialOnAdClosedEvent(IronSourceAdInfo info) {
       
    }

    private void InterstitialOnAdShowFailedEvent(IronSourceError error, IronSourceAdInfo info) {
        if (!IronSource.Agent.isInterstitialReady()) {
            IronSource.Agent.loadInterstitial();
        }

    }

    private void InterstitialOnAdShowSucceededEvent(IronSourceAdInfo info) {
        if (!IronSource.Agent.isInterstitialReady()) {
            IronSource.Agent.loadInterstitial();
        }
    }

    private void InterstitialOnAdClickedEvent(IronSourceAdInfo info) {
       
    }

    private void InterstitialOnAdOpenedEvent(IronSourceAdInfo info) {
        
    }

    private void InterstitialOnAdLoadFailed(IronSourceError error) {
        if (!IronSource.Agent.isInterstitialReady()) {
            IronSource.Agent.loadInterstitial();
        }
        
    }

    private void InterstitialOnAdReadyEvent(IronSourceAdInfo info) {
        
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


    /************* RewardedVideo AdInfo Delegates *************/
    // Indicates that there’s an available ad.
    // The adInfo object includes information about the ad that was loaded successfully
    // This replaces the RewardedVideoAvailabilityChangedEvent(true) event
    void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo) {
       
    }
    // Indicates that no ads are available to be displayed
    // This replaces the RewardedVideoAvailabilityChangedEvent(false) event
    void RewardedVideoOnAdUnavailable() {
        LoadRewardAds();
    }
    // The Rewarded Video ad view has opened. Your activity will loose focus.
    void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo) {
        LoadRewardAds();
    }
    // The Rewarded Video ad view is about to be closed. Your activity will regain its focus.
    void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo) {

        Debug.Log("Reward Get");
        LoadRewardAds();
    }
    // The user completed to watch the video, and should be rewarded.
    // The placement parameter will include the reward data.
    // When using server-to-server callbacks, you may ignore this event and wait for the ironSource server callback.
    void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo) {
       
    }
    // The rewarded video ad was failed to show.
    void RewardedVideoOnAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo) {
        LoadRewardAds();
    }
    // Invoked when the video ad was clicked.
    // This callback is not supported by all networks, and we recommend using it only if
    // it’s supported by all networks you included in your build.
    void RewardedVideoOnAdClickedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo) {
    }



    #endregion
}
