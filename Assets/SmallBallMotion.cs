using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallBallMotion : MonoBehaviour {
    [Header("Component")]
    [SerializeField] private Transform child;
    [SerializeField] private Rigidbody2D rb;


    private bool isBatTouch = false;   // if Batsman Hit Bat So Run Calculation OtherWise No RunCalculation 
    [SerializeField] private float flt_BallForce = 15;     // if Ball Force When Any Player Collide get This Force;
     private Vector2 velocity;  // Ball Velocity
    private bool isHitRuuner = true;

  



    // Get RandomVelocity Ofball

   

    public void SetRandomVelocityOfBall(Vector3 _velocity) {
        this.gameObject.SetActive(true);
        StartCoroutine(DelayOfspawn(_velocity));

    }

    private IEnumerator DelayOfspawn(Vector3 _velocity) {

        yield return new WaitForSeconds(1);
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        this.velocity = _velocity.normalized;
        rb.AddForce(velocity * flt_BallForce, ForceMode2D.Impulse);
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
            StopAllCoroutines();
            Destroy(gameObject);

        }
        else if (collision.CompareTag(TagName.tag_MaxRun)) {


            Debug.Log("MaxRunEnterd");
            GameManager.Instance.IncreasedRun(collision.GetComponent<Collder_Runner>().MyRunValue);
            StopAllCoroutines();
            Destroy(gameObject);

        }
        else if (collision.CompareTag(TagName.tag_Fielder)) {

            Destroy(gameObject);
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

            StopAllCoroutines();
            Destroy(gameObject);


        }
        else if (collision.gameObject.CompareTag(TagName.tag_PlayerAi)) {

            StopAllCoroutines();
            Destroy(gameObject);

        }
        else if (collision.gameObject.CompareTag(TagName.tag_SideWall)) {


            // If Touch Wall so Get Reflect Direction Of Touch when Swing is False
            // if Touch Wall So Calucte Swing Direction;
            WallTouchEffect(collision);

        }
        else if (collision.gameObject.CompareTag(TagName.tag_WallPower)) {
           
            WallTouchEffect(collision);
            StopAllCoroutines();
            Destroy(gameObject);
        }



    }

   
    private void WallTouchEffect(Collision2D collision) {

        Vector2 forceDirection = Vector2.zero;
        velocity = Vector3.Reflect(velocity.normalized, collision.contacts[0].normal);

        forceDirection = new Vector2(velocity.x, velocity.y);


        rb.AddForce(forceDirection * flt_BallForce, ForceMode2D.Impulse);
    }

   

}

