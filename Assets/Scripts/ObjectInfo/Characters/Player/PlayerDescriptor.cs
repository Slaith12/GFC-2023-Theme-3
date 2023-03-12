using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKGG.ObjectInfo
{
    [CreateAssetMenu(menuName = "ObjectInfo/Descriptor/Player")]
    public class PlayerDescriptor : ScriptableObject, ICharacterDescriptor
    {
        [SerializeField] PlayerAttributes m_attributes;
        public PlayerAttributes attributes => m_attributes;

        [SerializeField] PlayerCostume m_costume;
        public PlayerCostume costume { get => m_costume; }

        CharacterAttributes ICharacterDescriptor.attributes { get => m_attributes; }
        CharacterCostume ICharacterDescriptor.costume { get => m_costume; }
    }
}