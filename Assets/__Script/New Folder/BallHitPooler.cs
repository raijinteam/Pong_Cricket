using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BallHitPooler : MonoBehaviour {

    public static BallHitPooler instance;

    [SerializeField] private ParticleSystem[] all_BallHitVfx;
    private static int index = 0;

    private void Awake() {
        instance = this;
    }

  

    internal void BallHitPooler_OnBallHitEffect(Vector2 point, Vector2 normalized) {
        Debug.Log("PLay Vfx");
        all_BallHitVfx[index].gameObject.SetActive(false);
        all_BallHitVfx[index].transform.position = point;
        float angle = MathF.Atan2(normalized.y, normalized.x) * Mathf.Rad2Deg;
        all_BallHitVfx[index].transform.localEulerAngles = new Vector3(angle, 90, 0);
        all_BallHitVfx[index].gameObject.SetActive(true);
        all_BallHitVfx[index].Play();

        index++;
        if (index >= all_BallHitVfx.Length) {
            index = 0;
        }
    }
}
