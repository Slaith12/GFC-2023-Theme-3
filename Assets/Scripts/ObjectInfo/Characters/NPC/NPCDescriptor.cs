using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKGG.ObjectInfo
{
    public class NPCDescriptor : ScriptableObject, ICharacterDescriptor
    {
        [SerializeField] CharacterAttributes m_attributes;
        public CharacterAttributes attributes { get => m_attributes; }
        [SerializeField] CharacterCostume m_costume;
        public CharacterCostume costume { get => m_costume; }
    }
}