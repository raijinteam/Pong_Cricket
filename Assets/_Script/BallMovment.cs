using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallMovment : MonoBehaviour {


    [Header("Component")]
    [SerializeField] private Transform child;
    [SerializeField] private Rigidbody2D rb;


    private bool isBatTouch = false;

    [SerializeField] private float flt_BallForce = 15;     // if Ball Force When Any Player Collide get This Force;
    [SerializeField] private float swingDir = 2;     // Swind Direction Value  - Our case We Need only X Direction
    [SerializeField] private float xMultiplier = 0.2f;  // force MultyPlier To Doing Extra Swing
    [SerializeField] private float flt_SwingForce = 40;   // if Bwllwer Hit The Ball Get Swing Force;
    [SerializeField] private bool isSwinging = false;  // Swing Status
    [SerializeField] private Vector2 _velocity;  // Ball Velocity
    private float flt_MinYVelocity = 2;
    



   // Get RandomVelocity Ofball

   public void SetRandomVelocityOfBall() {
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

            if (isBatTouch) {
                GameManager.Instance.IncreasedRun(collision.GetComponent<Collder_Runner>().MyRunValue);
            }
           
        }
        else if (collision.CompareTag(TagName.tag_Wicket)) {

          
            GameManager.Instance.IncreasedWicket();
            GetComponent<Collider2D>().enabled = false;
           

        }
        else if (collision.CompareTag(TagName.tag_MaxRun)) {


            Debug.Log("MaxRunEnterd");
            GameManager.Instance.IncreasedRun(collision.GetComponent<Collder_Runner>().MyRunValue);
            GetComponent<Collider2D>().enabled = false;

        }
    }


    // Collsion Deatection
    // Regid Body All Velocity Zero



    private void OnCollisionEnter2D(Collision2D collision) {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0;

        if (collision.gameObject.CompareTag(TagName.tag_Player)) {

            if (collision.gameObject.TryGetComponent<Player>(out Player player)) {

                if (player.MyState == PlayerState.BatsMan) {
                    isBatTouch = true;
                    BatsManBallTouchEffect(collision);
                }
                else {
                    isBatTouch = false;
                     BallwerBallTouchEffcet(collision);
                }
            }
            // If Touch Bawler Calculate    Bawller OpsiteDirection And Get Swing Force
           
        }
        else if (collision.gameObject.CompareTag(TagName.tag_PlayerAi)) {

            // If Touch BatsMan Calculate  Bat OpsiteDirection AndGetForce
            if (collision.gameObject.TryGetComponent<PlayerAI>(out PlayerAI player)) {

                if (player.MyState == PlayerState.BatsMan) {
                    isBatTouch = true;
                    BatsManBallTouchEffect(collision);
                }
                else {
                    isBatTouch = false;
                    BallwerBallTouchEffcet(collision);
                }
            }

        }
        else if (collision.gameObject.CompareTag(TagName.tag_UpDownWall)) {
            

            _velocity = Vector3.Reflect(_velocity.normalized, collision.contacts[0].normal);
            if (_velocity.magnitude < 0.5f) {
                _velocity = new Vector2(1, 0);
            }

            isSwinging = false;
            
            rb.AddForce(_velocity * flt_BallForce, ForceMode2D.Impulse);
        }
        else {

           
            // If Touch Wall so Get Reflect Direction Of Touch when Swing is False
            // if Touch Wall So Calucte Swing Direction;
            WallTouchEffect(collision);

        }

    }

    private void BallwerBallTouchEffcet(Collision2D collision) {
        Vector2 direction = new Vector2(transform.position.x, transform.position.y) - collision.contacts[0].point;
        direction = direction.normalized;

        Vector2 playerPoint = collision.collider.transform.InverseTransformPoint(collision.contacts[0].point);

        float playerForce = 0;


        if (collision.gameObject.TryGetComponent<Player>(out Player Player)) {
            playerForce = Player.flt_GetBallForceAsPerCollsionForce(playerPoint);
            flt_SwingForce = Player.flt_GetSwingForceAsPerCollsionForce(playerPoint);
        }
        else if (collision.gameObject.TryGetComponent<PlayerAI>(out PlayerAI PlayerAI)) {
            playerForce = PlayerAI.flt_GetBallForceAsPerCollsionForce(playerPoint);
            flt_SwingForce = PlayerAI.flt_GetSwingForceAsPerCollsionForce(playerPoint);
           
        }

      

      
      

        rb.AddForce(direction * playerForce, ForceMode2D.Impulse);
        isSwinging = true;
        if (collision.transform.position.y - transform.position.y > 0) {
            isSwinging = false;
        }
    }

    private void BatsManBallTouchEffect(Collision2D collision) {
        Vector2 direction = new Vector2(transform.position.x, transform.position.y) - collision.contacts[0].point;
        direction = direction.normalized;
        _velocity = direction.normalized;
        rb.velocity = Vector2.zero;

        Vector2 playerPoint = collision.collider.transform.InverseTransformPoint(collision.contacts[0].point);
        float playerForce = 0;  


        if (collision.gameObject.TryGetComponent<Player>(out Player Player)) {
             playerForce = Player.flt_GetBallForceAsPerCollsionForce(playerPoint);
        }
        else if (collision.gameObject.TryGetComponent<PlayerAI>(out PlayerAI PlayerAI)) {
             playerForce = PlayerAI.flt_GetBallForceAsPerCollsionForce(playerPoint);
            PlayerAI.BallHitWithPaddle();
        }

        isSwinging = false;
        if (collision.transform.position.y - transform.position.y < 0) {
            direction = Vector2.right;
        }


        rb.AddForce(direction * playerForce, ForceMode2D.Impulse);
       

       
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
}
