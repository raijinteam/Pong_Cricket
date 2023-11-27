using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScorePoint : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI txt_Score;
    private int myValue;
    public void SetData(int Score) {
        myValue = Score;
        if (Score > 0) {
            txt_Score.text = "+ "+Score.ToString();
        }
        else {
            txt_Score.text =  Score.ToString();
        }
        
        
    }


    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.gameObject.CompareTag(TagName.tag_Ball)) {

            if (myValue > 0) {
                GameManager.Instance.IncreasedRun(myValue);
            }
            else {

                Debug.Log("Decreased Run");
                GameManager.Instance.DecreasedRun(-myValue);
            }
            
            PowerUpManager.Instance.PowerupRandomizer.DestroyPoint(this);
        }
    }
}
