using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    public float gravity = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Attractable attractable = collision.GetComponent<Attractable>();
        if (attractable != null)
            attractable.Attract(this);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Attractable attractable = collision.GetComponent<Attractable>();
        if (attractable != null)
            attractable.Unattract(this);
    }
}
