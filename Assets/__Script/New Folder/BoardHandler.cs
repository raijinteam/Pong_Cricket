using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardHandler : MonoBehaviour {


    public static BoardHandler instance;

    [Header("Ground Handler")]
    [SerializeField] private Transform pf_Board;
    [SerializeField] private Sprite sprite_50;
    [SerializeField] private Sprite sprite100;
    [SerializeField] private Sprite sprite_NoBoardSmash;
    [SerializeField] private Sprite sprite_OutNoBord;
    [SerializeField] private float flt_ScaleMutiplayer;
    [SerializeField] private float flt_ShownScaleAnimationTime;
    [SerializeField] private int Rotation;
    [SerializeField] private float flt_OutAnimation;
    [SerializeField] private float flt_ShowOutAnimation;

    [Header("50 And 100 Animation")]
    [SerializeField] private float flt_AnimtionTimeForRunner;
    [SerializeField] private float flt_ScaleMutiplayerRunner;
    [SerializeField] private float flt_ShownScaleAnimationTimeRunner;
    [SerializeField] private float flt_Shownpostion;
    [SerializeField] private float flt_EndPostion;
    [SerializeField] private float flt_StartPostion;



    [Header("Board data")]
    [SerializeField] private Sprite sprite_Six;
    [SerializeField] private Sprite sprite_Four;
    [SerializeField] private Sprite sprite_Smash;
    [SerializeField] private Sprite sprite_Out;
    [SerializeField] private Sprite sprite_CleanBold;


    [Header("shake data")]
    [SerializeField] private float flt_ShakeTime;
    [SerializeField] private int shakeVibrato;
    [SerializeField] private float shake_Stregeth;
    [SerializeField] private float shake_Randomness;


    public event Action<Sprite> OnPlayBoardAnimation;

    private void Awake() {
        instance = this;
    }

    public void ActvetedFourPanel(int myRunValue) {
        if (myRunValue == 4) {
             OnPlayBoardAnimation?.Invoke(sprite_Four);
        }
        else if (myRunValue == 6) {
            OnPlayBoardAnimation?.Invoke(sprite_Six);
        }
       
    }

   
    public void ActivetedOutpanel() {
        int index = Random.Range(0, 100);
        if (index <= 50) {
            OnPlayBoardAnimation?.Invoke(sprite_CleanBold);
        }
        else {
            OnPlayBoardAnimation?.Invoke(sprite_Out);
        }
        SpawnOutandSmashEffectInGround(true);
    }

    private void SpawnOutandSmashEffectInGround(bool isOut) {

        Transform current = Instantiate(pf_Board, pf_Board.transform.position, Quaternion.identity);
        if (isOut) {
            current.GetComponent<SpriteRenderer>().sprite = sprite_OutNoBord;
        }
        else {
            current.GetComponent<SpriteRenderer>().sprite = sprite_NoBoardSmash;
        }

        Sequence sq1 = DOTween.Sequence();
        Vector3 postion = Camera.main.transform.position;
        sq1.Append(Camera.main.transform.DOShakePosition(flt_ShakeTime, shake_Stregeth, shakeVibrato,
                        shake_Randomness));
        sq1.AppendCallback(() => {

            Camera.main.transform.position = postion;
        });


        current.localScale = Vector3.zero;
        Sequence sq = DOTween.Sequence();


        sq.Append(current.DOScale(Vector3.one * flt_ScaleMutiplayer, flt_ShowOutAnimation));
        sq.Join(current.DORotate(Vector3.forward * Rotation * 360, flt_ShowOutAnimation * 1.5f, RotateMode.FastBeyond360));
        sq.Append(current.DOScale(Vector3.one * (flt_ScaleMutiplayer + 0.25f), flt_ShowOutAnimation).SetLoops(5, LoopType.Yoyo));
        sq.AppendCallback(() => { GameManager.Instance.SpawnBallWicketTime(); });
        sq.Append(current.DOScale(Vector3.zero, flt_OutAnimation));
        sq.Join(current.DORotate(Vector3.back * Rotation * 360, flt_OutAnimation * 2, RotateMode.FastBeyond360));
        sq.AppendCallback(() => { Destroy(current.gameObject); });

        sq.Play();
    }

    public void ActvetedSmash() {
        OnPlayBoardAnimation?.Invoke(sprite_Smash);
        SpawnOutandSmashEffectInGround(false);
    }

   
 

    public  void Actveted50RunEffect() {

        Transform current = Instantiate(pf_Board, transform.position, Quaternion.identity);
        current.GetComponent<SpriteRenderer>().sprite = sprite_50;
        RuunerAnimation(current);


    }

    public void Actveted100RunEffect() {
        Transform current = Instantiate(pf_Board, transform.position, Quaternion.identity);
        current.GetComponent<SpriteRenderer>().sprite = sprite100;
        RuunerAnimation(current);
    }

    private void RuunerAnimation(Transform current) {

        current.localScale = Vector3.zero;

        current.localPosition = new Vector3(0, flt_StartPostion, 0);

        Sequence sq = DOTween.Sequence();
        sq.Append(current.DOLocalMoveY(flt_Shownpostion, flt_AnimtionTimeForRunner/ 2).SetEase(Ease.OutBack));
        sq.Join(current.DOScale(Vector3.one* flt_ScaleMutiplayerRunner, flt_AnimtionTimeForRunner).SetEase(Ease.OutBack));
        sq.Append(current.DOScale(Vector3.one * (flt_ScaleMutiplayerRunner + 0.4f ) , flt_ShownScaleAnimationTimeRunner).SetLoops(8, LoopType.Yoyo));
        sq.Append(current.DOScale(Vector3.zero, (flt_AnimtionTimeForRunner/ 2)).SetEase(Ease.InBack));
        sq.Join(current.DOLocalMoveY(flt_EndPostion, flt_AnimtionTimeForRunner/ 2).SetEase(Ease.InBack));
        sq.AppendCallback(() => { Destroy(current.gameObject); });

        //sq.Play();
    }
}

