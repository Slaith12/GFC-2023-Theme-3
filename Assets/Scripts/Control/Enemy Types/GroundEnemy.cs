using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKGG.Control
{
    public class GroundEnemy : EnemyController
    {
        protected override void Idle()
        {
            targetLocation = transform.position; //prevent the alien from oscillating around its target location if it overshoots
            mover.targetVelocity = Vector2.zero;
        }

        protected override void FindNewWanderTarget()
        {
            float distance = Mathf.Lerp(attributes.minWanderDistance, attributes.maxWanderDistance, Random.value);
            Vector2 moveOffset;
            //75% chance to move in a random direction (100% if there's no players)
            //25% chance to move towards a random player
            if (Random.value < 0.75f || players == null || players.Count == 0)
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
                    moveOffset = Vector2.right * distance;
                }
                else
                {
                    moveOffset = Vector2.left * distance;
                }
            }
            targetLocation = (Vector2)transform.position + moveOffset;
        }
    }
}