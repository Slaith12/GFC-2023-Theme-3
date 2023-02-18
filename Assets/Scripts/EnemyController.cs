using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Move all movement code to a separate Mover class to allow it to be easily reused
public class EnemyController : MonoBehaviour
{
    [SerializeField] int health = 10;
    private bool dead;

    [SerializeField] float maxSpeed;
    [SerializeField] float acceleration = 5;
    [SerializeField] Vector2 targetLocation;
    private Vector2 targetVelocity;

    private new Rigidbody2D rigidbody;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        targetVelocity = (targetLocation - (Vector2)transform.position).normalized * maxSpeed;
        if (dead)
            targetVelocity = Vector2.up * maxSpeed;
        Vector2 velocityDiff = targetVelocity - rigidbody.velocity;
        if (velocityDiff.magnitude < acceleration * Time.fixedDeltaTime)
        {
            rigidbody.velocity = targetVelocity;
        }
        else
        {
            rigidbody.velocity += velocityDiff.normalized * acceleration * Time.fixedDeltaTime;
        }
    }

    public void Damage(int damage, Vector2 knockback)
    {
        health -= damage;
        if (health <= 0)
            dead = true;
        rigidbody.velocity = knockback;
    }
}
