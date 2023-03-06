using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update

    public static float healthTotal;

    public float playerHealth;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer d = collision.gameObject.GetComponent<DamageDealer>();
        if (d != null) 
        {
            playerHealth -= d.damage;
        }

        //for when we implement healing (can change to fit)
        /*HealthPotion p = collision.gameObject.GetComponent<HealthPotion>();
        if (p != null) 
        {
            playerHealth += p.healVal;
        }*/

        if (playerHealth < healthTotal) 
        { 
            playerHealth = healthTotal;
        }
    }
}
