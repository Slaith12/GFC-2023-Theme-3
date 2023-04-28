using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKGG.Attributes
{
    [CreateAssetMenu(menuName = "Attributes/Enemy")]
    public class EnemyAttributes : CharacterAttributes
    {
        [Header("Wandering")]
        [Tooltip("How fast the enemy moves around when it's not chasing anyone")]
        public float wanderSpeed = 4f;
        public float minWanderDistance = 5f;
        public float maxWanderDistance = 12f;
        [Tooltip("How far from its target location the enemy can stop when wandering")]
        public float maxWanderOffset = 0.75f;
        [Tooltip("Minimum time the enemy waits while wandering.")]
        public float minWanderDelay = 1.5f;
        [Tooltip("Maximum time the enemy waits while wandering.")]
        public float maxWanderDelay = 3f;

        [Header("Detection")]
        [Tooltip("How far away the enemy can detect new people")]
        public float sightRange = 5f;
        [Tooltip("How far away the enemy can follow people it's chasing")]
        public float attentionRange = 7f;
        [Tooltip("How often the enemy checks around itself for people")]
        public float recheckTime;

        [Header("Combat")]
        public int damage;
        public float knockback;
    }
}
