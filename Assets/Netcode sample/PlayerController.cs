using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{
    // basic controls
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    private float moveInput;

    private Rigidbody2D rb;

    //jump basics
    private bool facingRight = true;

    [SerializeField] bool isGrounded;
    [SerializeField] Transform groundCheck;
    [SerializeField] float checkRadius;
    [SerializeField] LayerMask whatIsGround;

    //extra jumps
    private int extraJumps;
    [SerializeField] int extraJumpsValue;

    //gravity change
    [SerializeField] float gravUp;
    [SerializeField] float gravDown;

    [SerializeField] float jumpchange = 0.7f;

    //coyote time
    [SerializeField] float hangtime;
    private float hangCounter;

    //jump buffering
    [SerializeField] float jumpBufferLength = 0.15f;
    [SerializeField] float jumpBufferCount;

    void Start()
    {

        extraJumps = extraJumpsValue;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if(!IsOwner)
            return;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);


        if (isGrounded)
        {
            hangCounter = hangtime;
            rb.gravityScale = gravUp;
        }
        else
        {
            hangCounter -= Time.deltaTime;
        }



        float moveInput = Input.GetAxisRaw("Horizontal");

        // the raw part makes it more snappy and responsive, but can be removed from the Input.GetAxisRaw
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if ((facingRight == false && moveInput < 0) || (facingRight == true && moveInput > 0))
        {
            Flip();
            //CreateDust();
        }

    }

    void Update()
    {
        if (!IsOwner)
            return;

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
            rb.velocity = Vector2.up * jumpForce;
            extraJumps--;

        }
        else if (jumpBufferCount >= 0 && extraJumps > 0 && hangCounter > 0 || Input.GetKeyDown(KeyCode.W) && extraJumps > 0 && hangCounter > 0)
        {
            rb.velocity = Vector2.up * jumpForce;
            jumpBufferCount = 0;
        }

        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
        {
            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpchange);
                extraJumps--;
                //CreateDust();
            }

        }

        if (!isGrounded && (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W) || rb.velocity.y == 0)) {
            rb.gravityScale = gravDown;
        }
    }



    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

}
