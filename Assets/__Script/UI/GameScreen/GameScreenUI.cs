using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;



public class GameScreenUI : MonoBehaviour {

    [field: SerializeField] public Panel_Counter panel_Counter { get; set; }


    [SerializeField] private TextMeshProUGUI txt_Timer;
    [SerializeField] private TextMeshProUGUI txt_FirstInningScore;
    [SerializeField] private TextMeshProUGUI txt_ChasingTimescore;
    [SerializeField] private TextMeshProUGUI txt_ChasingTarget;
    [SerializeField] private GameObject obj_Chasing;
    [SerializeField] private GameObject Obj_NotChasing;


    [SerializeField] public GameObject obj_ShowSummryPanel;
    [SerializeField] private TextMeshProUGUI txt_SummryCurrentScoreText;
    [SerializeField] private TextMeshProUGUI txt_TargetSummary;

    [Header("PlayerData")]
    [SerializeField] private Image img_Player;
    [SerializeField] private GameObject bat;
    [SerializeField] private GameObject ball;


    [Header("PlayerAI")]
    [SerializeField] private Image img_PlayerAi;
    [SerializeField] private GameObject batsAi;
    [SerializeField] private GameObject ballAi;


    private int TargetRun = 0;
    private int TargetWicket = 0;
    private int CurrentRunForLerp = 0;
    private int CurrentWicketForLerp = 0;
    private float flt_RuneIncresedTime = 0.75f;

    private Coroutine coro_Run;

    private bool isFirstInnigComplted;


    [SerializeField] private RectTransform rect_Header;
    [SerializeField] private float flt_Animation;

    [Header("ScoreDotweendata")]
    [SerializeField] private float flt_Scale;
    [SerializeField] private float flt_ScaleanimationTime;
    [SerializeField] private float flt_PunchVector;
    [SerializeField] private float flt_PunchDuretion;


    [SerializeField] private Button btn_Rotate;





    private void OnEnable() {

        img_Player.sprite = DataManager.Instance.img_PlayerSprite;
        img_PlayerAi.sprite = DataManager.Instance.img_PlayerSpriteAi;
        isFirstInnigComplted = false;
        obj_Chasing.SetActive(isFirstInnigComplted);
        Obj_NotChasing.SetActive(!isFirstInnigComplted);

        rect_Header.anchoredPosition = new Vector2(0, 400);

        rect_Header.DOAnchorPos(Vector2.zero, flt_Animation).SetEase(Ease.OutBack);

    }
    public void ScaleDownAnimation(float time) {

        rect_Header.DOAnchorPosY(400, time).SetEase(Ease.OutBack);
    }



    private void Update() {

        txt_Timer.text = GameManager.Instance.flt_CurrnetGameTime.ToString("F0");

    }

    public void SetScore(int Run, int Wicket) {
        TargetRun = Run;
        TargetWicket = Wicket;

        if (Run == 0 && Wicket == 0) {
            txt_ChasingTimescore.text = 0.ToString("f0") + "/" + 0.ToString("f0");
            txt_FirstInningScore.text = 0.ToString("f0") + "/" + 0.ToString("f0");
            CurrentRunForLerp = 0;
            CurrentWicketForLerp = 0;
            return;
        }


        if (TargetWicket != CurrentWicketForLerp || TargetRun != CurrentRunForLerp) {

            StartTween();


        }
    }



    private void StartTween() {

        GameObject TargetObj = isFirstInnigComplted ? obj_Chasing : Obj_NotChasing;
        GameObject Text = isFirstInnigComplted ? txt_ChasingTimescore.gameObject : txt_FirstInningScore.gameObject;

        DOTween.Kill(Text.transform);
        DOTween.Kill(TargetObj.transform);
        Text.transform.localScale = Vector3.one;

        TargetObj.transform.localScale = Vector3.one;
        TargetObj.transform.DOPunchScale(Vector3.one * flt_PunchVector, flt_PunchDuretion, 10, 1);
        //Sequence seqScale = DOTween.Sequence();
        //seqScale.Append(Text.transform.DOScale(flt_Scale, flt_ScaleanimationTime).SetLoops(-1, LoopType.Yoyo));

        Text.transform.DOScale(flt_Scale, flt_ScaleanimationTime).SetLoops(-1, LoopType.Yoyo);

        //Invoke("EndTween", 2f);
        if (this.gameObject.activeSelf) {
            StartCoroutine(EndtweenProcess(Text.transform, TargetObj.transform));
        }

    }

    private IEnumerator EndtweenProcess(Transform _toKill, Transform TargetObj) {
        float flt_CurrentTime = 0;
        float Run = CurrentRunForLerp;
        float Wicket = CurrentWicketForLerp;

        // text scale animation set loop -1

        float loopTime = flt_RuneIncresedTime;
        if (TargetRun - Run <= 2 || TargetWicket - Wicket == 1) {
            loopTime /= 2;
        }

        while (flt_CurrentTime <= 1) {
            flt_CurrentTime += Time.deltaTime / loopTime;

            CurrentRunForLerp = ((int)Mathf.Lerp(Run, TargetRun, flt_CurrentTime));
            CurrentWicketForLerp = ((int)Mathf.Lerp(Wicket, TargetWicket, flt_CurrentTime));

            if (isFirstInnigComplted) {
                txt_ChasingTimescore.text = CurrentRunForLerp.ToString("f0") + "/" + CurrentWicketForLerp.ToString("f0");
            }
            else {
                txt_FirstInningScore.text = CurrentRunForLerp.ToString("f0") + "/" + CurrentWicketForLerp.ToString("f0");
            }


            yield return null;
        }

        DOTween.Kill(_toKill);
        DOTween.Kill(TargetObj);
        if (isFirstInnigComplted) {
            txt_ChasingTimescore.text = TargetRun.ToString("f0") + "/" + TargetWicket.ToString("f0");
        }
        else {
            txt_FirstInningScore.text = TargetRun.ToString("f0") + "/" + TargetWicket.ToString("f0");
        }

        // _toKill.localScale = Vector3.one;

        _toKill.DOScale(Vector3.one, flt_ScaleanimationTime / 1.5f).SetEase(Ease.Linear);
        TargetObj.DOScale(Vector3.one, flt_ScaleanimationTime / 1.5f).SetEase(Ease.Linear);
    }


    public void ShowSummeryScreen(int currentRun, int currentWicket) {

        obj_ShowSummryPanel.gameObject.SetActive(true);
        txt_SummryCurrentScoreText.text = $"{currentRun}/{currentWicket}";
        txt_TargetSummary.text = $"Target: {(currentRun + 1)}";
    }


    public void SetRoationData(bool isRotate) {


        GameManager.Instance.CurrentGamePlayer.player.BtnClick(isRotate);
    }

    public void setchaseRunData(int chasingRun) {
        txt_TargetSummary.text = chasingRun.ToString();
        SetInningData();
        isFirstInnigComplted = true;
        if (isFirstInnigComplted) {
            Debug.Log("Chasing run: " + chasingRun);
            txt_ChasingTarget.text = "Target: " + chasingRun.ToString();
        }

        obj_Chasing.SetActive(isFirstInnigComplted);
        Obj_NotChasing.SetActive(!isFirstInnigComplted);
    }

    public void SetInningData() {
        if (GameManager.Instance.CurrentGamePlayer.MyState == PlayerState.BatsMan) {

            bat.gameObject.SetActive(true);
            ball.gameObject.SetActive(false);
            ballAi.gameObject.SetActive(true);
            batsAi.gameObject.SetActive(false);
        }
        else {
            bat.gameObject.SetActive(false);
            ball.gameObject.SetActive(true);
            ballAi.gameObject.SetActive(false);
            batsAi.gameObject.SetActive(true);
        }
    }
}
