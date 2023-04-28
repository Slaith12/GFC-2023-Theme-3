using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKGG.Physics
{
    public class Attractable : MonoBehaviour
    {
        [Tooltip("If this is set to true, make sure the object's rotation is frozen in the rigidbody or collision will not work. Do not ask why this is, I have no idea.")]
        [SerializeField] bool rotateToCenter = true;
        private Attractor currentAttractor;
        //this is only needed for attractors with overlapping fields, which shouldn't ever happen, but I'm including it just in case
        private List<Attractor> overlappedAttractors;

        private new Rigidbody2D rigidbody;

        [HideInInspector] public float angle;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            angle = 270; //upright
            overlappedAttractors = new List<Attractor>();
        }

        private void FixedUpdate()
        {
            if (currentAttractor != null)
            {
                if (rotateToCenter) RotateToCenter();
                Vector2 attractionDir = (Vector2)currentAttractor.transform.position - rigidbody.position;
                //cancel out normal gravity and replace it with planet gravity
                rigidbody.AddForce(Physics2D.gravity * -rigidbody.gravityScale);
                rigidbody.AddForce(Physics2D.gravity.magnitude * attractionDir.normalized * (rigidbody.gravityScale * currentAttractor.gravityScale));
            }
        }

        public void Attract(Attractor attractor)
        {
            overlappedAttractors.Add(attractor);
            if (currentAttractor == null)
                currentAttractor = attractor;
        }

        public void Unattract(Attractor attractor)
        {
            overlappedAttractors.Remove(attractor);
            if (currentAttractor == attractor)
            {
                currentAttractor = null;
                if (overlappedAttractors.Count >= 1)
                    currentAttractor = overlappedAttractors[0];
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
        /// Take a vector offset in relative (rotated) coordinates and convert it to world coordinates
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
}
