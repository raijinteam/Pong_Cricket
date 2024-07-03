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
    [SerializeField] private float flt_IntialForce;
    [SerializeField] private float flt_BallCurrentAmmount;
    [SerializeField] private float flt_BallIncreasedAmount;
    [SerializeField] private float flt_BallForce = 15;     // if Ball Force When Any Player Collide get This Force;
    [SerializeField] private float swingDir = 2;     // Swind Direction Value  - Our case We Need only X Direction
    [SerializeField] private float xMultiplier = 0.2f;  // force MultyPlier To Doing Extra Swing
    [SerializeField] private float flt_SwingForce = 40;   // if Bwllwer Hit The Ball Get Swing Force;
    [SerializeField] private bool isSwinging = false;  // Swing Status
    [SerializeField] private Vector2 _velocity;  // Ball Velocity
    private float flt_MinYVelocity = 5;
    private bool shouldWaitBeforeCollidingWithWallRuns = true;

   

    private Coroutine coro_InvicipbelPowerup;
    [SerializeField] private bool isInvisiblePowerUpTakenByPlayer;
    [SerializeField] private bool isInvisiblePowerupActive;

    // TEST //
    public float ballVelocity;

    // Get RandomVelocity Ofball

    [SerializeField] private GameObject body;
    [SerializeField] private ParticleSystem ps_ExpltionEffect;
    public Animator animator;

    public void PlayBallDestroyerEffect() {
        Debug.Log("Play Vfx");
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        isSwinging = false;
        body.gameObject.SetActive(false);


        //AudioManager.insatance.Play_BallDestroySound();
        Instantiate(ps_ExpltionEffect, transform.position, ps_ExpltionEffect.transform.rotation);
        transform.position = Vector3.zero;
    }


    public void SetRandomVelocityOfBall() {
        this.gameObject.SetActive(true);
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        _velocity = new Vector2(Random.Range(-2, 2), -2).normalized;
        rb.AddForce(_velocity * flt_IntialForce, ForceMode2D.Impulse);
        flt_BallForce = flt_IntialForce;
        flt_BallCurrentAmmount = 0;
        animator.enabled = true;

    }

    private void Start() {
        animator.enabled = false;
    }


    private void FixedUpdate() {

        SwingMotion();
       
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


               
                collision.GetComponent<Collder_Runner>().BallTouch();
                BoardHandler.instance.ActvetedFourPanel(collision.GetComponent<Collder_Runner>().MyRunValue);
                GameManager.Instance.spawnRunEffect(collision.GetComponent<Collder_Runner>().GetNoSprite(), collision.gameObject.transform.position);
                GameManager.Instance.IncreasedRun(collision.GetComponent<Collder_Runner>().MyRunValue);
                collision.GetComponent<Collder_Runner>().ChangeColor();
                shouldWaitBeforeCollidingWithWallRuns = false;

                if (this.gameObject.activeSelf) {
                    StartCoroutine(DelayofTwoRunner());
                }
                


            }
           
        }
        else if (collision.CompareTag(TagName.tag_Wicket)) {

            GameManager.Instance.IncreasedWicket();
            collision.GetComponent<Collder_Runner>().ChangeColor();


        }
        else if (collision.CompareTag(TagName.tag_MaxRun)) {
            Debug.Log("MaxRunEnterd");
            collision.GetComponent<Collder_Runner>().BallTouch();
            BoardHandler.instance.ActvetedSmash();
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
            GameManager.Instance.CurrentGamePlayerAI.playerAi.PLayerHitBall();
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
            GameManager.Instance.CurrentGamePlayerAI.playerAi.PLayerHitBall();
          
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
            GameManager.Instance.CurrentGamePlayerAI.playerAi.PLayerHitBall();

        }
        else if (collision.gameObject.CompareTag(TagName.tag_PlayerAi)) {

            HandleInvisiblePowerup(false);
            BallCollidedWithPlayerPaddle(collision);
            GameManager.Instance.CurrentGamePlayerAI.playerAi.BallHitWithPaddle();

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
                GameManager.Instance.CurrentGamePlayerAI.playerAi.PLayerHitBall();
              

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

        if (_collider.gameObject.TryGetComponent<PlayerData>(out PlayerData player)) {

            player.DesbleCollider();
            Vector2 playerPoint = _collider.collider.transform.InverseTransformPoint(_collider.contacts[0].point);
            float playerForce = player.playerCollsion.flt_GetBallForceAsPerCollsionForce(playerPoint);
            flt_BallForce = playerForce + flt_BallCurrentAmmount;
            flt_BallCurrentAmmount += flt_BallIncreasedAmount;
            if (player.MyState == PlayerState.BatsMan) {
                isBatTouch = true;

                BatsManBallTouchEffect(_collider, playerForce);
             
            }
            else {
                isBatTouch = false;

                flt_SwingForce = player.playerCollsion.flt_GetSwingForceAsPerCollsionForce(playerPoint);
                BallwerBallTouchEffcet(_collider, playerForce);
              
            }
          
        }
        // If Touch Bawler Calculate    Bawller OpsiteDirection And Get Swing Force
    }

   
    private void BallwerBallTouchEffcet(Collision2D collision, float _Force) {
        Vector2 direction = new Vector2(transform.position.x, transform.position.y) - collision.contacts[0].point;
        direction = direction.normalized;
        _velocity = direction.normalized;

        Vector2 absDirection = new Vector2(MathF.Abs(direction.x), MathF.Abs(direction.y));
        if (absDirection.y < 0.15f) {
            if (direction.y < 0) {
                direction.y = -0.2f;
            }
            else {
                direction.y = 0.2f;
            }

        }
        if (absDirection.x <= 0.1f) {

            if (direction.x < 0) {
                direction.x = -0.2f;
            }
            else {
                direction.x = 0.2f;
            }
        }
        direction.Normalize();

        rb.velocity = Vector2.zero;
        rb.AddForce(direction * _Force, ForceMode2D.Impulse);
        BallHitPooler.instance.BallHitPooler_OnBallHitEffect(collision.contacts[0].point, -direction);
        isSwinging = true;
        if (collision.transform.position.y - transform.position.y > 0) {
            isSwinging = false;
        }

    }

    private void BatsManBallTouchEffect(Collision2D collision, float _force) {
        
       
        Vector2 direction = new Vector2(transform.position.x, transform.position.y) - collision.contacts[0].point;
        direction = direction.normalized;
        _velocity = direction.normalized;

        Debug.Log("Batsman Ball - 1: " + direction);

        Vector2 absDirection = new Vector2(MathF.Abs(direction.x), MathF.Abs(direction.y));
        if (absDirection.y < 0.15f) {
            if (direction.y < 0) {
                direction.y = -0.2f;
            }
            else {
                direction.y = 0.2f;
            }

        }
        if (absDirection.x <= 0.1f) {

            if (direction.x < 0) {
                direction.x = -0.2f;
            }
            else {
                direction.x = 0.2f;
            }
        }

        Debug.Log("Batsman Ball - 2: " + direction);

        _velocity = direction.normalized;
        rb.velocity = Vector2.zero;

        isSwinging = false;
        //if (collision.transform.position.y - transform.position.y < 0) {
        //    direction = Vector2.right;
        //}

        Debug.Log("Batsman Ball - 3: " + _velocity);
        Debug.Log("Batsman Ball FORCE: " + _force);

        rb.AddForce(direction * _force, ForceMode2D.Impulse);
        BallHitPooler.instance.BallHitPooler_OnBallHitEffect(collision.contacts[0].point, -direction);
    }

    private void WallTouchEffect(Collision2D collision) {

        Vector2 forceDirection = Vector2.zero;
        if (isSwinging) {


            Vector2 direction = new Vector2(transform.position.x, transform.position.y) - collision.contacts[0].point;
            Debug.Log("Wall touch - 1 (Swinging): " + direction);
            Vector2 absDirection = new Vector2(Mathf.Abs(direction.x), Mathf.Abs(direction.y));
            if (absDirection.y < 0.15f) {
                if (direction.y < 0) {
                    direction.y = -0.2f;
                }
                else {
                    direction.y = 0.2f;
                }

            }
            if (absDirection.x <= 0.1f) {

                if (direction.x < 0) {
                    direction.x = -0.2f;
                }
                else {
                    direction.x = 0.2f;
                }
            }

            Debug.Log("Wall touch - 2 (Swinging): " + direction);
            forceDirection = direction.normalized;

            Debug.Log("Wall touch - 3 (Swinging): " + forceDirection);
            //forceDirection.y *= yMultiplier;
            //forceDirection.y = Mathf.Clamp(forceDirection.y, 0.1f, 0.4f);

            forceDirection = new Vector2(forceDirection.x * xMultiplier, forceDirection.y);
        }
        else {

            _velocity = Vector3.Reflect(_velocity.normalized, collision.contacts[0].normal);

            Debug.Log("Wall touch - 1: " + _velocity);

            Vector2 absDirection = new Vector2(Mathf.Abs(_velocity.x), Mathf.Abs(_velocity.y));
            if (absDirection.y < 0.15f) {
                if (_velocity.y < 0) {
                    _velocity.y = -0.2f;
                }
                else {
                    _velocity.y = 0.2f;
                }

            }
            if (absDirection.x <= 0.1f) {

                if (_velocity.x < 0) {
                    _velocity.x = -0.2f;
                }
                else {
                    _velocity.x = 0.2f;
                }
            }
            Debug.Log("Wall touch - 2: " + _velocity);
            _velocity.Normalize();

            Debug.Log("Wall touch - 3: " + _velocity);
            forceDirection = new Vector2(_velocity.x, _velocity.y);
        }

        Debug.Log("Wall Ball FORCE: " + flt_BallForce);
        rb.velocity = Vector2.zero;
        rb.AddForce(forceDirection * flt_BallForce, ForceMode2D.Impulse);
        BallHitPooler.instance.BallHitPooler_OnBallHitEffect(collision.contacts[0].point, forceDirection);

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
    public void EnableBall() {
        gameObject.SetActive(true);
        body.SetActive(true);
        animator.enabled = false;
    }
}
