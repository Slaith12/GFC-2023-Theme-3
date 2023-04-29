using SKGG.Attributes;
using SKGG.Combat;
using SKGG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKGG.Control
{
    [RequireComponent(typeof(Mover), typeof(Health))]
    public abstract class EnemyController : MonoBehaviour
    {
        protected List<Transform> players => PlayerController.players;

        private AttributeContainer attributeContainer;
        protected EnemyAttributes attributes { get => (EnemyAttributes)attributeContainer.attributes; }

        protected Vector2 targetLocation;
        protected Transform targettedPlayer;
        public virtual bool targettingPlayer { get => targettedPlayer != null; }
        protected bool isIdle;
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
            Init();
        }

        protected virtual void Init()
        {

        }

        void FixedUpdate()
        {
            recheckTimer -= Time.fixedDeltaTime;
            if (recheckTimer <= 0)
            {
                Recheck();
                recheckTimer = attributes.recheckTime;
            }

            if (targettedPlayer == null)
            {
                Wander();
            }
            else
            {
                Chase();
            }

            if (!isIdle && (targetLocation.x != transform.position.x && (targetLocation.x > transform.position.x) != facingRight))
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

            float targetDistance = targettedPlayer != null ? (targettedPlayer.position - transform.position).magnitude : attributes.attentionRange * 2;
            if (targetDistance > attributes.attentionRange)
            {
                targettedPlayer = null;
            }
            foreach (Transform player in players)
            {
                float playerDistance = (player.position - transform.position).magnitude;
                if (playerDistance <= attributes.sightRange)
                {
                    if (playerDistance < targetDistance)
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
            if (targetOffset.magnitude < attributes.minWanderOffset)
            {
                isIdle = true;
            }
            else if (targetOffset.magnitude > attributes.maxWanderOffset)
            {
                isIdle = false;
            }

            if(isIdle)
            {
                Idle();
                idleTimer -= Time.fixedDeltaTime;
                if (idleTimer <= 0)
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

        ///<summary>
        /// The enemy's behavior when it is not moving anywhere
        ///</summary>
        /// <remarks>
        /// Remember that this is called in FixedUpdate, so you should use Time.fixedDeltaTime instead of Time.deltaTime
        /// </remarks>
        protected abstract void Idle();
        protected abstract void FindNewWanderTarget();

        /// <summary>
        /// The enemy's behavior when it detects someone
        /// </summary>
        /// <remarks>
        /// Remember that this is called in FixedUpdate, so you should use Time.fixedDeltaTime instead of Time.deltaTime
        /// </remarks>
        protected virtual void Chase()
        {
            isIdle = false;
            GetChaseTarget();
            //the way target velocity is calculated is slightly problematic with the planets but enemy ai needs to be expanded anyways so I'm leaving it like this
            mover.targetVelocity = (targetLocation - (Vector2)transform.position).normalized * attributes.moveSpeed;
        }

        protected virtual void GetChaseTarget()
        {
            targetLocation = targettedPlayer.position; //this variable would be different for different enemy types
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
    }
}