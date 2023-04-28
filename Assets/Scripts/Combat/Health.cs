using SKGG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKGG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] int health = 10;
        [HideInInspector] public bool dead;

        public event Action<int, Vector2> OnHurt;
        public event Action OnDeath;

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
            OnHurt?.Invoke(damage, knockback);

            if (health <= 0)
            {
                dead = true;
                OnDeath?.Invoke();
            }
            if (mover != null)
                mover.Knockback(knockback);
        }
    }
}
