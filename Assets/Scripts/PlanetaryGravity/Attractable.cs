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
            if (rotateToCenter) RotateToCenter();

            if (!currentAttractor.AttractedObjects.Contains(m_collider)) currentAttractor = null;
        }
    }

    public void Attract(Attractor attractor)
    {
        rotateToCenter = currentAttractor; //?????? I know what this line means but why is it like this?
        Vector2 attractionDir = (Vector2)attractor.transform.position - m_rigidbody.position;
        m_rigidbody.AddForce(attractionDir.normalized * attractor.gravity * 100 * Time.fixedDeltaTime);

        if (currentAttractor == null)
        {
            currentAttractor = attractor;
            
        }

    }

    /// <summary>
    /// Take a vector offset in world coordinates and convert it to relative (rotated) coordinates
    /// </summary>
    public Vector2 WorldToRelativeOffset(Vector2 vector)
    {
        //remember: 0 degrees = left of planet ( 1,0 => 0,-1; 0,1 => 1,0)
        //90 degrees = bottom of planet ( 1,0 => -1, 0; 0,1 => 0,-1)
        float radians = angle * Mathf.Deg2Rad;
        float newX = -vector.x * Mathf.Sin(radians) + vector.y * Mathf.Cos(radians);
        float newY = -vector.x * Mathf.Cos(radians) - vector.y * Mathf.Sin(radians);
        return new Vector2(newX, newY);
    }

    /// <summary>
    /// Take a vector offset in relative rotated) coordinates and convert it to world coordinates
    /// </summary>
    public Vector2 RelativeToWorldOffset(Vector2 vector)
    {
        //remember: 0 degrees = left of planet ( 1,0 => 0,1; 0,1 => -1,0)
        //90 degrees = bottom of planet ( 1,0 <= -1, 0; 0,1 <= 0,-1)
        float radians = angle * Mathf.Deg2Rad;
        float newX = -vector.x * Mathf.Sin(radians) - vector.y * Mathf.Cos(radians);
        float newY = vector.x * Mathf.Cos(radians) - vector.y * Mathf.Sin(radians);
        return new Vector2(newX, newY);
    }

    void RotateToCenter()
    {
        Vector2 distanceVector = (Vector2)currentAttractor.transform.position - (Vector2)transform.position;
        angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
    }
}
