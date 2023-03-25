using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace SKGG.Netcode
{
    public class RotationSync : NetworkBehaviour
    {
        float ping => (float)(NetworkManager.LocalTime.Time - NetworkManager.ServerTime.Time);

        //remember that this values will be on a delay, so set the tolerances according to that.
        //movement is going to be client-authoritative because it's way easier to do it like that, and I don't think we'll
        //need to worry about cheating yet
        //this is a band-aid fix to stop items from rubber-banding on their own screen. It's caused by current method of
        //sending inputs over the network not working perfectly.
        //This doesn't actually stop rubber-banding, it just makes it only seen on characters/items that aren't controlled by the player
        private NetworkVariable<float> trackedRotation = new NetworkVariable<float>(writePerm: NetworkVariableWritePermission.Owner);

        [Tooltip("The max deviation from the tracked rotation allowed before the object is forcibly resynced.\n" +
            "This value scales with velocity and ping, so it's safe to set it somewhat low.")]
        [SerializeField] float rotationTolerance = 0.5f;

        //used for position checks
        LinkedList<float> recentVelocities;
        float maxRecentVelocity;

        bool fixedUpdateExecuted;

        private new Rigidbody2D rigidbody;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            recentVelocities = new LinkedList<float>();
            maxRecentVelocity = 0;
            fixedUpdateExecuted = false;
        }

        private void FixedUpdate()
        {
            fixedUpdateExecuted = true;
            if (!IsOwner)
            {
                float currentVelocity = Mathf.Abs(rigidbody.angularVelocity);
                if (currentVelocity > maxRecentVelocity)
                {
                    maxRecentVelocity = currentVelocity;
                }
                recentVelocities.AddLast(currentVelocity);
            }
        }

        void Update()
        {
            if (!fixedUpdateExecuted)
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
            trackedRotation.Value = transform.eulerAngles.z;
        }

        private void ClientSync()
        {
            //the recent velocities list is trimmed in update rather than fixed update as an optimization, so that it's done a maximum of 1 time per frame
            //this isn't called when no fixed updates happened before this update so it doesn't affect higher frame rates
            TrimRecentVelocities();
            if (Mathf.Abs(transform.eulerAngles.z - trackedRotation.Value) > rotationTolerance + ping * maxRecentVelocity * 1.1f)
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
            if (maxRemovedVelocity == maxRecentVelocity)
            {
                maxRecentVelocity = 0;
                foreach (float speed in recentVelocities)
                {
                    maxRecentVelocity = Mathf.Max(maxRecentVelocity, speed);
                }
            }
        }

        public void ForceResync()
        {
            Debug.Log($"Force syncing object {gameObject.name}");
            transform.eulerAngles = new Vector3(0, 0, trackedRotation.Value);
            recentVelocities.Clear();
            recentVelocities.AddFirst(Mathf.Abs(rigidbody.angularVelocity));
            maxRecentVelocity = recentVelocities.First.Value;
        }
    }
}