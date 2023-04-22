using SKGG.Netcode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKGG.Physics
{
    //-TODO: Review grab/throw physics to see if they can be made more fun (idea: make acceleration faster when item is at lower speed to make controls snappier) [low priority, likely not needed]
    //-TODO: Move all object handling code to the grabber script to reduce complexity and allow future implementation of different grabber behaviors [low priority, likely won't have other grabbers, and grabbing is already complete]
    //TODO: Change Grab and Release methods to work with networking (make sure all clients know when an object is grabbed/released, preferably also knowing who did it)
    [RequireComponent(typeof(Rigidbody2D)/*, typeof(PositionSync), typeof(RotationSync)*/)]
    public class GrabbableObject : MonoBehaviour
    {
        enum FlipBehavior { FlipX, FlipY, NoFlip }

        private Transform firstHandPlacement; //instantiated by this script, moves to where player grabs object.
        [Tooltip("Where the player would place their second hand on the object. Leave blank if it's a 1 handed object.")]
        [SerializeField] Transform secondHandPlacement;
        [Range(-180, 180)]
        [SerializeField] float targetRotation;
        [SerializeField] float rotationOffsetFactor;
        [SerializeField] float airResistance = 1;
        [SerializeField] FlipBehavior flipBehavior;

        public Vector2 firstHandPosition { get => firstHandPlacement.position; private set => firstHandPlacement.position = value; }
        public Vector2 secondHandPosition
        {
            get
            {
                if (isTwoHanded)
                    return secondHandPlacement.position;
                Debug.LogError("Tried getting second hand position of 1-handed object. Be sure to check the isTwoHanded property before checking secondHandPosition");
                return Vector2.zero;
            }
        }
        public bool isTwoHanded { get => secondHandPlacement != null; }
        public IGrabber currentHolder { get; private set; } //null if not being held
        public bool isCurrentlyHeld => currentHolder != null;

        public bool facingRight { get; private set; }

        private new Rigidbody2D rigidbody;
        //private PositionSync positionSync;
        //private RotationSync rotationSync;

        //script will need to do some COM trickery to stablize the object when held so the following fields are helpful in making sure the object still behaves properly
        private Vector2 standardCOM; //the proper COM of the object
        private float standardInertia; //the inertia of the object when rotated around its proper COM

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            //positionSync = GetComponent<PositionSync>();
            //rotationSync = GetComponent<RotationSync>();

            rigidbody.gravityScale = 0; //gravity will be simulated in script;
            firstHandPlacement = new GameObject("Hand 1 Placement").transform;
            firstHandPlacement.parent = transform;
            standardCOM = rigidbody.centerOfMass;
            standardInertia = rigidbody.inertia;
        }

        private void FixedUpdate()
        {
            SimulateGravity();
            SimulateAirResistance();
            if (currentHolder == null)
                return;
            MoveTowardsCursor();
            RotateTowardsTarget();
        }

        private void SimulateGravity()
        {
            ApplyForceAtCOM(Physics2D.gravity * rigidbody.mass);
        }

        private void SimulateAirResistance()
        {
            ApplyForceAtCOM(-rigidbody.velocity * airResistance);
        }

        private void MoveTowardsCursor()
        {
            Vector2 interpolatedPos = firstHandPosition + rigidbody.velocity * currentHolder.lookAheadTime;
            Vector2 movementVector = currentHolder.targetLocation - interpolatedPos;
            rigidbody.AddForce(movementVector * currentHolder.followStrength);
        }

        private void RotateTowardsTarget()
        {
            float interpolatedRotation = ConstrainAngle(transform.eulerAngles.z) + rigidbody.angularVelocity * currentHolder.lookAheadTime;
            float currentTarget = ConstrainAngle(targetRotation + currentHolder.rotationOffset * rotationOffsetFactor);
            if (interpolatedRotation - currentTarget > 180) //this usually happens when the rotations end up on different ends of the constraints (-180 and +180)
            {
                currentTarget += 360;
            }
            else if (currentTarget - interpolatedRotation > 180)
            {
                currentTarget -= 360;
            }
            float rotationTorque = currentTarget - interpolatedRotation * currentHolder.torqueStrength;
            rigidbody.AddTorque(rotationTorque * rigidbody.inertia);
        }

        private void ApplyForceAtCOM(Vector2 force)
        {
            rigidbody.AddForceAtPosition(force, transform.localToWorldMatrix.MultiplyPoint(standardCOM));
        }

        private float ConstrainAngle(float angle)
        {
            while (angle > 180)
                angle -= 360;
            while (angle < -180)
                angle += 360;
            return angle;
        }

        public bool Grab(IGrabber grabber, Vector2 grabPosition, bool forceGrab = false)
        {
            if (isCurrentlyHeld && !forceGrab)
                return false;
            currentHolder = grabber;
            firstHandPosition = grabPosition;
            AdjustRotationCenter();
            //temporarily ignore sync updates so that server can get caught up
            //positionSync.IgnoreUpdates(1);
            //rotationSync.IgnoreUpdates(1);
            return true;
        }

        public void Release(bool forceResync = false)
        {
            currentHolder = null;
            AdjustRotationCenter();
            if (forceResync)
            {
                ForceResync();
            }
            //temporarily ignore sync updates so that server can get caught up
            //positionSync.IgnoreUpdates(1);
            //rotationSync.IgnoreUpdates(1);
        }

        public void ForceResync()
        {
            //Debug.Log($"Object {gameObject.name} forcibly resynced by outside code.");
            //positionSync.ForceResync();
            //rotationSync.ForceResync();
        }

        public void Flip()
        {
            facingRight = !facingRight;
            switch (flipBehavior)
            {
                case FlipBehavior.FlipX:
                    {
                        Vector3 scale = transform.localScale;
                        scale.x *= -1;
                        transform.localScale = scale;
                        transform.Rotate(0, 0, 180);
                        break;
                    }
                case FlipBehavior.FlipY:
                    {
                        Vector3 scale = transform.localScale;
                        scale.y *= -1;
                        transform.localScale = scale;
                        transform.Rotate(0, 0, 180);
                        break;
                    }
                case FlipBehavior.NoFlip:
                    {
                        break;
                    }
            }
        }

        private void AdjustRotationCenter()
        {
            if (currentHolder == null)
            {
                rigidbody.centerOfMass = standardCOM;
                rigidbody.inertia = standardInertia;
            }
            else
            {
                //object will rotate around the first hand, and unity always rotates objects around the center of mass
                Vector2 newCenterOfRotation = firstHandPlacement.localPosition;
                rigidbody.centerOfMass = newCenterOfRotation;
                float distanceSquared = (newCenterOfRotation - standardCOM).sqrMagnitude;
                rigidbody.inertia = standardInertia + rigidbody.mass * distanceSquared; //parallel axis theorem wow
            }
        }

        private void OnValidate()
        {
            ClientNetworkTransform networkTransform = GetComponent<ClientNetworkTransform>();
            if (networkTransform == null)
                return;
            networkTransform.SyncScaleX = (flipBehavior == FlipBehavior.FlipX);
            networkTransform.SyncScaleY = (flipBehavior == FlipBehavior.FlipY);
        }
    }
}
