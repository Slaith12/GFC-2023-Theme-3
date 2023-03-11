using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKGG.ObjectInfo
{
    public class PlayerInfo : MonoBehaviour, ICharacterInfo
    {
        [SerializeField] PlayerAttributes m_attributes;
        public PlayerAttributes attributes => m_attributes;

        [SerializeField] PlayerCostume m_costume;
        public CharacterCostume costume { get => m_costume; set => m_costume = (PlayerCostume)value; }

        CharacterAttributes ICharacterInfo.attributes { get => m_attributes; set => m_attributes = (PlayerAttributes)value; }

        [Header("Object References")]
        [SerializeField] SpriteRenderer head;
        [SerializeField] SpriteRenderer torso;
        [SerializeField] SpriteRenderer hand1;
        [SerializeField] SpriteRenderer hand2;
        [SerializeField] SpriteRenderer foot1;
        [SerializeField] SpriteRenderer foot2;

        void Start()
        {
            GetComponent<Animator>().runtimeAnimatorController = m_costume.animator;
            head.sprite = m_costume.head;
            torso.sprite = m_costume.torso;
            hand1.sprite = m_costume.hand1Closed;
            hand2.sprite = m_costume.hand2Closed;
            foot1.sprite = m_costume.foot1;
            foot2.sprite = m_costume.foot2;
        }
    }
}