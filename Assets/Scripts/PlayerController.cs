using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rigidbody2D;
    float horizontalInput;
    float verticalInput;
    Vector3 initialPosition;
    [SerializeField]
    int maxSpeed;
    [SerializeField]
    float moveForce;
    [SerializeField]
    float jumpForce;
    [SerializeField]
    float climbingSpeed;
    [SerializeField]
    Image playerImage;

    bool jumpAllowed = true;
    bool atLadder = false;
    bool isClimbing = false;
    private void Awake()
    {
        initialPosition = transform.position;
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //get input
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        //Move Right
        if (horizontalInput > 0)
        {
            transform.Translate(Vector3.right * horizontalInput * moveForce * Time.deltaTime);
            //rigidbody2D.AddForce(Vector2.right * moveForce, ForceMode2D.Impulse);
            //ClampSpeed();
            animator.SetBool("idle_b", false);
            animator.SetBool("right_b", true);
            playerImage.transform.eulerAngles = new Vector3(0, 0, 0);
        }

        //Move left
        else if (horizontalInput < 0)
        {
            transform.Translate(Vector3.right * horizontalInput * moveForce * Time.deltaTime);
            //rigidbody2D.AddForce(Vector2.left * moveForce, ForceMode2D.Impulse);
            //ClampSpeed();
            animator.SetBool("idle_b", false);
            animator.SetBool("left_b", true);
            playerImage.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            animator.SetBool("right_b", false);
            animator.SetBool("left_b", false);
            animator.SetBool("idle_b", true);
            //rigidbody2D.velocity = Vector2.ClampMagnitude(rigidbody2D.velocity, 0);
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpAllowed && !isClimbing)
        {
            transform.Translate(Vector3.up * jumpForce);
            StartCoroutine(AllowJump());

            //rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }


        if (verticalInput != 0 && atLadder)
        {
            isClimbing = true;
        }


    }

    private void FixedUpdate()
    {
        if (isClimbing)
        {
            rigidbody2D.gravityScale = 0;
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, verticalInput * climbingSpeed);
            GetComponent<CapsuleCollider2D>().isTrigger = true;
        }
        else
        {
            GetComponent<CapsuleCollider2D>().isTrigger = false;
            rigidbody2D.gravityScale = 4;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            atLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            atLadder = false;
            isClimbing = false;
        }
    }

    void ClampSpeed()
    {
        if (rigidbody2D.velocity.magnitude > maxSpeed)
            rigidbody2D.velocity = Vector2.ClampMagnitude(rigidbody2D.velocity, maxSpeed);
        if (rigidbody2D.velocity.magnitude < -maxSpeed)
            rigidbody2D.velocity = Vector2.ClampMagnitude(rigidbody2D.velocity, -maxSpeed);
    }

    public void ResetPlayer()
    {
        transform.position = initialPosition;
    }

    IEnumerator AllowJump()
    {
        jumpAllowed = false;
        yield return new WaitForSeconds(0.7f);
        jumpAllowed = true;
    }
}
