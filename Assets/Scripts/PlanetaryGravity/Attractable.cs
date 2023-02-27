using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractable : MonoBehaviour
{
    private bool rotateToCenter = true;
    private Attractor currentAttractor;

    private Collider2D m_collider;
    private Rigidbody2D m_rigidbody;

    [HideInInspector] public float angle;

    private void Awake()
    {
        m_collider = GetComponent<Collider2D>();
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (currentAttractor != null)
        {
            if (!currentAttractor.AttractedObjects.Contains(m_collider)) currentAttractor = null;
            
            if (rotateToCenter) RotateToCenter();
        }
    }

    public void Attract(Attractor attractor)
    {
        rotateToCenter = currentAttractor; //?????? I know what this line means but why is it here?
        Vector2 attractionDir = (Vector2)attractor.planetTransform.position - m_rigidbody.position;
        m_rigidbody.AddForce(attractionDir.normalized * -attractor.gravity * 100 * Time.fixedDeltaTime);

        if (currentAttractor == null)
        {
            currentAttractor = attractor;
            
        }

    }

    void RotateToCenter()
    {
        Vector2 distanceVector = (Vector2)currentAttractor.planetTransform.position - (Vector2)transform.position;
        angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
    }
}
