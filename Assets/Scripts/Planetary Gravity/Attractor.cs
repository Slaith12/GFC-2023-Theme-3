using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    public float gravityScale = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Attracting {collision.name}");
        Attractable attractable = collision.GetComponent<Attractable>();
        if (attractable != null)
            attractable.Attract(this);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log($"Unattracting {collision.name}");
        Attractable attractable = collision.GetComponent<Attractable>();
        if (attractable != null)
            attractable.Unattract(this);
    }
}
