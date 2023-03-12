using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKGG.ObjectInfo
{
    /// <summary>
    /// This interface allows the components shared by npcs and players to be able access their attributes without worrying
    /// about what type of character they're on
    /// </summary>
    public interface ICharacterDescriptor
    {
        public CharacterAttributes attributes { get; }
        public CharacterCostume costume { get; }
    }
}