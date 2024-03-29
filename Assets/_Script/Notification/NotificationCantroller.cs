using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;




#if UNITY_ANDROID
using Unity.Notifications.Android;
using UnityEngine.Android;
#endif

#if UNITY_IOS
using Unity.Notifications.iOS;
#endif

public class NotificationCantroller : MonoBehaviour {


    public static NotificationCantroller instance;
    [SerializeField] private AndroidNot android_not;
    [SerializeField] private IosNot iosNot;


    private void Awake() {
        instance = this;
    }

    private void Start() {

#if UNITY_ANDROID
        android_not.RequestToAuthentication();
        android_not.RegisterDaiyrewardNotificationChanel();
        android_not.RegisterChestNotificationChanel();
#endif

#if UNITY_IOS
        StartCoroutine(iosNot.RequestAuthorization());
#endif
    }


    private void OnApplicationFocus(bool focus) {
        if (focus == false) {
            Debug.Log("Focus False");
           


            TimeSpan Time = RewardsManager.Instance.dailyRewardData.GetCurrentTimeLeft();
            float second = Time.Seconds;
            Debug.Log("Pendingsecond for Daily Reward" + second);
           
            //SetDailyNotification("Pong2d" , "DailyReward Active" , second);

            if (ChestManager.Instance.IsAnyChestTimecalcuLationStart()) {

                Time = ChestManager.Instance.GetTimeLeftForChestUnlock();
                 second = Time.Seconds;
                Debug.Log("Pendingsecond for chest Reward" + second);
                SetchestNotification("Pong2d", "ChestOpning", second);
            }
        }
        else {
            Debug.Log("focas" + true);
            RemoveAllNotification();
        }
       

    }

   


    private void SetDailyNotification(string GameName, string title , float time) {

#if UNITY_ANDROID

       
        android_not.SendDailyRewardNotification(GameName, title, time);
#endif

#if UNITY_IOS

           
            iosNot.SendNotification(GameName , title , "" , time);
#endif
    }

    private void SetchestNotification(string GameName, string title, float time) {

#if UNITY_ANDROID

      
        android_not.SendChestNotification(GameName, title, time);
#endif

#if UNITY_IOS

          
            iosNot.SendNotification(GameName , title , "" , time);
#endif
    }

    private void RemoveAllNotification() {

#if UNITY_ANDROID

        AndroidNotificationCenter.CancelAllNotifications();
       
#endif

#if UNITY_IOS

            iOSNotificationCenter.RemoveAllScheduledNotifications();
            
#endif
    }





}


