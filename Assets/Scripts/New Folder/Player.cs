using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpForce;

    private bool facingRight = true;

    private Vector2 relativeVelocity 
    { get => playerAttract.WorldToRelativeOffset(rigidbody.velocity); 
      set => rigidbody.velocity = playerAttract.RelativeToWorldOffset(value); }

    //ground check
    [SerializeField] bool isGrounded;
    [SerializeField] Transform groundCheck;
    [SerializeField] float checkRadius;
    [SerializeField] LayerMask whatIsGround;

    private bool jumpInput;

    private Attractable playerAttract;
    private new Rigidbody2D rigidbody;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        playerAttract = GetComponent<Attractable>();
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        float moveInput = Input.GetAxis("Horizontal");
        relativeVelocity = new Vector2(moveInput * speed, relativeVelocity.y);

        if(jumpInput)
        {
            if (isGrounded)
            {
                Jump();
            }
            jumpInput = false;
        }


        if ((facingRight == false && moveInput < 0) || (facingRight == true && moveInput > 0))
        {
            Flip();
            //CreateDust();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {
            jumpInput = true; //jumping is handled in FixedUpdate but GetKeyDown works better in Update
        }
    }

    void Jump()
    {
        relativeVelocity = new Vector2 (relativeVelocity.x, jumpForce);
        Debug.Log($"Jump velocity {rigidbody.velocity}");
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
        if(Application.isPlaying)
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + rigidbody.velocity);
    }
}
