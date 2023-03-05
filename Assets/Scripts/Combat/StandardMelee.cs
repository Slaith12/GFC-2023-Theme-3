using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardMelee : MonoBehaviour
{
    [Tooltip("Use a composite collider on the hitbox if it needs multiple parts. (Unity may add a rigidbody to the hitbox. Set it to kinematic if it does)\n" +
        "If you want multiple hitboxes with different properties, you'll need a different script.")]
    [SerializeField] Hitbox hitbox;
    [Tooltip("The speed the weapon needs to be travelling to register as a hit.")]
    [SerializeField] float minimumSpeed = 3;
    [SerializeField] int damage; //yes health values will be integers
    [SerializeField] float knockbackStrength;

    private new Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        hitbox.OnHit += OnHit;
    }

    private void OnHit(Hitbox hitbox, Collider2D collision)
    {
        if (rigidbody.velocity.magnitude < minimumSpeed)
            return;
        if(collision.tag == "Enemy")
        {
            Vector2 hitOffset = collision.transform.position - hitbox.transform.position;
            Vector2 knockbackVector;
            if (hitOffset.x > 0)
                knockbackVector = Vector2.right * knockbackStrength;
            else
                knockbackVector = Vector2.left * knockbackStrength;
            collision.GetComponent<EnemyController>().Damage(damage, knockbackVector);
        }
    }
}
