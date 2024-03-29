using SKGG.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace SKGG.Physics
{
    //this file is only for the non-network code. If you're looking for the network code, look in the Scripts/Netcode/NetworkPlayerGrab file

    //TODO: Have player hands rotate based on the held item rather than the player's body.
    //TODO: --NOTE: Read the Netcode for Gameobjects "NetworkObject Parenting" article before doing this-- Completely detach hands from player while holding item (would fix visual bug when turning around with item, and allows for more control with code)
    [RequireComponent(typeof(AttributeContainer))]
    public partial class PlayerGrab : NetworkBehaviour, IGrabber
    {

        private AttributeContainer attributeContainer;
        private PlayerAttributes attributes => (PlayerAttributes)attributeContainer.attributes;

        public Vector2 targetLocation { get; set; }
        public float followStrength { get => attributes.pullStrength; }
        public float rotationOffset { get => GetRotationOffset(); }
        public float torqueStrength { get => attributes.torqueStrength; }
        public float lookAheadTime { get => attributes.lookAheadTime; }

        [Header("Hands/Indicators (Visual Only)")]
        [SerializeField] Transform firstHand; //i am considering making the hand objects un-parented in Awake to make sure they don't move weirdly with the player
        [SerializeField] Transform firstHandDefaultPos;
        [SerializeField] Transform secondHand;
        [SerializeField] Transform secondHandDefaultPos;
        [SerializeField] float handTravelTime;
        [SerializeField] Transform grabCursor;

        private GrabbableObject currentGrabbed;
        private bool grabbingObject;
        private float interpTime;
        private bool facingRight;

        private void Awake()
        {
            attributeContainer = GetComponent<AttributeContainer>();
        }
        
        void Update()
        {
            grabCursor.position = targetLocation;
            MoveHands();
            UpdateFacing();
        }

        public void GrabObject(GrabbableObject grabbable, Vector2 pos)
        {
            if (grabbingObject)
            {
                Debug.LogWarning("Attempted to grab object while already holding something. This is probably a bug.");
                return;
            }
            bool grabSuccessful = grabbable.Grab(this, pos);
            if (!grabSuccessful)
                return;
            //the script needs to tell the other clients that the object was grabbed. the grab position is stored in the grabbed object, so it doesn't need to be passed
            TriggerGrabEvent(grabbable);
            UpdateAfterGrab(grabbable);
        }

        //this is here so that this is available normally to the client sided grab as well as give the network enforced grab a way to bypass the checks and rpc call
        private void UpdateAfterGrab(GrabbableObject grabbable)
        {
            grabbingObject = true;
            currentGrabbed = grabbable;
            if (currentGrabbed.facingRight != facingRight)
                currentGrabbed.Flip();
            interpTime = handTravelTime;
        }

        public void ReleaseCurrentObject(bool forceResync = false)
        {
            if (!grabbingObject)
                return;
            if(!forceResync && IsOwner)
            {
                TriggerReleaseEvent();
            }
            //the Release function assumes that the caller is the person grabbing the object, so make sure it isn't called when it isn't held by this
            if ((object)currentGrabbed.currentHolder == this)
            {
                currentGrabbed.Release(forceResync);
            }
            grabbingObject = false;
            interpTime = handTravelTime;
            return;
        }

        private void MoveHands()
        {
            if (grabbingObject)
            {
                if (interpTime > 0f)
                {
                    interpTime -= Time.deltaTime;
                    firstHand.position = Vector2.Lerp(currentGrabbed.firstHandPosition, firstHandDefaultPos.position, interpTime / handTravelTime);
                    if (currentGrabbed.isTwoHanded)
                        secondHand.position = Vector2.Lerp(currentGrabbed.secondHandPosition, secondHandDefaultPos.position, interpTime / handTravelTime);
                }
                else
                {
                    firstHand.position = currentGrabbed.firstHandPosition;
                    if (currentGrabbed.isTwoHanded)
                        secondHand.position = currentGrabbed.secondHandPosition;
                }
            }
            else
            {
                if (interpTime > 0f)
                {
                    interpTime -= Time.deltaTime;
                    firstHand.position = Vector2.Lerp(firstHandDefaultPos.position, currentGrabbed.firstHandPosition, interpTime / handTravelTime);
                    if (currentGrabbed.isTwoHanded)
                    {
                        secondHand.position = Vector2.Lerp(secondHandDefaultPos.position, currentGrabbed.secondHandPosition, interpTime / handTravelTime);
                    }
                }
                else
                {
                    firstHand.position = firstHandDefaultPos.position;
                    secondHand.position = secondHandDefaultPos.position;
                }
            }
        }

        //this feels like it would be better in PlayerController but I'm too lazy to move it
        //i don't think we'll need to touch this once we're done with the new input system stuff
        private void UpdateFacing()
        {
            //note: all sprites face left by default
            if (targetLocation.x > transform.position.x)
            {
                if (facingRight)
                    return;
                facingRight = true;
                Flip();
            }
            else
            {
                if (!facingRight)
                    return;
                facingRight = false;
                Flip();
            }
        }

        private void Flip()
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
            if (grabbingObject)
            {
                currentGrabbed.Flip();
            }
        }

        private float GetRotationOffset()
        {
            Vector2 posOffset = targetLocation - (Vector2)firstHandDefaultPos.position;
            Vector2 horizontal = Vector2.right;
            if (posOffset.x < 0)
                horizontal = Vector2.left;
            return Vector2.SignedAngle(horizontal, posOffset);
        }
    }
}
