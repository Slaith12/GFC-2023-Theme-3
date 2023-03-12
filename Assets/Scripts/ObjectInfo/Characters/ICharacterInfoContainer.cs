using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKGG.ObjectInfo
{
    public interface ICharacterInfoContainer
    {
        public ICharacterDescriptor descriptor { get; set; }
    }
}