using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Panel_Counter : MonoBehaviour {

    [SerializeField] private GameObject[] all_GameObject;

    public void startCounter() {

        this.gameObject.SetActive(true);
        StartCoroutine(Counter_Start());
    }

    private IEnumerator Counter_Start() {

        for (int i = 0; i < all_GameObject.Length; i++) {
            all_GameObject[i].transform.localScale = Vector3.zero;
        }

        for (int i = 0; i < all_GameObject.Length; i++) {


            Sequence sq = DOTween.Sequence();
            sq.Append(all_GameObject[i].transform.DOScale(Vector3.one, 0.25f)).AppendInterval(0.5f).
                            Append(all_GameObject[i].transform.DOScale(Vector3.zero, 0.25f));

            yield return new WaitForSeconds(1);

        }

        GameManager.Instance.GetBallVeloctyAndStartGame();
    }
}
