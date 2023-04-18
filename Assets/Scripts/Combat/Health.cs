using SKGG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKGG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] int health = 10;
        [HideInInspector] public bool dead;
        [SerializeField] GameObject resourceDrop;

        private Mover mover;

        private void Awake()
        {
            mover = GetComponent<Mover>();
        }

        public void Damage(int damage)
        {
            Damage(damage, Vector2.zero);
        }

        public void Damage(int damage, Vector2 knockback)
        {
            health -= damage;
            if (health <= 0)
            {
                dead = true;
                Instantiate(resourceDrop);
            }
            if (mover != null)
                mover.Knockback(knockback);
        }
    }
}
