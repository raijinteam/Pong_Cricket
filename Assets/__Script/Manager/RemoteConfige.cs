using Firebase.RemoteConfig;
using System;
using Firebase.Extensions;
using System.Threading.Tasks;
using UnityEngine;

public class RemoteConfige : MonoBehaviour {

    public ConfigData allConfigData;


    private void Awake() {

        Debug.Log("JasonData" + JsonUtility.ToJson(allConfigData));
        CheckRemoteConfigValues();
    }

    public Task CheckRemoteConfigValues() {
        Debug.Log("Fetching data...");
        Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }
    private void FetchComplete(Task fetchTask) {
        if (!fetchTask.IsCompleted) {
            Debug.LogError("Retrieval hasn't finished.");
            return;
        }

        var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
        var info = remoteConfig.Info;
        if (info.LastFetchStatus != LastFetchStatus.Success) {
            Debug.LogError($"{nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
            return;
        }

        // Fetch successful. Parameter values must be activated to use.
        remoteConfig.ActivateAsync()
          .ContinueWithOnMainThread(
            task => {
                Debug.Log($"Remote data loaded and ready for use. Last fetch time {info.FetchTime}.");

                string configData = remoteConfig.GetValue("Game_Data").StringValue;
                allConfigData = JsonUtility.FromJson<ConfigData>(configData);

                print("Total values: "+remoteConfig.AllValues.Count);

                foreach (var item in remoteConfig.AllValues) {
                    print("Key :" + item.Key);
                    print("Value: " + item.Value.StringValue);
                }

            });
    }


}

[Serializable]
public class ConfigData {
    public string playerName;
    public float gameVersion;
    public int crrLevel;
    public bool over18;
}
