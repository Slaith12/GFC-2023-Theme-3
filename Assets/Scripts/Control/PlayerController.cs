using SKGG.Attributes;
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
        private struct InputData : INetworkSerializable
        {
            public float walkInput;
            public Vector2 cursorOffset;
            public bool jumpInput;
            public bool grabInput;
            public bool releaseInput;

            public InputData(float walkInput, Vector2 cursorOffset, bool jumpInput, bool grabInput, bool releaseInput)
            {
                this.walkInput = walkInput;
                this.cursorOffset = cursorOffset;
                this.jumpInput = jumpInput;
                this.grabInput = grabInput;
                this.releaseInput = releaseInput;
            }

            public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
            {
                serializer.SerializeValue(ref walkInput);
                serializer.SerializeValue(ref cursorOffset);
                serializer.SerializeValue(ref jumpInput);
                serializer.SerializeValue(ref grabInput);
                serializer.SerializeValue(ref releaseInput);
            }
        }

        private AttributeContainer attributeContainer;
        private PlayerAttributes attributes { get => (PlayerAttributes)attributeContainer.attributes; }

        private bool jumpInput;
        private bool grabInput;
        private bool releaseInput;

        private PlayerInputs inputs;

        private Mover mover;
        private PlayerGrab playerGrab;

        void Awake()
        {
            attributeContainer = GetComponent<AttributeContainer>();

            inputs = new PlayerInputs(transform);
            inputs.Grab += GrabInput;
            inputs.Release += ReleaseInput;
            inputs.cursorRange = attributes.grabRange;

            mover = GetComponent<Mover>();
            playerGrab = GetComponent<PlayerGrab>();
        }

        private void Start()
        {
            //NetworkManager.StartHost();
        }

        public override void OnNetworkSpawn()
        {
            if (!IsOwner)
            {
                //GetComponent<NetworkObject>().ChangeOwnership(NetworkManager.LocalClientId);
            }
            if (IsOwner)
            {
                inputs.Enable(); //inputs are disabled by default
            }
            base.OnNetworkSpawn();
        }

        public override void OnDestroy()
        {
            inputs.Disable();
            base.OnDestroy();
        }

        private void FixedUpdate()
        {
            if(IsOwner)
            {
                InputData inputData = new InputData(inputs.walkInput, inputs.cursorOffset, jumpInput, grabInput, releaseInput);
                SendInputDataServerRpc(inputData);
                jumpInput = false;
                grabInput = false;
                releaseInput = false;
            }
        }

        [ServerRpc]
        private void SendInputDataServerRpc(InputData inputData)
        {
            SendsInputDataClientRpc(inputData);
            ProcessInputs(inputData);
        }

        [ClientRpc]
        private void SendsInputDataClientRpc(InputData inputData)
        {
            if (IsServer) //the host would've already processed the inputs
                return;
            ProcessInputs(inputData);
        }

        private void ProcessInputs(InputData inputData)
        {
            playerGrab.targetLocation = inputData.cursorOffset + (Vector2)transform.position;

            //y value doesn't matter since player can't fly, so mover doesn't change y
            mover.targetVelocity = new Vector2(inputData.walkInput * attributes.moveSpeed, 0);
            if (inputData.jumpInput)
            {
                mover.Jump(0); //replace with actual variable
            }
            if (inputData.grabInput)
            {
                Grab();
            }
            if (inputData.releaseInput)
            {
                Release();
            }
        }

        //no button for jumping right now, can easily add one (will need to modify PlayerInputs script, same code as Item Action)
        private void JumpInput()
        {
            jumpInput = true;
        }

        private void GrabInput()
        {
            grabInput = true;
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

        private void ReleaseInput()
        {
            releaseInput = true;
        }

        private void Release()
        {
            playerGrab.ReleaseCurrentObject();
        }
    }
}
