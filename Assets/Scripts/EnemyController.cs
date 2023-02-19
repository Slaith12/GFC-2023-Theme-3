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
    private Vector2 targetVelocity;

    private new Rigidbody2D rigidbody;
    private Transform player;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform; //this won't work for multiplayer.
    }

    void FixedUpdate()
    {
        Vector3 targetLocation = player.position;
        targetVelocity = (targetLocation - transform.position).normalized * maxSpeed;
        if (dead)
            targetVelocity = Vector2.up * maxSpeed - Physics2D.gravity;
        Vector2 velocityDiff = targetVelocity - rigidbody.velocity;
        Debug.Log($"Target Velocity: {targetVelocity} Acceleration Direction: {velocityDiff}");
        if (velocityDiff.magnitude < acceleration * Time.fixedDeltaTime)
        {
            rigidbody.velocity = targetVelocity;
            Debug.Log("Setting velocity to target");
        }
        else
        {
            rigidbody.AddForce(velocityDiff.normalized * acceleration * rigidbody.mass);
        }
    }

    public void Damage(int damage, Vector2 knockback)
    {
        health -= damage;
        if (health <= 0)
        {
            dead = true;
            acceleration += Physics2D.gravity.magnitude;
        }
        rigidbody.velocity = knockback;
    }
}
