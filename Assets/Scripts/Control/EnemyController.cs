using SKGG.Combat;
using SKGG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKGG.Control
{
    [RequireComponent(typeof(Mover), typeof(Health))]
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] float maxSpeed = 5;

        private Mover mover;
        private Health health;
        private Transform player;

        void Awake()
        {
            mover = GetComponent<Mover>();
            health = GetComponent<Health>();
            player = GameObject.FindGameObjectWithTag("Player").transform; //this won't work for multiplayer.
        }

        void FixedUpdate()
        {
            Vector3 targetLocation = player.position; //this variable would be different for different enemy types
                                                      //the way target velocity is calculated is slightly problematic with the planets but enemy ai needs to be expanded anyways so I'm leaving it like this
            mover.targetVelocity = (targetLocation - transform.position).normalized * maxSpeed;
            if (health.dead)
                mover.targetVelocity = Vector2.zero;
        }
    }
}
