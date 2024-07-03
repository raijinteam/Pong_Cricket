using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Tutorial_PopUpMessage : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI txt_Message;
    [SerializeField] private bool isCompletedTutorial = false;

  

    public void SetPopupMessgae( string Messgae,bool isCompletedTutorial) {

        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        Debug.Log("SpawnMessger");
        txt_Message.text = Messgae;
        this.isCompletedTutorial = isCompletedTutorial;

       
    }

    public void OnClick_TapToContinue() {
        if (!isCompletedTutorial) {
            TutorialHandler.instance.ClickToTapToConitue();
        }
        else {
            TutorialHandler.instance.Startgame();
        }
       
        
        Destroy(this.gameObject);
    }

   
}
