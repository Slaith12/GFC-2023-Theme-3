using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKGG.ObjectInfo
{
    [CreateAssetMenu(menuName = "ObjectInfo/Costumes/Player")]
    public class PlayerCostume : CharacterCostume
    {
        public Sprite hand1Closed;
        public Sprite hand1Open;
        public Sprite triggerHand;
        public Sprite hand2Closed;
        public Sprite hand2Open;
    }
}