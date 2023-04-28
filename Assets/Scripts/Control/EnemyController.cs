using SKGG.Attributes;
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
        protected List<Transform> players => PlayerController.players;

        private AttributeContainer attributeContainer;
        protected EnemyAttributes attributes { get => (EnemyAttributes)attributeContainer.attributes; }

        protected Vector2 targetLocation;
        protected Transform targettedPlayer;
        protected float idleTimer;
        protected float recheckTimer;
        private bool facingRight;

        protected Mover mover;
        protected Health health;

        void Awake()
        {
            attributeContainer = GetComponent<AttributeContainer>();
            mover = GetComponent<Mover>();
            health = GetComponent<Health>();
            health.OnDeath += OnDeath;
            targetLocation = transform.position;
        }

        void FixedUpdate()
        {
            recheckTimer -= Time.fixedDeltaTime;
            if(recheckTimer <= 0)
            {
                Recheck();
                recheckTimer = attributes.recheckTime;
            }

            if(targettedPlayer == null)
            {
                Wander();
            }
            else
            {
                Chase();
            }

            if(targetLocation.x != transform.position.x && (targetLocation.x > transform.position.x) != facingRight)
            {
                Flip();
            }
        }

        /// <summary>
        /// The enemy checks around itself to see what players it can see
        /// </summary>
        /// <remarks>
        /// This is called even if the ai is currently chasing someone, this should be used to also untarget players it can't see anymore
        /// </remarks>
        protected virtual void Recheck()
        {
            if (players == null || players.Count == 0)
                return;

            float targetDistance = targettedPlayer != null ? (targettedPlayer.position - transform.position).magnitude : attributes.attentionRange*2;
            if (targetDistance > attributes.attentionRange)
            {
                targettedPlayer = null;
            }
            foreach(Transform player in players)
            {
                float playerDistance = (player.position - transform.position).magnitude;
                if (playerDistance <= attributes.sightRange)
                {
                    if(playerDistance < targetDistance)
                    {
                        targettedPlayer = player;
                        targetDistance = playerDistance;
                    }
                }
            }
        }

        ///<summary>
        /// The enemy's behavior when it is not detecting anyone
        ///</summary>
        /// <remarks>
        /// Remember that this is called in FixedUpdate, so you should use Time.fixedDeltaTime instead of Time.deltaTime
        /// </remarks>
        protected virtual void Wander()
        {
            Vector2 targetOffset = targetLocation - (Vector2)transform.position;
            if (targetOffset.magnitude < attributes.maxWanderOffset)
            {
                Idle();
                idleTimer -= Time.fixedDeltaTime;
                if(idleTimer <= 0)
                {
                    FindNewWanderTarget();
                }
            }
            else
            {
                mover.targetVelocity = targetOffset.normalized * attributes.wanderSpeed;
                idleTimer = Mathf.Lerp(attributes.minWanderDelay, attributes.maxWanderDelay, Random.value);
            }
        }

        protected virtual void Idle()
        {
            targetLocation = transform.position; //prevent the alien from oscillating around its target location if it overshoots
            mover.targetVelocity = Vector2.zero;
        }

        protected virtual void FindNewWanderTarget()
        {
            float distance = Mathf.Lerp(attributes.minWanderDistance, attributes.maxWanderDistance, Random.value);
            Vector2 moveOffset;
            //25% chance to move in a random direction (100% if there's no players)
            //75% chance to move towards a random player
            if (Random.value < 0.25f || players == null || players.Count == 0)
            {
                Debug.Log("Moving in a random direction");
                if (Random.value < 0.5f)
                    moveOffset = Vector2.left * distance;
                else
                    moveOffset = Vector2.right * distance;
            }
            else
            {
                Debug.Log("Moving towards a random player");
                float[] playerWeights = new float[players.Count];
                float totalWeight = 0;
                for (int i = 0; i < players.Count; i++)
                {
                    float squarePlayerDistance = (players[i].position - transform.position).sqrMagnitude;
                    playerWeights[i] = 1 / squarePlayerDistance;
                    totalWeight += playerWeights[i];
                }
                float weight = Random.Range(0, totalWeight);
                int index = 0;
                while (weight > playerWeights[index])
                {
                    weight -= playerWeights[index];
                    index++;
                }
                Transform chosenPlayer = players[index];
                //25% chance for enemy to go away from selected player
                if ((chosenPlayer.position.x > transform.position.x) != (Random.value < 0.25f))
                {
                    moveOffset = Vector2.right * distance;
                }
                else
                {
                    moveOffset = Vector2.left * distance;
                }
            }
            targetLocation = (Vector2)transform.position + moveOffset;
        }

        /// <summary>
        /// The enemy's behavior when it detects someone
        /// </summary>
        /// <remarks>
        /// Remember that this is called in FixedUpdate, so you should use Time.fixedDeltaTime instead of Time.deltaTime
        /// </remarks>
        protected virtual void Chase()
        {
            targetLocation = targettedPlayer.position; //this variable would be different for different enemy types
            //the way target velocity is calculated is slightly problematic with the planets but enemy ai needs to be expanded anyways so I'm leaving it like this
            mover.targetVelocity = (targetLocation - (Vector2)transform.position).normalized * attributes.moveSpeed;
        }

        protected virtual void Flip()
        {
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

        protected virtual void OnDeath()
        {
            mover.targetVelocity = Vector2.zero;
            this.enabled = false;
        }

        private void OnValidate()
        {
            AttributeContainer attributeContainer = GetComponent<AttributeContainer>();
            if (attributeContainer == null)
                return;
            EnemyAttributes attributes = (EnemyAttributes)attributeContainer.attributes;
            StandardMelee melee = GetComponent<StandardMelee>();
            //different enemy types may remove the standard melee
            if (melee != null && attributes != null)
            {
                melee.damage = attributes.damage;
                melee.knockbackStrength = attributes.knockback;
            }
        }
    }
}
