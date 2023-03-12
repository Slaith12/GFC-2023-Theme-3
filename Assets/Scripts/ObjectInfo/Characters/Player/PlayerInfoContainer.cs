using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKGG.ObjectInfo
{
    public class PlayerInfoContainer : MonoBehaviour, ICharacterInfoContainer
    {
        public PlayerDescriptor descriptor;
        ICharacterDescriptor ICharacterInfoContainer.descriptor { get => descriptor; set => descriptor = (PlayerDescriptor)value; }

        [Header("Object References")]
        [SerializeField] SpriteRenderer head;
        [SerializeField] SpriteRenderer torso;
        [SerializeField] SpriteRenderer hand1;
        [SerializeField] SpriteRenderer hand2;
        [SerializeField] SpriteRenderer foot1;
        [SerializeField] SpriteRenderer foot2;

        void Start()
        {
            GetComponent<Animator>().runtimeAnimatorController = descriptor.costume.animator;
            head.sprite = descriptor.costume.head;
            torso.sprite = descriptor.costume.torso;
            hand1.sprite = descriptor.costume.hand1Closed;
            hand2.sprite = descriptor.costume.hand2Closed;
            foot1.sprite = descriptor.costume.foot1;
            foot2.sprite = descriptor.costume.foot2;
        }
    }
}