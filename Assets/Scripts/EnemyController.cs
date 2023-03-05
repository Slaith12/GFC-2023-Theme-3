using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] int health = 10;
    private bool dead;

    [SerializeField] float maxSpeed;

    private new Rigidbody2D rigidbody;
    private Mover mover;
    private Transform player;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        mover = GetComponent<Mover>();
        player = GameObject.FindGameObjectWithTag("Player").transform; //this won't work for multiplayer.
    }

    void FixedUpdate()
    {
        Vector3 targetLocation = player.position; //this variable would be different for different enemy types
        mover.targetVelocity = (targetLocation - transform.position).normalized * maxSpeed; //this line would be the same for all enemies
        if (dead)
            mover.targetVelocity = Vector2.zero;
    }

    public void Damage(int damage, Vector2 knockback)
    {
        health -= damage;
        if (health <= 0)
        {
            dead = true;
        }
        mover.Knockback(knockback);
    }
}
