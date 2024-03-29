using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mini_GameInformation : MonoBehaviour {

    public void OnClick_OnPlay() {
        this.gameObject.SetActive(false);
        Mini_GameManager.instance.StartMiniGame();
    }
}
