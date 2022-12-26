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

    public Vector2 firstHandPosition { get { return firstHandPlacement.position; } private set { firstHandPlacement.position = value; } }
    public Vector2 secondHandPosition { get { return secondHandPlacement.position; } }
    public IGrabber currentHolder { get; private set; } //null if not being held

    private new Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        firstHandPlacement = new GameObject("Hand 1 Placement").transform;
        firstHandPlacement.parent = transform;
    }

    private void FixedUpdate()
    {
        if (currentHolder == null)
            return;
        Vector2 interpolatedPos = firstHandPosition + rigidbody.velocity * currentHolder.lookAheadTime;
        Vector2 movementVector = currentHolder.targetLocation - interpolatedPos;
        rigidbody.AddForceAtPosition(movementVector * currentHolder.followStrength, firstHandPosition);
    }

    public bool Grab(IGrabber grabber, Vector2 grabPosition)
    {
        if (currentHolder != null)
            return false;
        currentHolder = grabber;
        firstHandPosition = grabPosition;
        return true;
    }

    public void Release()
    {
        currentHolder = null;
    }
}
