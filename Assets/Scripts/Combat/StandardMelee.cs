using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardMelee : MonoBehaviour
{
    [Tooltip("Use a composite collider on the hitbox if it needs multiple parts. (Unity may add a rigidbody to the hitbox. Set it to kinematic if it does)\n" +
        "If you want multiple hitboxes with different properties, you'll need a different script.")]
    [SerializeField] Hitbox hitbox;
    [SerializeField] int damage; //yes health values will be ints
    [SerializeField] float knockback;

    private void Awake()
    {
        hitbox.OnHit += OnHit;
    }

    private void OnHit(Hitbox hitbox, Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            Debug.Log("Hit!");
        }
    }
}
