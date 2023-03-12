using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKGG.ObjectInfo
{
    public class NPCInfoContainer : MonoBehaviour, ICharacterInfoContainer
    {
        public NPCDescriptor descriptor;
        ICharacterDescriptor ICharacterInfoContainer.descriptor { get => descriptor; set => descriptor = (NPCDescriptor)value; }
    }
}