using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKGG.Attributes
{
    [CreateAssetMenu(menuName ="Attributes/Character")]
    public class CharacterAttributes : AttributeList
    {
        [Header("Standard Movement")]
        public float moveSpeed = 5;
        public float acceleration = 10;
        [Tooltip("Can the character move in both the x and y directions or only the x direction?")]
        public bool canFly = false;
    }
}