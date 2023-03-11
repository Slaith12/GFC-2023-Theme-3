using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKGG.ObjectInfo
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// The attributes and costume variables usually need to be set by the code that spawns the object, so don't do anything in
    /// the Awake function that depends on them.
    /// </remarks>
    public interface ICharacterInfo
    {
        public CharacterAttributes attributes { get; set; }
        public CharacterCostume costume { get; set; }
    }
}