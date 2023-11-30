using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_SloteTime : MonoBehaviour {

    [field: SerializeField] public GameObject chest_Running;
    [field: SerializeField] public GameObject chest_OpenInfo;


    public void OnClick_ChestRunningBtn() {

        this.gameObject.SetActive(false);
        TutorialHandler.instance.ui_HomeScreen.HideAllPanel();
        TutorialHandler.instance.ui_HomeScreen.slote_Running.gameObject.SetActive(true);
    }

    public void OnClick_ChestFinishedBtn() {
        this.gameObject.SetActive(false);
        
        TutorialHandler.instance.ui_HomeScreen.HideAllPanel();
        TutorialHandler.instance.ui_HomeScreen.slote_Finisehed.gameObject.SetActive(true);
    }
}

   
