using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : DamageDealer
{
    // Start is called before the first frame update
    [SerializeField] Collider2D col;
    [SerializeField] Rigidbody2D rb;

    [SerializeField] Player player;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.MovePosition(player.transform.position);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
