using SKGG.Attributes;
using SKGG.Netcode;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace SKGG.Physics
{
    //this file is only for the network code. If you're looking for the non-network code, look in the Scripts/Physics/Grabbing/PlayerGrab file
    public partial class PlayerGrab : NetworkBehaviour
    {
        private void TriggerGrabEvent(GrabbableObject grabbable)
        {
            if (!IsOwner)
                return;
            Vector2 worldGrabPos = grabbable.firstHandPosition; //the grabbable object sets this when it gets grabbed
            Vector2 localGrabPos = grabbable.transform.worldToLocalMatrix.MultiplyPoint(worldGrabPos);
            ulong objectID = grabbable.GetComponent<NetworkObject>().NetworkObjectId;
            if (IsServer)
            {
                GrabClientRpc(objectID, localGrabPos, RPCParamHelper.SendToAllButOneClient(OwnerClientId));
            }
            else
            {
                GrabServerRpc(objectID, localGrabPos);
            }
        }

        [ServerRpc]
        private void GrabServerRpc(ulong objectId, Vector2 localPos, ServerRpcParams rpcParams = default)
        {
            ulong grabbingClientID = rpcParams.Receive.SenderClientId;
            //PlayerGrab will be "owned" by the player it's attached to. If the player that tries to grab somehow isn't
            //the one who owns the PlayerGrab component, then it's likely invalid. This could happen if a client sends a
            //message to have someone else grab.
            if(grabbingClientID != OwnerClientId)
            {
                FailedGrabClientRpc(GrabFailReason.GrabNotPermitted, RPCParamHelper.SendToOneClient(grabbingClientID));
                return;
            }
            if (!NetworkManager.SpawnManager.SpawnedObjects.ContainsKey(objectId))
            {
                FailedGrabClientRpc(GrabFailReason.ObjectNotFound, RPCParamHelper.SendToOneClient(grabbingClientID));
                return;
            }
            NetworkObject grabbedObject = NetworkManager.SpawnManager.SpawnedObjects[objectId];
            GrabbableObject grabbed = grabbedObject.GetComponent<GrabbableObject>();
            if (grabbed == null)
            {
                FailedGrabClientRpc(GrabFailReason.ObjectUngrabbable, RPCParamHelper.SendToOneClient(grabbingClientID));
                return;
            }

            if(grabbed.isCurrentlyHeld)
            {
                FailedGrabClientRpc(GrabFailReason.ObjectAlreadyHeld, RPCParamHelper.SendToOneClient(grabbingClientID));
                return;
            }
            //remember that this is still the PlayerGrab component attached to the player calling this function, so the transform and attributes are still available
            Vector2 grabberPos = transform.position;
            Vector2 grabbedPos = grabbed.transform.localToWorldMatrix.MultiplyPoint(localPos);
            //give the player a bit of leeway with the grab range to account for delay
            if((grabbedPos - grabberPos).magnitude > attributes.grabRange*1.1f)
            {
                FailedGrabClientRpc(GrabFailReason.ObjectOutOfRange, RPCParamHelper.SendToOneClient(grabbingClientID));
                return;
            }
            grabbed.Grab(this, grabbedPos, forceGrab: true);
            //change the ownership to the person who grabbed it so that it doesn't teleport in their hands
            grabbed.GetComponent<NetworkObject>().ChangeOwnership(OwnerClientId);
            UpdateAfterGrab(grabbed);
            //the client that sent the message already has it grabbed on their side, don't need to confirm it
            //it could lead to errors if we send it to everyone without making sure that the original client has a special case for it
            GrabClientRpc(objectId, localPos, RPCParamHelper.SendToAllButOneClient(grabbingClientID));
        }

        enum GrabFailReason { Unknown, GrabNotPermitted, ObjectNotFound, ObjectUngrabbable, ObjectAlreadyHeld, ObjectOutOfRange }

        [ClientRpc]
        private void FailedGrabClientRpc(GrabFailReason reason, ClientRpcParams rpcParams)
        {
            //if for whatever reason the player isn't grabbing the object, ignore the message since the point of this method
            //is to have the player stop holding the object.
            if(!grabbingObject)
            {
                return;
            }
            switch(reason)
            {
                case GrabFailReason.GrabNotPermitted:
                    //this means that the client tried to tell the server to have someone else grab an object.
                    //clients should only control their own character, so this happens as either a bug or a cheat
                    Debug.LogError($"Attempted to grab as {gameObject.name}, which is not owned by this player.");
                    goto case GrabFailReason.ObjectAlreadyHeld;

                case GrabFailReason.ObjectNotFound:
                    //this is a fairly serious case since it means that the object wasn't actually spawned,
                    //this mean there's a desync between the client and the server.
                    //it's also possible that this is caused by the grabbed object despawning during the time the message was being sent.
                    //this would happen when crafting materials are either consumed for crafting or bundled into a sack
                    //in either case, the grabbed object should be destroyed since it shouldn't exist
                    if(currentGrabbed != null && currentGrabbed.gameObject != null)
                    {
                        Destroy(currentGrabbed.gameObject);
                    }
                    grabbingObject = false;
                    currentGrabbed = null;
                    break;

                case GrabFailReason.ObjectUngrabbable:
                    //i don't see why this would ever happen unless NGO screws up and the object IDs get desynced
                    //the only way this can realistically happen is if we destroy an object's GrabbableObject component
                    //i see no reason we'd ever do this and it would likely be a bug
                    //i don't know how this would be handled so it'll get the default treatment with a console warning
                    Debug.LogError("Attempted to grab ungrabbable object. " +
                        "The GrabbableObject component may have been deleted on the server side or added on the client side. " +
                        "This is likely a bug.");
                    goto case GrabFailReason.ObjectAlreadyHeld;

                case GrabFailReason.ObjectAlreadyHeld:
                    //this will be the most common case, so it'll be treated as the default case
                    //this will happen when multiple clients try to grab the same thing at the same time, and they'll all grab it on their own screen
                    //this is just basic rollback
                    ReleaseCurrentObject(true);
                    break;

                case GrabFailReason.ObjectOutOfRange:
                    //this could reasonably happen if the grabbable object is moving quickly, but it should be rare
                    //this is mostly an anti-cheating measure so it's fine to give it the default case
                    //if we want to do more anti-cheating actions in response to this, do it in the server code
                    goto case GrabFailReason.ObjectAlreadyHeld;

                default:
                    Debug.LogError("Grab failed for unrecognized reason. Fail code " + reason);
                    goto case GrabFailReason.ObjectAlreadyHeld;
            }
        }

        [ClientRpc]
        private void GrabClientRpc(ulong objectId, Vector2 localPos, ClientRpcParams rpcParams)
        {
            if (IsServer) //the host would've already processed this
                return;
            NetworkObject grabbedObject = NetworkManager.SpawnManager.SpawnedObjects[objectId];
            GrabbableObject grabbed = grabbedObject.GetComponent<GrabbableObject>();
            Vector2 grabbedPos = grabbed.transform.localToWorldMatrix.MultiplyPoint(localPos);
            grabbed.Grab(this, grabbedPos, forceGrab: true);
            UpdateAfterGrab(grabbed);

        }
    }
}