using UnityEngine;
using UnityEngine.UI;

public class mini_HomeScreen : MonoBehaviour {

    [SerializeField] private Image img_ads;

    private void OnEnable() {

        img_ads.sprite = DataManager.Instance.GetSprite();
        DataManager.Instance.Changesprite += SetMySpriet;
    }
    private void OnDisable() {

        DataManager.Instance.Changesprite -= SetMySpriet;
    }

    private void SetMySpriet(Sprite sprite) {
        img_ads.sprite = sprite;
    }

    public void OnClick_StarMiniGame() {
        if (DataManager.Instance.skipIts <= 0) {
            // SET ADS
            AdsManager.instance.ShowRewardAds(AdsRewardType.MiniGame);
        }
        else {
            DataManager.Instance.RemoveSkipIts();
            Mini_UiManager.instance.mini_GameInformation.gameObject.SetActive(true);
        }

        this.gameObject.SetActive(false);
        
    }
}
