using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

namespace SKGG.ObjectInfo
{
    [CreateAssetMenu(menuName = "ObjectInfo/Costumes/Standard Character")]
    public class CharacterCostume : ScriptableObject
    {
        public AnimatorController animator;
        public Sprite head;
        public Sprite torso;
        public Sprite foot1;
        public Sprite foot2;
    }
}