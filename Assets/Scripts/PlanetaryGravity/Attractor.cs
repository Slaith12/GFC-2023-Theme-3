using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    public LayerMask AttractionLayer;
    public float gravity = -10;
    [SerializeField] private float effectionRadius = 10;
    [HideInInspector] public List<Collider2D> AttractedObjects = new List<Collider2D>();

    void FixedUpdate()
    {
        SetAttractedObjects();
        AttractObjects();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, effectionRadius);
    }

    void SetAttractedObjects()
    {
        AttractedObjects.Clear();

        AttractedObjects.AddRange(Physics2D.OverlapCircleAll(transform.position, effectionRadius, AttractionLayer));
    }

    void AttractObjects()
    {
        for (int i = 0; i < AttractedObjects.Count; i++)
        {
            AttractedObjects[i].GetComponent<Attractable>().Attract(this);
        }
    }

}
