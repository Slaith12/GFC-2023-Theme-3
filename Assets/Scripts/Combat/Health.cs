using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Port EnemyController's damage handling to this script to allow it to be reused for the player
public class Health : MonoBehaviour
{
    [SerializeField] int health = 10;
    [HideInInspector] public bool dead;

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
        }
        if(mover != null)
            mover.Knockback(knockback);
    }
}
