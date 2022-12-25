using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] float respawnX;
    [SerializeField] Vector2 spawnLocation;
    [SerializeField] Vector2 spawnVelocity;

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x <= respawnX)
        {
            transform.position = spawnLocation;
            GetComponent<Rigidbody2D>().velocity = spawnVelocity;
        }
    }
}
