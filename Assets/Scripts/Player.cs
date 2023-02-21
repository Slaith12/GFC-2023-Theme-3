using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    private float moveInput;

    [SerializeField] Rigidbody2D rb;

    //jump basics
    private bool facingRight = true;

    [SerializeField] bool isGrounded;
    [SerializeField] Transform groundCheck;
    [SerializeField] float checkRadius;
    [SerializeField] LayerMask whatIsGround;

    //extra jumps
    private int extraJumps;
    [SerializeField] int extraJumpsValue;


    [SerializeField] float jumpchange = 0.7f;

    //coyote time
    [SerializeField] float hangtime;
    private float hangCounter;

    //jump buffering
    [SerializeField] float jumpBufferLength = 0.15f;
    [SerializeField] float jumpBufferCount;

    [SerializeField] Attractable playerAttract;

    private float velY;

    void Start()
    {

        extraJumps = extraJumpsValue;
    }

    void FixedUpdate()
    {

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);


        if (isGrounded)
        {
            hangCounter = hangtime;
        }
        else
        {
            hangCounter -= Time.deltaTime;
        }


        float moveInput = Input.GetAxisRaw("Horizontal");

        // the raw part makes it more snappy and responsive, but can be removed from the Input.GetAxisRaw
        rb.velocity = new Vector2(moveInput * speed * Mathf.Sin(playerAttract.angle * Mathf.Deg2Rad + Mathf.PI), 
            rb.velocity.y);


        if ((facingRight == false && moveInput < 0) || (facingRight == true && moveInput > 0))
        {
            Flip();
            //CreateDust();
        }

    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            jumpBufferCount = jumpBufferLength;

        }
        else
        {
            jumpBufferCount -= Time.deltaTime;
        }


        if (isGrounded == true)
        {
            extraJumps = extraJumpsValue;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && extraJumps >= 0 || Input.GetKeyDown(KeyCode.W) && extraJumps >= 0)
        {
            Jump();
            extraJumps--;

        }
        else if (jumpBufferCount >= 0 && extraJumps > 0 && hangCounter > 0 || Input.GetKeyDown(KeyCode.W) && extraJumps > 0 && hangCounter > 0)
        {
            Jump();
            jumpBufferCount = 0;
        }

        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
        {
            /* 
            if (rb.velocity.y * Mathf.Cos(playerAttract.angle) > 0)
            {
                velY *= jumpchange;
                extraJumps--;
                //CreateDust();
            }*/

        }
    }

    void Jump() 
    {
        rb.AddForce(new Vector2 (jumpForce * Mathf.Sin(playerAttract.angle * Mathf.Deg2Rad + Mathf.PI),
            jumpForce * Mathf.Cos(playerAttract.angle * Mathf.Deg2Rad + Mathf.PI)));
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + rb.velocity);
    }


}
