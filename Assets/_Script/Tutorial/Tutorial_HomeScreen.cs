using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_HomeScreen : MonoBehaviour {

    [field: SerializeField] public GameObject Slote_Empaty { get; set; }
    [field : SerializeField] public GameObject slote_Filled { get; set; }
    [field : SerializeField] public GameObject slote_Running { get; set; }

    [field: SerializeField] public GameObject slote_Finisehed { get; set; }


    [SerializeField]private ChestState CurrentChestState;


    



    public void OnClick_OnChestfilledTimeBtn() {

        if (CurrentChestState == ChestState.slote_Filled) {
            TutorialHandler.instance.ui_SloteTime.gameObject.SetActive(true);
            TutorialHandler.instance.ui_SloteTime.chest_OpenInfo.SetActive(false);
            TutorialHandler.instance.ui_SloteTime.chest_Running.SetActive(true);
            CurrentChestState = ChestState.slote_Running;

        }
        else if (CurrentChestState == ChestState.slote_Running) {

            TutorialHandler.instance.ui_SloteTime.gameObject.SetActive(true);
            TutorialHandler.instance.ui_SloteTime.chest_OpenInfo.SetActive(true);
            TutorialHandler.instance.ui_SloteTime.chest_Running.SetActive(false);
            CurrentChestState = ChestState.slote_finished;
        }
        else if (CurrentChestState == ChestState.slote_finished) {
            CurrentChestState = ChestState.slote_Empaty;
            HideAllPanel();
            Slote_Empaty.SetActive(true);
            Debug.Log("AnimationDone");
            TutorialHandler.instance.CompletedTutorial();
        }

       
        



    }

    public void HideAllPanel() {
        Slote_Empaty.SetActive(false);
        slote_Filled.SetActive(false);
        slote_Running.SetActive(false);
        slote_Finisehed.SetActive(false);
    }



}

public enum ChestState {

    slote_Empaty,
    slote_Filled,
    slote_Running,
    slote_finished
}