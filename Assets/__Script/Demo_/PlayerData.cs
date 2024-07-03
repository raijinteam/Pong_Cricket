using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {

    public PlayerState MyState;
   


    // Compononant
    public PlayerCollsionHandler playerCollsion;
    public Player player;
    public PlayerAI playerAi;
    [SerializeField] private Collider2D myCollider;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private MMF_Player mmf_PlayerScaleup;
    [SerializeField] private MMF_Player mmf_PlayerScaleDown;



    public void SetPlayerState(PlayerState _myState) {
        this.MyState = _myState;

        int index = CharacterManager.Instance.currentSelectedCharacter;
        sr.sprite = CharacterManager.Instance.GetCharacterIcon(index);
       
        playerCollsion.SetPlayerForceData(index);

        if (player != null) player.SetValueOfClampPosition();
        if (playerAi != null) playerAi.SetValueOfClampPosition();
        PlayScaleUpAnimation();

    }


   

    public void ExtendPadle(float _ScaleIncrease) {

        transform.localScale += Vector3.one * _ScaleIncrease;
        if (player != null) player.SetValueOfClampPosition();
        if (playerAi != null) playerAi.SetValueOfClampPosition();
    }

    public void ResetScale(float _ScaleIncrease) {
        transform.localScale -= Vector3.one * _ScaleIncrease;
        if (player != null) player.SetValueOfClampPosition();
        if (playerAi != null) playerAi.SetValueOfClampPosition();
    }

    public void PlayScaleUpAnimation() {

        mmf_PlayerScaleup.PlayFeedbacks();
    }
    public void PlayScaleDownAnimation() {

        mmf_PlayerScaleDown.PlayFeedbacks();
    }

   
    public void DesbleCollider() {
        myCollider.enabled = false;
        StartCoroutine(Delay_OfSomeSecond());
    }

    private IEnumerator Delay_OfSomeSecond() {
        yield return new WaitForSeconds(0.2f);
        myCollider.enabled = true;
    }
}
