using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Change Grab and Release methods to work with networking (make sure all clients know when an object is grabbed/released, preferably also knowing who did it)
//TODO: Add input validation to Grab and Release methods to make sure the objects calling those methods are actually capable of grabbing/releasing object [this may be better done in the player controller]
[RequireComponent(typeof(Rigidbody2D))]
public class GrabbableObject : MonoBehaviour
{
    private Transform firstHandPlacement; //instantiated by this script, moves to where player grabs object.
    [Tooltip("Where the player would place their second hand on the object. Leave blank if it's a 1 handed object.")]
    [SerializeField] Transform secondHandPlacement;
    [SerializeField] float targetRotation;

    public Vector2 firstHandPosition { get => firstHandPlacement.position;  private set => firstHandPlacement.position = value; }
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

    private new Rigidbody2D rigidbody;
    //script will need to do some COM trickery to stablize the object when held so the following fields are helpful in making sure the object still behaves properly
    private Vector2 standardCOM; //the proper COM of the object
    private float standardInertia; //the inertia of the object when rotated around its proper COM

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.gravityScale = 0; //gravity will be simulated in script;
        firstHandPlacement = new GameObject("Hand 1 Placement").transform;
        firstHandPlacement.parent = transform;
        standardCOM = rigidbody.centerOfMass;
        standardInertia = rigidbody.inertia;
    }

    private void FixedUpdate()
    {
        SimulateGravity();
        if (currentHolder == null)
            return;
        Vector2 interpolatedPos = firstHandPosition + rigidbody.velocity * currentHolder.lookAheadTime;
        Vector2 movementVector = currentHolder.targetLocation - interpolatedPos;
        rigidbody.AddForceAtPosition(movementVector * currentHolder.followStrength, firstHandPosition);
        float interpolatedRotation = transform.eulerAngles.z + rigidbody.angularVelocity * currentHolder.lookAheadTime;
    }

    private void SimulateGravity()
    {
        rigidbody.AddForceAtPosition(Physics2D.gravity*rigidbody.mass, transform.localToWorldMatrix.MultiplyPoint(standardCOM)); //the matrix scares me and i'm not 100% sure it actually does what I think it's doing
    }

    private void ConstrainRotation()
    {
        float eulerAngle = transform.eulerAngles.z;
        while (eulerAngle > 180)
            eulerAngle -= 360;
        while (eulerAngle < -180)
            eulerAngle += 360;
    }

    public bool Grab(IGrabber grabber, Vector2 grabPosition)
    {
        if (currentHolder != null)
            return false;
        currentHolder = grabber;
        firstHandPosition = grabPosition;
        AdjustRotationCenter();
        return true;
    }

    public void Release()
    {
        currentHolder = null;
        AdjustRotationCenter();
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
}
