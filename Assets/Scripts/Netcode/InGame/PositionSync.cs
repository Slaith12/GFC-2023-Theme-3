using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace SKGG.Netcode
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PositionSync : NetworkBehaviour
    {
        float ping => (float)(NetworkManager.LocalTime.Time - NetworkManager.ServerTime.Time);
        Vector2 position { get => transform.position; set => transform.position = value; }
        Vector2 velocity { get => rigidbody.velocity; set => rigidbody.velocity = value; }

        //remember that this values will be on a delay, so set the tolerances according to that.
        //movement is going to be client-authoritative because it's way easier to do it like that, and I don't think we'll
        //need to worry about cheating yet
        private NetworkVariable<Vector2> trackedPosition = new NetworkVariable<Vector2>(writePerm: NetworkVariableWritePermission.Owner);
        private NetworkVariable<Vector2> trackedVelocity = new NetworkVariable<Vector2>(writePerm: NetworkVariableWritePermission.Owner);

        [Header("Host Update Settings")]
        [Min(0)]
        [Tooltip("The amount that the object needs to move before its position is updated for clients. Lower values have more accuracy at the cost of more network traffic.")]
        [SerializeField] float positionUpdateThreshold = 0.5f;
        [Min(0)]
        [Tooltip("The amount that the object's velocity needs to change before it's updated for clients. Lower values have more accuracy at the cost of more network traffic.")]
        [SerializeField] float velocityUpdateThreshold = 1;
        [Tooltip("Whether to have the object send an additional update when it stops moving.")]
        [SerializeField] bool updateOnSleep = true;
        private bool isSleeping;

        [Header("Client Sync Settings")]
        [Min(0)]
        [Tooltip("The max position deviation from the tracked position allowed before the object is forcibly resynced.\n" +
            "Setting this too low can cause rubberbanding. Set it high and let velocity nudge deal with smaller desyncs.")]
        [SerializeField] float minPositionTolerance = 5;
        [Min(0)]
        [Tooltip("The minimum amount that the object is \"nudged\" towards the tracked position. Set this low enough so that it can't overpower the actual velocity")]
        [SerializeField] float minVelocityNudge = 0.5f;
        [Range(0, 1)]
        [Tooltip("The amount that the object is \"nudged\" towards the tracked position based on how far away it is. The close to 1 it is, the more the nudge will oppose the velocity on the client's screen")]
        [SerializeField] float velocityNudgeScale = 0.1f;

        //used for position checks
        LinkedList<float> recentVelocities;
        float maxRecentVelocity;

        bool fixedUpdateExecuted;

        private new Rigidbody2D rigidbody;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            maxRecentVelocity = velocity.magnitude;
            recentVelocities = new LinkedList<float>();
            recentVelocities.AddFirst(maxRecentVelocity);
            fixedUpdateExecuted = false;
            if (IsOwner)
            {
                trackedPosition.Value = position;
                trackedVelocity.Value = velocity;
            }
        }

        private void FixedUpdate()
        {
            if (!IsSpawned)
            {
                return;
            }
            fixedUpdateExecuted = true;
            if (!IsOwner)
            {
                float currentVelocity = velocity.magnitude;
                if (currentVelocity > maxRecentVelocity)
                {
                    maxRecentVelocity = currentVelocity;
                }
                recentVelocities.AddLast(currentVelocity);

                Vector2 positionDeviation = position - trackedPosition.Value;
                float nudgeMagnitude = (minVelocityNudge + velocityNudgeScale * positionDeviation.magnitude) * Time.fixedDeltaTime;
                if(nudgeMagnitude > positionDeviation.magnitude)
                {
                    position = trackedPosition.Value;
                }
                else
                {
                    position += -positionDeviation.normalized * nudgeMagnitude;
                }
            }
        }

        void Update()
        {
            if(!fixedUpdateExecuted || !IsSpawned)
            {
                return;
            }
            fixedUpdateExecuted = false;
            if (!NetworkManager.IsListening)
                return;
            //syncs are done in update rather than fixed update so that multiple updates aren't made on the same frame by the same object
            if (IsOwner)
            {
                ServerSync();
            }
            else
            {
                ClientSync();
            }
        }

        public override void OnGainedOwnership()
        {
            base.OnGainedOwnership();
            if (recentVelocities != null)
            {
                recentVelocities.Clear();
                maxRecentVelocity = 0;
            }
        }

        private void ServerSync()
        {
            if(!isSleeping && rigidbody.IsSleeping())
            {
                isSleeping = true;
                if(updateOnSleep)
                {
                    trackedPosition.Value = position;
                    trackedVelocity.Value = velocity;
                }
                return;
            }
            if ((trackedPosition.Value - position).magnitude > positionUpdateThreshold)
            {
                trackedPosition.Value = position;
                isSleeping = false;
            }
            if ((trackedVelocity.Value - velocity).magnitude > velocityUpdateThreshold)
            {
                trackedVelocity.Value = velocity;
                isSleeping = false;
            }
        }

        private void ClientSync()
        {
            //the recent velocities list is trimmed in update rather than fixed update as an optimization, so that it's done a maximum of 1 time per frame
            //this isn't called when no fixed updates happened before this update so it doesn't affect higher frame rates
            TrimRecentVelocities();
            if ((position - trackedPosition.Value).magnitude > minPositionTolerance + ping*maxRecentVelocity*1.1f)
            {
                ForceResync();
            }
        }

        private void TrimRecentVelocities()
        {
            int recentVelocitiesSize = Mathf.CeilToInt(ping * 1.1f / Time.fixedDeltaTime);
            float maxRemovedVelocity = 0;
            while (recentVelocities.Count > recentVelocitiesSize)
            {
                maxRemovedVelocity = Mathf.Max(maxRemovedVelocity, recentVelocities.First.Value);
                recentVelocities.RemoveFirst();
            }
            if(maxRemovedVelocity == maxRecentVelocity)
            {
                maxRecentVelocity = 0;
                foreach(float speed in recentVelocities)
                {
                    maxRecentVelocity = Mathf.Max(maxRecentVelocity, speed);
                }
            }
        }

        public void ForceResync()
        {
            Debug.Log($"Force syncing object {gameObject.name}");
            position = trackedPosition.Value;
            velocity = trackedVelocity.Value;
            recentVelocities.Clear();
            maxRecentVelocity = velocity.magnitude;
            recentVelocities.AddFirst(maxRecentVelocity);
        }
    }
}