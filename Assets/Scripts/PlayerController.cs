using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mover), typeof(PlayerGrab))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5;
    [SerializeField] float m_cursorRange = 10;
    public float cursorRange { get => cursorRange; set { m_cursorRange = value; inputs.cursorRange = value; } }

    private PlayerInputs inputs;

    private Mover mover;
    private PlayerGrab playerGrab;

    void Awake()
    {
        inputs = new PlayerInputs(transform);
        inputs.Grab += Grab;
        inputs.Release += Release;
        inputs.cursorRange = m_cursorRange;

        mover = GetComponent<Mover>();
        playerGrab = GetComponent<PlayerGrab>();
    }

    private void FixedUpdate()
    {
        playerGrab.targetLocation = inputs.cursorOffset + (Vector2)transform.position;

        mover.targetVelocity = new Vector2(inputs.walkInput*moveSpeed, 0);
    }

    private void Grab()
    {
        Vector2 cursorPos = (Vector2)transform.position + inputs.cursorOffset;
        Collider2D[] hits = Physics2D.OverlapPointAll(cursorPos);
        foreach (Collider2D hit in hits)
        {
            GrabbableObject grabbable = hit.GetComponentInParent<GrabbableObject>();
            if (grabbable != null)
            {
                playerGrab.GrabObject(grabbable, cursorPos);
                return;
            }
        }
    }

    private void Release()
    {
        playerGrab.ReleaseCurrentObject();
    }
}
