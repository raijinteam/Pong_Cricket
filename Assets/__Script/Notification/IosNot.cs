using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_IOS
using Unity.Notifications.iOS;
#endif


public class IosNot : MonoBehaviour {

#if UNITY_IOS
    // request To Notification
    public IEnumerator RequestAuthorization() {
        var authorizationOption = AuthorizationOption.Alert | AuthorizationOption.Badge;
        using (var req = new AuthorizationRequest(authorizationOption, true)) {
            while (!req.IsFinished) {
                yield return null;
            };

            string res = "\n RequestAuthorization:";
            res += "\n finished: " + req.IsFinished;
            res += "\n granted :  " + req.Granted;
            res += "\n error:  " + req.Error;
            res += "\n deviceToken:  " + req.DeviceToken;
            Debug.Log(res);
        }
    }


    public void SendNotification(string title, string body,string subttile,int Time) {

        var timeTrigger = new iOSNotificationTimeIntervalTrigger() {
            TimeInterval = new TimeSpan(0, Time, 0),
            Repeats = false
        };

        var notification = new iOSNotification() {
            // You can specify a custom identifier which can be used to manage the notification later.
            // If you don't provide one, a unique string will be generated automatically.
            Identifier = "live _full",
            Title = title,
            Body = body ,
            Subtitle = subttile,
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = "category_a",
            ThreadIdentifier = "thread1",
            Trigger = timeTrigger,
        };

        iOSNotificationCenter.ScheduleNotification(notification);
    }
#endif


}
