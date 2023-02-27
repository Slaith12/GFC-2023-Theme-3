using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpForce;

    private new Rigidbody2D rigidbody;

    private bool facingRight = true;

    //ground check
    [SerializeField] bool isGrounded;
    [SerializeField] Transform groundCheck;
    [SerializeField] float checkRadius;
    [SerializeField] LayerMask whatIsGround;

    //I don't think we need coyote time and jump buffering since jumping isn't important at all in this game, these mechanics are meant more for platformers
    //I'm keeping these here anyway because there's no harm in keeping them
    //coyote time
    [SerializeField] float hangtime;
    private float hangTimer;

    //jump buffering
    [SerializeField] float jumpBufferLength = 0.15f;
    [SerializeField] float jumpBufferTimer;

    [SerializeField] Attractable playerAttract;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if (isGrounded)
        {
            hangTimer = hangtime;
        }
        else
        {
            hangTimer -= Time.deltaTime;
        }


        float moveInput = Input.GetAxis("Horizontal");

        rigidbody.velocity = new Vector2(moveInput * speed * Mathf.Sin(playerAttract.angle * Mathf.Deg2Rad + Mathf.PI), 
            rigidbody.velocity.y);

        /*despite everything i've tried, jumping seems to still only affect the y direction
        /i tried a few things out changing the above line which didn't work out - the issue is due to the x velocity
        resetting every frame, but I don't know how to make it properly add the velocity in the x direction
        also in general there are some issues with the jump*/

        //there is also the issue of sin(0)=0 -> you can't jump at 0 degrees, or move left and right at 90 degrees


        if ((facingRight == false && moveInput < 0) || (facingRight == true && moveInput > 0))
        {
            Flip();
            //CreateDust();
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            jumpBufferTimer = jumpBufferLength;
        }
        else
        {
            jumpBufferTimer -= Time.deltaTime;
        }

        if (jumpBufferTimer >= 0 && hangTimer > 0)
        {
            Jump();
            jumpBufferTimer = 0;
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
        rigidbody.velocity = (new Vector2 (jumpForce * Mathf.Sin(playerAttract.angle * Mathf.Deg2Rad - Mathf.PI),
            -jumpForce * Mathf.Cos(playerAttract.angle * Mathf.Deg2Rad + Mathf.PI)));

        //main issues are with the jumping
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
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + rigidbody.velocity);
    }


}
