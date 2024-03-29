using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKGG.Combat
{
    //Since different weapons/enemies can have different on hit effects, all hit handling will be done in the weapon script rather than this script.
    public delegate void OnHitHandler(Hitbox hitbox, Collider2D collision);

    public class Hitbox : MonoBehaviour
    {
        public event OnHitHandler OnHit;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (OnHit == null) //events are considered "null" if no functions are hooked up onto them, i.e. nothing happens if they're invoked.
            {
                //get the parent of the whole object (the debug message is more useful when which object is causing it. without this it would often say "Hitboxes".
                Transform baseObject = transform;
                while (baseObject.parent != null)
                    baseObject = baseObject.parent;
                Debug.LogWarning($"Hitbox on {baseObject.name} does not do anything! Make sure there's a weapon script that does something with this hitbox.");
            }
            else
            {
                OnHit.Invoke(this, collision);
            }
        }
    }
}
