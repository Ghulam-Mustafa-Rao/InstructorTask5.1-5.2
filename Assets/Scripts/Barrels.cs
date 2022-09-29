using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrels : MonoBehaviour
{
    [SerializeField]
    private int maxSpeed;
    [SerializeField]
    private int initialForce;
    Rigidbody2D rigidbody2D;
    bool collisonLeftDone=false;
    bool collisonRightDone=false;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        //Add initial force
        rigidbody2D.AddForce(Vector2.left * initialForce, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < GameManager.gameManager.LeftBound.transform.position.x)
        {
            //Debug.LogError("Delete it");
            Destroy(this.gameObject);
        }   
    }

    private void FixedUpdate()
    {
        //Limit barrel speed
        //Debug.LogError(rigidbody2D.velocity.magnitude);
        if (rigidbody2D.velocity.magnitude > maxSpeed)
            rigidbody2D.velocity = Vector2.ClampMagnitude(rigidbody2D.velocity, maxSpeed);
        if (rigidbody2D.velocity.magnitude < -maxSpeed)
            rigidbody2D.velocity = Vector2.ClampMagnitude(rigidbody2D.velocity, -maxSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("LeftBound") && !collisonLeftDone)
        {
            collisonLeftDone = true;
            collisonRightDone = false;
           
            rigidbody2D.AddForce(Vector2.right * initialForce, ForceMode2D.Impulse);
        }

        if(collision.gameObject.CompareTag("RightBound") && !collisonRightDone)
        {
            collisonRightDone = true;
            collisonLeftDone = false;

            rigidbody2D.AddForce(Vector2.left * initialForce, ForceMode2D.Impulse);
        }

        if(collision.gameObject.CompareTag("Player"))
        {
            GameManager.gameManager.UpdateLives();
        }
    }
}
