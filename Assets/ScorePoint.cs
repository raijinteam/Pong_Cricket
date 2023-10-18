using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScorePoint : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI txt_Score;
    private int myValue;
    public void SetData(int Score) {
        if (Score > 0) {
            txt_Score.text = "+ "+Score.ToString();
        }
        else {
            txt_Score.text =  Score.ToString();
        }
        
        
    }


    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.gameObject.CompareTag(TagName.tag_Ball)) {
            GameManager.Instance.IncreasedRun(myValue);
            PowerUpManager.Instance.PowerupRandomizer.DestroyPoint(this);
        }
    }
}
