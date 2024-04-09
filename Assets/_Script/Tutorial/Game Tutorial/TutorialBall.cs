using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TutorialBall : MonoBehaviour {

    [Header("Component")]
    [SerializeField] private Transform child;
    [SerializeField] private Rigidbody2D rb;


    private bool isBatTouch = false;   // if Batsman Hit Bat So Run Calculation OtherWise No RunCalculation 

    [SerializeField] private float flt_BallForce = 15;     // if Ball Force When Any Player Collide get This Force;
    [SerializeField] private float swingDir = 2;     // Swind Direction Value  - Our case We Need only X Direction
    [SerializeField] private float xMultiplier = 0.2f;  // force MultyPlier To Doing Extra Swing
    [SerializeField] private float flt_SwingForce = 40;   // if Bwllwer Hit The Ball Get Swing Force;
    [SerializeField] private bool isSwinging = false;  // Swing Status
    [SerializeField] private Vector2 _velocity;  // Ball Velocity
    private float flt_MinYVelocity = 5;
    private bool shouldWaitBeforeCollidingWithWallRuns = true;

    [SerializeField] private GameObject body;


    // TEST //
    public float ballVelocity;

    // Get RandomVelocity Ofball


    public void setRandomDirection() {

        isBatTouch = false;
        Vector2 direction = new Vector2(0,1);
        _velocity = direction;
        rb.AddForce(direction*flt_BallForce, ForceMode2D.Impulse);
    }


    private void FixedUpdate() {

        SwingMotion();


        ballVelocity = rb.velocity.magnitude;
    }



    // this is Run When Swing isTrue, this Function Calculate Swing Direction And Aply Regid Body
    // We Apply onlt X because We Need Circuler Motion In Horizontal;
    private void SwingMotion() {
        if (isSwinging) {
            swingDir = rb.velocity.x + flt_SwingForce * Time.fixedDeltaTime;

            float y_Velocity = rb.velocity.y;
            if (y_Velocity < flt_MinYVelocity) {
                y_Velocity = flt_MinYVelocity;
            }
            rb.velocity = new Vector2(swingDir, y_Velocity);
        }
    }




    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag(TagName.tag_Runner)) {

            if (isBatTouch && shouldWaitBeforeCollidingWithWallRuns) {

                TutorialHandler.instance.IncreasedRun(collision.GetComponent<Collder_Runner>().MyRunValue);
                shouldWaitBeforeCollidingWithWallRuns = false;
                StartCoroutine(DelayofTwoRunner());
            }

        }
        else if (collision.CompareTag(TagName.tag_Wicket)) {

          

        }
        else if (collision.CompareTag(TagName.tag_MaxRun)) {

           
           
        }
        
    }

   

    private IEnumerator DelayofTwoRunner() {
        yield return new WaitForSeconds(0.2f);
        shouldWaitBeforeCollidingWithWallRuns = true;
    }


    // Collsion Deatection
    // Regid Body All Velocity Zero

    private void OnCollisionEnter2D(Collision2D collision) {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0;


        if (collision.gameObject.CompareTag(TagName.tag_SideWall)) {


            // If Touch Wall so Get Reflect Direction Of Touch when Swing is False
            // if Touch Wall So Calucte Swing Direction;

            if (TutorialHandler.instance.CurrentTutorialState == Tutorial_State.learnRotationMotion) {
                TutorialHandler.instance.PlayerHitRotation();
            }
            
           

                WallTouchEffect(collision);

        }
        else if (collision.gameObject.CompareTag(TagName.tag_TutorialPlayer)) {
            isBatTouch = true;
            if (TutorialHandler.instance.CurrentTutorialState == Tutorial_State.learnHorizonatlMovement) {
                TutorialHandler.instance.PlayerHorizontalHitTouch();
            }
            else if (TutorialHandler.instance.CurrentTutorialState == Tutorial_State.LearnBowling) {
                TutorialHandler.instance.PlayerBowlingTouch();
            }
            BallCollidedWithPlayerPaddle( collision);
        }

        else if (collision.gameObject.CompareTag(TagName.tag_TutorialPlayerAI)) {
            isBatTouch = false;
            BallCollidedWithAIPaddle(collision);
        }
        else if (isBatTouch) {
            if (TutorialHandler.instance.CurrentTutorialState == Tutorial_State.learnMiddleofRun) {

                TutorialHandler.instance.MiddleHitBall();
            }
            else {
                Destroy(this.gameObject);
                TutorialHandler.instance.SpawnBall();
            }
        }
        else {
            Destroy(this.gameObject);
            TutorialHandler.instance.SpawnBall();
        }

    }

    private void BallCollidedWithPlayerPaddle(Collision2D _collider) {

        if (_collider.gameObject.TryGetComponent<tutorial_Player>(out tutorial_Player player)) {

            Vector2 playerPoint = _collider.collider.transform.InverseTransformPoint(_collider.contacts[0].point);
            if (TutorialHandler.instance.CurrentTutorialState == Tutorial_State.learnMiddleofRun) {
              
                float playerForce = player.flt_GetBallForceAsPerCollsionForce(playerPoint);
                flt_BallForce = playerForce;
            }

           

            if (player.MyState == PlayerState.BatsMan) {
                isBatTouch = true;

                BatsManBallTouchEffect(_collider, flt_BallForce);
               
            }
            else {
                isBatTouch = false;
                flt_SwingForce = player.flt_GetSwingForceAsPerCollsionForce(playerPoint);
               
                BallwerBallTouchEffcet(_collider, flt_SwingForce);
              
            }
        }
        // If Touch Bawler Calculate    Bawller OpsiteDirection And Get Swing Force
    }

    private void BallCollidedWithAIPaddle(Collision2D _collider) {
        // If Touch BatsMan Calculate  Bat OpsiteDirection AndGetForce
        if (_collider.gameObject.TryGetComponent<Tutorial_PlayerAI>(out Tutorial_PlayerAI player)) {

            Vector2 playerPoint = _collider.collider.transform.InverseTransformPoint(_collider.contacts[0].point);
            float playerForce = 15;
            flt_BallForce = playerForce;

            if (player.MyState == PlayerState.BatsMan) {
                isBatTouch = true;


                BatsManBallTouchEffect(_collider, playerForce);
            }
            else {
                isBatTouch = false;
                flt_SwingForce = 15;
                BallwerBallTouchEffcet(_collider, playerForce);
            }

          
        }
    }

    private void BallwerBallTouchEffcet(Collision2D collision, float _Force) {
        Vector2 direction = new Vector2(transform.position.x, transform.position.y) - collision.contacts[0].point;
        direction = direction.normalized;
        _velocity = direction.normalized;
        rb.AddForce(direction * _Force, ForceMode2D.Impulse);
        isSwinging = true;
        if (collision.transform.position.y - transform.position.y > 0) {
            isSwinging = false;
        }
    }

    private void BatsManBallTouchEffect(Collision2D collision, float _force) {


        Vector2 direction = new Vector2(transform.position.x, transform.position.y) - collision.contacts[0].point;
        direction = direction.normalized;
        _velocity = direction.normalized;
        rb.velocity = Vector2.zero;

        isSwinging = false;
        //if (collision.transform.position.y - transform.position.y < 0) {
        //    direction = Vector2.right;
        //}

        rb.AddForce(direction * _force, ForceMode2D.Impulse);
    }

    private void WallTouchEffect(Collision2D collision) {

        Vector2 forceDirection = Vector2.zero;
        if (isSwinging) {

            Vector2 direction = new Vector2(transform.position.x, transform.position.y) - collision.contacts[0].point;
            if (direction.y == 0) {
                direction.y = 0.5f;
            }
            forceDirection = direction.normalized;

            //forceDirection.y *= yMultiplier;
            //forceDirection.y = Mathf.Clamp(forceDirection.y, 0.1f, 0.4f);

            forceDirection = new Vector2(forceDirection.x * xMultiplier, forceDirection.y);
        }
        else {

            _velocity = Vector3.Reflect(_velocity.normalized, collision.contacts[0].normal);

            forceDirection = new Vector2(_velocity.x, _velocity.y);
        }

        Debug.Log("Ball Force Add");
        rb.AddForce(forceDirection * flt_BallForce, ForceMode2D.Impulse);

    }

  

  

   
}
