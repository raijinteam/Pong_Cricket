using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mini_MenuScreen : MonoBehaviour {

    public void OnClickON_MenuScreen() {

        this.gameObject.SetActive(false);
        Mini_UiManager.instance.mini_HomeScreen.gameObject.SetActive(true);
    }
}
