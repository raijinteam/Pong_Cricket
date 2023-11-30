using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_UI : MonoBehaviour {

    [SerializeField] private GameObject panel_HorizontalMotion;
    [SerializeField] private GameObject panel_RotationMotion;
    [SerializeField] private GameObject panel_MiddleMotion;
    [SerializeField] private GameObject panel_ScoringSystem;
    [SerializeField] private GameObject panel_BowlingSystem;

   

    private void OnEnable() {
        TutorialHandler.instance.tutorialChange += ChangeState;
    }

    private void ChangeState(Tutorial_State obj) {
        HideAllPanel();
        switch (obj) {

            case Tutorial_State.learnHorizonatlMovement:
                panel_HorizontalMotion.SetActive(true);
               
                break;
            case Tutorial_State.learnRotationMotion:
                panel_RotationMotion.SetActive(true);
              
                break;
            case Tutorial_State.learnMiddleofRun:
                panel_MiddleMotion.SetActive(true);
               
                break;
            case Tutorial_State.LearnScoreingSytem:
                panel_ScoringSystem.SetActive(true);
             
                break;
            case Tutorial_State.LearnBowling:
                panel_BowlingSystem.SetActive(true);
              
                break;
            default:
                break;
        }
    }

    private void HideAllPanel() {

        panel_HorizontalMotion.SetActive(false);
        panel_RotationMotion.SetActive(false);
        panel_BowlingSystem.SetActive(false);
        panel_MiddleMotion.SetActive(false);
        panel_ScoringSystem.SetActive(false);
        
    }

    private void OnDisable() {
        TutorialHandler.instance.tutorialChange -= ChangeState;
    }

   
}
