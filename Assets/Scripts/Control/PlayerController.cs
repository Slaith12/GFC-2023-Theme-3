using SKGG.Combat;
using SKGG.Inputs;
using SKGG.Movement;
using SKGG.Physics;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace SKGG.Control
{
    [RequireComponent(typeof(Mover), typeof(PlayerGrab), typeof(Health))]
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField] float moveSpeed = 5;

        [SerializeField] float m_cursorRange = 10;
        public float cursorRange { get => cursorRange; set { m_cursorRange = value; inputs.cursorRange = value; } }

        private bool jumpInput;

        private PlayerInputs inputs;

        private Mover mover;
        private PlayerGrab playerGrab;

        void Awake()
        {
            inputs = new PlayerInputs(transform);
            inputs.Grab += Grab;
            inputs.Release += Release;
            inputs.cursorRange = m_cursorRange;
            inputs.Disable(); //don't enable controls until player is fully spawned

            mover = GetComponent<Mover>();
            playerGrab = GetComponent<PlayerGrab>();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (!IsOwner)
            {
                //only the owner (the controlling player) should have control over this astronaut
                //Destroy(this) would also work here but I don't want to go that far
                this.enabled = false;
            }
            else
            {
                inputs.Enable();
            }
        }

        public override void OnDestroy()
        {
            inputs.Disable();
            base.OnDestroy();
        }

        private void FixedUpdate()
        {
            playerGrab.targetLocation = inputs.cursorOffset + (Vector2)transform.position;

            //y value doesn't matter since player can't fly, so mover doesn't change y
            mover.targetVelocity = new Vector2(inputs.walkInput * moveSpeed, 0);

            if (jumpInput)
            {
                mover.Jump(0); //replace with actual variable
                jumpInput = false;
            }
        }

        //no button for jumping right now, can easily add one (will need to modify PlayerInputs script, same code as Item Action)
        private void Jump()
        {
            jumpInput = true;
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
}
