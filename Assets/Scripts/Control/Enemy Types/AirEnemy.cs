using SKGG.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKGG.Control
{
    public class AirEnemy : EnemyController
    {
        protected new FlierAttributes attributes => (FlierAttributes)base.attributes;

        private float driftTimer;
        protected Vector2 driftOffset;

        protected float hoverHeight;

        protected override void Init()
        {
            base.Init();
            hoverHeight = attributes.minWanderHeight;
        }

        ///<inheritdoc/>
        protected override void Idle()
        {
            driftTimer -= Time.fixedDeltaTime;
            if(driftTimer <= 0)
            {
                driftOffset = Random.insideUnitCircle * attributes.driftMagnitude;
                driftTimer = attributes.driftPeriod;
            }
            Vector2 moveOffset = (targetLocation + driftOffset) - (Vector2)transform.position;
            mover.targetVelocity = moveOffset.normalized * attributes.driftSpeed;
        }

        protected override void FindNewWanderTarget()
        {
            float distance = Mathf.Lerp(attributes.minWanderDistance, attributes.maxWanderDistance, Random.value);
            float xOffset;
            //75% chance to move in a random direction (100% if there's no players)
            //25% chance to move towards a random player
            if (Random.value < 0.75f || players == null || players.Count == 0)
            {
                Debug.Log("Moving in a random direction");
                if (Random.value < 0.5f)
                    xOffset = -distance;
                else
                    xOffset = distance;
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
                float weight = Random.value * totalWeight;
                int index = 0;
                while (weight > playerWeights[index])
                {
                    weight -= playerWeights[index];
                    index++;
                }
                Transform chosenPlayer = players[index];
                if (chosenPlayer.position.x > transform.position.x)
                {
                    xOffset = distance;
                }
                else
                {
                    xOffset = -distance;
                }
            }
            float heightAboveGround = Mathf.Lerp(attributes.minWanderHeight, attributes.maxWanderHeight, Random.value);
            targetLocation.x = transform.position.x + xOffset;
            targetLocation.y = heightAboveGround + FindGroundHeight(targetLocation.x, minSpaceAbove: heightAboveGround);
            hoverHeight = targetLocation.y;
        }

        protected float FindGroundHeight(float xPos, float minSpaceAbove = 0)
        {
            return -15;
        }

        protected override void GetChaseTarget()
        {
            base.GetChaseTarget();
            if (Mathf.Abs(targetLocation.x - transform.position.x) > attributes.descentThreshold)
            {
                targetLocation.y = hoverHeight;
            }
        }
    }
}