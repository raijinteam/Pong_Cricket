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
    private float flt_MinYVelocity = 2;
    private bool isHitRuuner = true;

    [SerializeField] private GameObject body;
    



   // Get RandomVelocity Ofball

   public void SetRandomVelocityOfBall() {
        this.gameObject.SetActive(true);
        StartCoroutine(DelayOfspawn());
       
    }

    private IEnumerator DelayOfspawn() {

        yield return new WaitForSeconds(1);
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        _velocity = new Vector2(2,-2).normalized;
        rb.AddForce(_velocity * flt_BallForce, ForceMode2D.Impulse);
    }

    private void FixedUpdate() {

        if (!GameManager.Instance.IsGameStart) {
            return;
        }

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
        if (collision.CompareTag(TagName.tag_Runner)) {

            if (isBatTouch && isHitRuuner) {
                GameManager.Instance.IncreasedRun(collision.GetComponent<Collder_Runner>().MyRunValue);
                isHitRuuner = false;
                StartCoroutine(DelayofTwoRunner());
            }
           
        }
        else if (collision.CompareTag(TagName.tag_Wicket)) {

          
            GameManager.Instance.IncreasedWicket();
       
        }
        else if (collision.CompareTag(TagName.tag_MaxRun)) {


            Debug.Log("MaxRunEnterd");
            GameManager.Instance.IncreasedRun(collision.GetComponent<Collder_Runner>().MyRunValue);

        }
        else if (collision.CompareTag(TagName.tag_Fielder)) {

            FielderHandler(collision);

        }
    }

    private void FielderHandler(Collider2D collision) {
        if (collision.TryGetComponent<Fielder>(out Fielder fielder)) {

            if (fielder.fielderState  == PlayerState.BatsMan && isBatTouch) {

                if (isTakeInvicible && !isPLayerTakeInVicilble) {
                    EnableBall();
                }
                return;
            }
            else if (fielder.fielderState == PlayerState.BatsMan && !isBatTouch) {

               

                ContactPoint2D[] contacts = new ContactPoint2D[1];
                int numContacts = collision.GetContacts(contacts);

                Vector2 direction = -new Vector2(transform.position.x, transform.position.y) + contacts[0].point;
                direction = direction.normalized;
                _velocity = direction.normalized;
                rb.velocity = Vector2.zero;

                isSwinging = false;
                //if (collision.transform.position.y - transform.position.y < 0) {
                //    direction = Vector2.right;
                //}
                rb.AddForce(direction * fielder.GetFielderForce(), ForceMode2D.Impulse);
            }
            else if (fielder.fielderState == PlayerState.Bowler && isBatTouch) {
                ContactPoint2D[] contacts = new ContactPoint2D[1];
                int numContacts = collision.GetContacts(contacts);

                Vector2 direction = -new Vector2(transform.position.x, transform.position.y) + contacts[0].point;
                direction = direction.normalized;
                _velocity = direction.normalized;
                rb.velocity = Vector2.zero;

                isSwinging = false;
                //if (collision.transform.position.y - transform.position.y < 0) {
                //    direction = Vector2.right;
                //}
                rb.AddForce(direction * fielder.GetFielderForce(), ForceMode2D.Impulse);
            }
            else if (fielder.fielderState == PlayerState.Bowler && !isBatTouch) {

                if (isTakeInvicible && isPLayerTakeInVicilble) {
                    EnableBall();
                }
                return;
            }

        }
    }

    private IEnumerator DelayofTwoRunner() {
        yield return new WaitForSeconds(0.2f);
        isHitRuuner = true;
    }


    // Collsion Deatection
    // Regid Body All Velocity Zero

    private void OnCollisionEnter2D(Collision2D collision) {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0;

        if (collision.gameObject.CompareTag(TagName.tag_Player)) {
            if (isTakeInvicible && !isPLayerTakeInVicilble) {
                EnableBall();
            }

            BallCollidedWithPlayerPaddle(collision);
                   
        }
        else if (collision.gameObject.CompareTag(TagName.tag_PlayerAi)) {

            if (isTakeInvicible && isPLayerTakeInVicilble) {
                EnableBall();
            }

            BallCollidedWithAIPaddle(collision);
           

        }
        else if (collision.gameObject.CompareTag(TagName.tag_SideWall)) {


            // If Touch Wall so Get Reflect Direction Of Touch when Swing is False
            // if Touch Wall So Calucte Swing Direction;
            WallTouchEffect(collision);

        }
        else if (collision.gameObject.CompareTag(TagName.tag_WallPower)) {
            PowerUpManager.Instance.PowerUpTheWall.IncreaseNumberOfShots();
            WallTouchEffect(collision);
        }
        


    }

    private void BallCollidedWithPlayerPaddle(Collision2D _collider) {

        if (_collider.gameObject.TryGetComponent<Player>(out Player player)) {

            Vector2 playerPoint = _collider.collider.transform.InverseTransformPoint(_collider.contacts[0].point);
            float playerForce = player.flt_GetBallForceAsPerCollsionForce(playerPoint);

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
        if (collision.transform.position.y - transform.position.y < 0) {
            direction = Vector2.right;
        }
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

    public void Resetball() {

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        transform.position = Vector3.zero;
        StopAllCoroutines();
        isHitRuuner = true;
        isBatTouch = false;
        body.gameObject.SetActive(true);
        isTakeInvicible = false;
        isPLayerTakeInVicilble = false;

    }

    [SerializeField] private bool isPLayerTakeInVicilble;
    [SerializeField] private bool isTakeInvicible;

    public void DisableBall(bool Value) {
        isTakeInvicible = true;
        isPLayerTakeInVicilble = Value;
        body.gameObject.SetActive(false);
    }
    public void EnableBall() {
        isTakeInvicible = true  ;
        isPLayerTakeInVicilble = false;
        body.gameObject.SetActive(true);
    }
}
