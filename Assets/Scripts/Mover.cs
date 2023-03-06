using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Attractable))]
public class Mover : MonoBehaviour
{
    [SerializeField] float acceleration = 5;
    [Tooltip("Can the character move in both the x and y directions or only the x direction?")]
    [SerializeField] bool canFly;

    [HideInInspector] public Vector2 targetVelocity;
    public Vector2 relativeVelocity {
        get => attractable.WorldToRelativeOffset(rigidbody.velocity);
        set => rigidbody.velocity = attractable.RelativeToWorldOffset(value);
    }

    private new Rigidbody2D rigidbody;
    private Attractable attractable;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        attractable = GetComponent<Attractable>();
    }

    void FixedUpdate()
    {
        Vector2 velocityDiff = targetVelocity - relativeVelocity;
        //non-flying characters can't change their y velocity normally
        if (!canFly)
            velocityDiff.y = 0;

        if (velocityDiff.magnitude < acceleration * Time.fixedDeltaTime)
        {
            relativeVelocity += velocityDiff;
        }
        else
        {
            relativeVelocity += acceleration * Time.fixedDeltaTime * velocityDiff.normalized; //apparently this ordering improves performance according to VS.
        }
    }

    public void Knockback(Vector2 knockback)
    {
        relativeVelocity = knockback;
    }

    public void Jump(float jumpPower)
    {
        relativeVelocity = new Vector2(relativeVelocity.x, jumpPower);
    }
}
