using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Min_BallMovement : MonoBehaviour {

    [Header("Component")]

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float flt_BallForce = 15;     // if Ball Force When Any Player Collide get This Force;
   
    [SerializeField] private Vector2 _velocity;  // Ball Velocity
  
   

  
    // TEST //
    public float ballVelocity;

    // Get RandomVelocity Ofball

    public void SetRandomVelocityOfBall() {
        this.gameObject.SetActive(true);
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        
        _velocity = new Vector2(2, 2).normalized;
        rb.AddForce(_velocity * flt_BallForce, ForceMode2D.Impulse);

    }



    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.CompareTag(TagName.tag_Pickup)) {

            Destroy(collision.gameObject);

            Mini_GameManager.instance.SpawnOneCollectable();
            Mini_GameManager.instance.AddCoin();
        }
        
    }






    // Collsion Deatection
    // Regid Body All Velocity Zero

    private void OnCollisionEnter2D(Collision2D collision) {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0;

        if (collision.gameObject.CompareTag(TagName.tag_Player)) {


            BatsManBallTouchEffect(collision);

        }
        else if (collision.gameObject.CompareTag(TagName.tag_SideWall)) {


            WallTouchEffect(collision);

        }
        else {
            Destroy(gameObject);
            Mini_GameManager.instance.SpawnNewBall();
        }
        



    }

   

  
  

    private void BatsManBallTouchEffect(Collision2D collision) {

        if (collision.gameObject.TryGetComponent<Mini_Player >(out Mini_Player player)) {

            Vector2 direction = new Vector2(transform.position.x, transform.position.y) - collision.contacts[0].point;
            direction = direction.normalized;
            _velocity = direction.normalized;
            rb.velocity = Vector2.zero;



            rb.AddForce(direction * flt_BallForce, ForceMode2D.Impulse);
        }
       
    }

    private void WallTouchEffect(Collision2D collision) {


        _velocity = Vector3.Reflect(_velocity.normalized, collision.contacts[0].normal);

          rb.AddForce(_velocity * flt_BallForce, ForceMode2D.Impulse);

    }


}
