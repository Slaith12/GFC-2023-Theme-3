using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKGG.Attributes
{
    [CreateAssetMenu(menuName = "Attributes/Player")]
    public class PlayerAttributes : CharacterAttributes
    {
        [Header("Grabbing")]
        public float grabRange = 10;
        public float pullStrength = 1000;
        public float torqueStrength = 1;
        public float lookAheadTime = 0.1f;
    }
}