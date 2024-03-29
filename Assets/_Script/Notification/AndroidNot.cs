using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

#if UNITY_ANDROID
using Unity.Notifications.Android;
using UnityEngine.Android;
#endif


public class AndroidNot : MonoBehaviour {

#if UNITY_ANDROID

    public string chanel_DailyRewardId = "DailyReward_channel";
    public string chanel_Chest = "Chest_Notification";
   
    // Request Authoriztion  Notification

    public void RequestToAuthentication() {

        if (!Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS")) {
            Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
        }

    }


    public void RegisterDaiyrewardNotificationChanel() {

        var chanel = new AndroidNotificationChannel {
            Id = chanel_DailyRewardId,
            Name = "Defual_Name",
            Importance = Importance.Default,
            Description = "Full Lives"

        };

        AndroidNotificationCenter.RegisterNotificationChannel(chanel);
    }

    public void RegisterChestNotificationChanel() {

        var chanel = new AndroidNotificationChannel {
            Id = chanel_Chest,
            Name = "Defual_Name",
            Importance = Importance.Default,
            Description = "Full Lives"

        };

        AndroidNotificationCenter.RegisterNotificationChannel(chanel);
    }

    public void SendDailyRewardNotification(string title, string text , float sec) {

        var notification = new AndroidNotification();

        notification.Title = title;
        notification.Text = text;
        notification.FireTime = System.DateTime.Now.AddSeconds(sec);
        notification.SmallIcon = "Icon_0";
        notification.LargeIcon = "Icon_1";
        AndroidNotificationCenter.SendNotification(notification, chanel_DailyRewardId);
    }

    public void SendChestNotification(string title, string text, float sec) {

        var notification = new AndroidNotification();

        notification.Title = title;
        notification.Text = text;
        notification.FireTime = System.DateTime.Now.AddSeconds(sec);
        notification.SmallIcon = "Icon_0";
        notification.LargeIcon = "Icon_1";
        AndroidNotificationCenter.SendNotification(notification, chanel_Chest);
    }


#endif
}
