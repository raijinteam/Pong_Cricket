using GameAnalyticsSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAnalyticsManager : MonoBehaviour , IGameAnalyticsATTListener {

    public static GameAnalyticsManager instance;


    private void Awake() {
        instance = this;
    }

    void Start() {

        
        if (Application.platform == RuntimePlatform.IPhonePlayer) {
            GameAnalytics.RequestTrackingAuthorization(this);
        }
        else {
            GameAnalytics.Initialize();
        }
    }

    public void GameAnalyticsATTListenerNotDetermined() {
        GameAnalytics.Initialize();
    }
    public void GameAnalyticsATTListenerRestricted() {
        GameAnalytics.Initialize();
    }
    public void GameAnalyticsATTListenerDenied() {
        GameAnalytics.Initialize();
    }
    public void GameAnalyticsATTListenerAuthorized() {
        GameAnalytics.Initialize();
    }

    public void AddNewDiesign(string str_EventName ) {
        GameAnalytics.NewDesignEvent(str_EventName);
    }

    public void AddNewEventWithData(string str_EventName , float flt_Value) {
        GameAnalytics.NewDesignEvent("str_EventName", flt_Value);
    }
}
