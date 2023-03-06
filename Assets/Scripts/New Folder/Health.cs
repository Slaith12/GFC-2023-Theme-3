using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Port EnemyController's damage handling to this script to allow it to be reused for the player
public class Health : MonoBehaviour
{
    [SerializeField] float health;

    public void Damage(int damage)
    {
        health -= damage;
    }
}
