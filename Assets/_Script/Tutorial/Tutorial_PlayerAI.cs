using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_PlayerAI : MonoBehaviour {

    [SerializeField] private Transform left_Postion;
    [SerializeField] private Transform right_Postion;

    [SerializeField] private Transform left_Bowler;
    [SerializeField] private Transform right_Bowler;


    [SerializeField] private float flt_MovementSpeed;
   
    private float flt_MinClampPostion;
    private float flt_maxClampPostion;

   
   

    public PlayerState MyState { get; internal set; }

    public void SetClampPostion() {


        flt_MinClampPostion = (left_Postion.position.x + transform.localScale.x / 2);
        flt_maxClampPostion = (right_Postion.position.x - transform.localScale.x / 2);
        transform.position = new Vector3(transform.position.x, Camera.main.orthographicSize - 7.5f, transform.position.z);
    }

    public void SetBollwerPostion() {

        flt_MinClampPostion = (left_Bowler.position.x + transform.localScale.x / 2);
        flt_maxClampPostion = (right_Bowler.position.x - transform.localScale.x / 2);
        transform.position = new Vector3(transform.position.x, -Camera.main.orthographicSize + 4.5f, transform.position.z);
       
       
        transform.rotation = Quaternion.identity;

    }


    private void Update() {

        ChansingBall();

    }

    private void ChansingBall() {
        if (TutorialHandler.instance.CurrentBall == null) {
            return;
        }

        Vector3 direction = (TutorialHandler.instance.CurrentBall.transform.position - transform.position).normalized;
        direction = new Vector3(direction.x, 0, 0).normalized;

        transform.Translate(direction * flt_MovementSpeed * Time.deltaTime, Space.World);

        float X = transform.position.x;

        X = Mathf.Clamp(X, flt_MinClampPostion, flt_maxClampPostion);

        transform.position = new Vector3(X, transform.position.y, transform.position.z);
    }
}
 