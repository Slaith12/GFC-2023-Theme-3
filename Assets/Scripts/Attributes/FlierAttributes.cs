using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKGG.Attributes
{
    [CreateAssetMenu(menuName = "Attributes/Enemy/Flying")]
    public class FlierAttributes : EnemyAttributes
    {
        [Header("Flying Properties")]
        [Tooltip("How quickly the enemy moves while drifting")]
        public float driftSpeed = 2f;
        [Tooltip("How much the enemy drifts while sitting still.")]
        public float driftMagnitude = 1f;
        [Tooltip("How long the enemy drifts in a particular direction before drifting another direction")]
        public float driftPeriod = 0.25f;
        public float minWanderHeight = 6;
        public float maxWanderHeight = 10;
        [Tooltip("How close the enemy has to be in order to descend to attack. (Set negative to have it never descend)")]
        public float descentThreshold = 8;
    }
}