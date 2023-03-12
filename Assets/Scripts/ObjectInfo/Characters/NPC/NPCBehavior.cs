using SKGG.ObjectInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKGG.Control
{
    //use editor scripting for this
    public abstract class NPCBehavior
    {
        public abstract void Init(EnemyController controller);
        public abstract void OnUpdate(EnemyController controller);
    }
}