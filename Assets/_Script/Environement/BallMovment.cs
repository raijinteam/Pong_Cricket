using DG.Tweening.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallMovment : MonoBehaviour {


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

    private Coroutine coro_InvicipbelPowerup;
    [SerializeField] private bool isInvisiblePowerUpTakenByPlayer;
    [SerializeField] private bool isInvisiblePowerupActive;

    // TEST //
    public float ballVelocity;

    // Get RandomVelocity Ofball

    public void SetRandomVelocityOfBall() {
        this.gameObject.SetActive(true);
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        _velocity = new Vector2(2, -2).normalized;
        rb.AddForce(_velocity * flt_BallForce, ForceMode2D.Impulse);

    }

   

    private void FixedUpdate() {

        if (!GameManager.Instance.IsGameRunning) {
            return;
        }

        SwingMotion();


        ballVelocity = rb.velocity.magnitude;
    }



    // this is Run When Swing isTrue, this Function Calculate Swing Direction And Aply Regid Body
    // We Apply onlt X because We Need Circuler Motion In Horizontal;
    private void SwingMotion() {
        if (isSwinging) {
            swingDir = rb.velocity.x + flt_SwingForce * Time.fixedDeltaTime;

            float y_Velocity = rb.velocity.y;
            if(y_Velocity < flt_MinYVelocity) {
                y_Velocity = flt_MinYVelocity;
            }
            rb.velocity = new Vector2(swingDir, y_Velocity);     
        }
    }




    private void OnTriggerEnter2D(Collider2D collision) {

        if (!GameManager.Instance.IsGameRunning) {
            return;
        }
        if (collision.CompareTag(TagName.tag_Runner)) {

            if (isBatTouch && shouldWaitBeforeCollidingWithWallRuns) {


                Debug.Log("RunIncreased");
                collision.GetComponent<Collder_Runner>().BallTouch();
                GameManager.Instance.IncreasedRun(collision.GetComponent<Collder_Runner>().MyRunValue);
                shouldWaitBeforeCollidingWithWallRuns = false;

                if (this.gameObject.activeSelf) {
                    StartCoroutine(DelayofTwoRunner());
                }
                


            }
           
        }
        else if (collision.CompareTag(TagName.tag_Wicket)) {

           
            GameManager.Instance.IncreasedWicket();
       
        }
        else if (collision.CompareTag(TagName.tag_MaxRun)) {
            Debug.Log("MaxRunEnterd");
            collision.GetComponent<Collder_Runner>().BallTouch();
            GameManager.Instance.IncreasedRun(collision.GetComponent<Collder_Runner>().MyRunValue);
           
        }
        else if (collision.CompareTag(TagName.tag_Fielder)) {

            FielderHandler(collision);

        }
        else if (collision.CompareTag(TagName.tag_PinBallPaddle)) {
            PinBallPaddle(collision);
        }
        else if (collision.CompareTag(TagName.tag_PowerUP)) {

            PlayerState myState = PlayerState.BatsMan;
            if (!isBatTouch) {
                myState = PlayerState.Bowler;
            }
            bool isPLayer = false;

            if (isBatTouch) {
                if (GameManager.Instance.CurrentGamePlayer.MyState == PlayerState.BatsMan) {
                    isPLayer = true;
                }
                else {
                    isPLayer = false;
                }
            }
            else {
                if (GameManager.Instance.CurrentGamePlayer.MyState == PlayerState.Bowler) {
                    isPLayer = true;
                }
                else {
                    isPLayer = false;
                }
            }
           
            PowerUpManager.Instance.PowerupCollected(isPLayer, myState,collision.gameObject);
        }
    }

    private void PinBallPaddle(Collider2D collision) {



        if (PowerUpManager.Instance.PowerUpPinBall.hasPlayerActivatedPowerup) {
            GameManager.Instance.CurrentGamePlayerAI.PLayerHitBall();
        }

        EnableBallIfInvisible();

        if (PowerUpManager.Instance.PowerUpPinBall.CanHitBowl(isBatTouch)) {

            if (!isBatTouch) {
                isBatTouch = true;
            }
            else {
                isBatTouch = false;
            }

            ContactPoint2D[] contacts = new ContactPoint2D[1];
           
            Vector2 direction = -new Vector2(transform.position.x, transform.position.y) + contacts[0].point;
            direction = direction.normalized;
            _velocity = direction.normalized;
            rb.velocity = Vector2.zero;

            isSwinging = false;

            flt_BallForce = PowerUpManager.Instance.PowerUpPinBall.GetPinBallForce(flt_BallForce);
            rb.AddForce(direction * flt_BallForce, ForceMode2D.Impulse);
        }


    }

    private void FielderHandler(Collider2D collision) {


        if (PowerUpManager.Instance.PowerUpFielder.hasPlayerActivatedPowerup) {
            GameManager.Instance.CurrentGamePlayerAI.PLayerHitBall();
          
        }
        EnableBallIfInvisible();

        if (PowerUpManager.Instance.PowerUpFielder.CanHitBowl(isBatTouch)) {

            if (!isBatTouch) {
                isBatTouch = true;
            }
            else {
                isBatTouch = false;
            }


            Debug.Log("Hit fielder");
            ContactPoint2D[] contacts = new ContactPoint2D[1];
            int numContacts = collision.GetContacts(contacts);

            Vector2 direction = -new Vector2(transform.position.x, transform.position.y) + contacts[0].point;
            direction = direction.normalized;
            _velocity = direction.normalized;
            rb.velocity = Vector2.zero;

            isSwinging = false;
            flt_BallForce = PowerUpManager.Instance.PowerUpFielder.GetFielderForce();
            rb.AddForce(direction * flt_BallForce, ForceMode2D.Impulse);
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

        if (collision.gameObject.CompareTag(TagName.tag_Player)) {

            HandleInvisiblePowerup(true);
            BallCollidedWithPlayerPaddle(collision);
                   
        }
        else if (collision.gameObject.CompareTag(TagName.tag_PlayerAi)) {

            HandleInvisiblePowerup(false);
            BallCollidedWithAIPaddle(collision);
           
        }
        else if (collision.gameObject.CompareTag(TagName.tag_SideWall)) {


            // If Touch Wall so Get Reflect Direction Of Touch when Swing is False
            // if Touch Wall So Calucte Swing Direction;
            if (body.gameObject.activeSelf == false && coro_InvicipbelPowerup == null) {
               coro_InvicipbelPowerup =  StartCoroutine(delay_Enble());
            }
            WallTouchEffect(collision);

        }
        else if (collision.gameObject.CompareTag(TagName.tag_WallPower)) {

            isSwinging = false;

            if (PowerUpManager.Instance.PowerUpTheWall.hasPlayerActivatedPowerup) {
                GameManager.Instance.CurrentGamePlayerAI.PLayerHitBall();
              

                if (GameManager.Instance.CurrentGamePlayer.MyState == PlayerState.BatsMan) {
                    isBatTouch = true;
                }
                else {
                    isBatTouch = false;
                }
            }
            else {

                if (GameManager.Instance.CurrentGamePlayerAI.MyState == PlayerState.BatsMan) {
                    isBatTouch = true;
                }
                else {
                    isBatTouch = false;
                }
            }

            EnableBallIfInvisible();
            WallTouchEffect(collision);
        }
        


    }

    private void BallCollidedWithPlayerPaddle(Collision2D _collider) {

        if (_collider.gameObject.TryGetComponent<Player>(out Player player)) {

            Vector2 playerPoint = _collider.collider.transform.InverseTransformPoint(_collider.contacts[0].point);
            float playerForce = player.flt_GetBallForceAsPerCollsionForce(playerPoint);
            flt_BallForce = playerForce;

            if (player.MyState == PlayerState.BatsMan) {
                isBatTouch = true;

                BatsManBallTouchEffect(_collider, playerForce);
                GameManager.Instance.CurrentGamePlayerAI.PLayerHitBall();
            }
            else {
                isBatTouch = false;

                flt_SwingForce = player.flt_GetSwingForceAsPerCollsionForce(playerPoint);
                BallwerBallTouchEffcet(_collider, playerForce);
                GameManager.Instance.CurrentGamePlayerAI.PLayerHitBall();
            }
        }
        // If Touch Bawler Calculate    Bawller OpsiteDirection And Get Swing Force
    }

    private void BallCollidedWithAIPaddle(Collision2D _collider) {
        // If Touch BatsMan Calculate  Bat OpsiteDirection AndGetForce
        if (_collider.gameObject.TryGetComponent<PlayerAI>(out PlayerAI player)) {

            Vector2 playerPoint = _collider.collider.transform.InverseTransformPoint(_collider.contacts[0].point);
            float playerForce = player.flt_GetBallForceAsPerCollsionForce(playerPoint);
            flt_BallForce = playerForce;

            if (player.MyState == PlayerState.BatsMan) {
                isBatTouch = true;


                BatsManBallTouchEffect(_collider, playerForce);
            }
            else {
                isBatTouch = false;
                flt_SwingForce = player.flt_GetSwingForceAsPerCollsionForce(playerPoint);
                BallwerBallTouchEffcet(_collider, playerForce);
            }

            player.BallHitWithPaddle();
        }
    }

    private void BallwerBallTouchEffcet(Collision2D collision, float _Force) {
        Vector2 direction = new Vector2(transform.position.x, transform.position.y) - collision.contacts[0].point;
        direction = direction.normalized;
        _velocity = direction.normalized;
        rb.AddForce(direction * _Force, ForceMode2D.Impulse);
        //isSwinging = true;
        //if (collision.transform.position.y - transform.position.y > 0) {
        //    isSwinging = false;
        //}
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


        rb.AddForce(forceDirection * flt_BallForce, ForceMode2D.Impulse);
       
    }

    private IEnumerator delay_Enble() {
        body.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        body.gameObject.SetActive(false);
        coro_InvicipbelPowerup = null;
    }

    public void Resetball() {
        Debug.Log("Resetball Resetball");
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        isSwinging = false;
        transform.position = Vector3.zero;    
        
        EnableBallIfInvisible();
        StopAllCoroutines();
        shouldWaitBeforeCollidingWithWallRuns = true;

        isBatTouch = false;
        coro_InvicipbelPowerup = null; 
    }

    public void ActivateInvisiblePowerup(bool _IsTakenByPlayer) {
        isInvisiblePowerUpTakenByPlayer = _IsTakenByPlayer;
        isInvisiblePowerupActive = true;
    }

    public void DisableInvisiblePowerup() {
        isInvisiblePowerupActive = false;
    }

    private void HandleInvisiblePowerup(bool _isHitByPlayer) {
        if (!isInvisiblePowerupActive) {
            return;
        }

        if (isInvisiblePowerUpTakenByPlayer) {
            if (_isHitByPlayer) {
                DisableBallIfInvisiblePowerupActive();
            }
            else {
                EnableBallIfInvisible();
            }
        }
        else {

            if (_isHitByPlayer) {
                EnableBallIfInvisible();
                
            }
            else {
                DisableBallIfInvisiblePowerupActive();
            }
        }
        
    }

    private void EnableBallIfInvisible() {

        if (!isInvisiblePowerupActive) {
            return;
        }

        if (coro_InvicipbelPowerup != null) {
            StopCoroutine(coro_InvicipbelPowerup);
            coro_InvicipbelPowerup = null;
        }
        body.gameObject.SetActive(true);
    }

    private void DisableBallIfInvisiblePowerupActive() {
        body.gameObject.SetActive(false);
    }
}
