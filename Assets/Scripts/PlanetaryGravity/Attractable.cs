using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractable : MonoBehaviour
{
    [SerializeField] private bool rotateToCenter = true;
    [SerializeField] Attractor currentAttractor;

    Transform m_transform;
    Collider2D m_collider;
    Rigidbody2D m_rigdibody;

    public float angle;

    private void Awake()
    {
        m_transform = GetComponent<Transform>();
        m_collider = GetComponent<Collider2D>();
        m_rigdibody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (currentAttractor != null)
        {
            if (!currentAttractor.AttractedObjects.Contains(m_collider)) currentAttractor = null;
            
            if (rotateToCenter) RotateToCenter();
        }
    }

    public void Attract(Attractor artgra)
    {
        rotateToCenter = currentAttractor;
        Vector2 attractionDir = (Vector2)artgra.planetTransform.position - m_rigdibody.position;
        m_rigdibody.AddForce(attractionDir.normalized * -artgra.gravity * 100 * Time.fixedDeltaTime);

        if (currentAttractor == null)
        {
            currentAttractor = artgra;
            
        }

    }

    void RotateToCenter()
    {
        Vector2 distanceVector = (Vector2)currentAttractor.planetTransform.position - (Vector2)m_transform.position;
        angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        m_transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
    }
}
