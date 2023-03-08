using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKGG.Attributes
{
    public abstract class AttributeList : ScriptableObject
    {
        //DO NOT put any values in any attribute list that would change during gameplay (i.e. current health, targetted player)
        //Also do not put any values that will be specific to each instance (i.e. component/gameobject references) 
        //Only put values that will remain constant across all instances (i.e. max health, targetting method)
    }
}