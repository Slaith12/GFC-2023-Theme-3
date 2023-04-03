using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace SKGG.Netcode
{
    public class ServerManager : MonoBehaviour
    {
        private NetworkManager networkManager => NetworkManager.Singleton;

        [SerializeField] GameObject[] playerPrefabs;
        private ulong[] costumeOwners;

        //don't do things with the network manager in awake
        private void Start()
        {
            costumeOwners = new ulong[4];
            networkManager.OnClientConnectedCallback += SpawnPlayer;
            networkManager.NetworkConfig.PlayerPrefab = null; //the player prefab should be spawned by this script, not by NetworkManager
            networkManager.OnClientDisconnectCallback += DespawnPlayer;
        }

        private void SpawnPlayer(ulong clientId)
        {
            //only the server has the authority to spawn a player
            if (!networkManager.IsServer)
                return;
            //Debug.Log($"Connecting client {clientId}");
            ReevaluatePlayers();
            int playerCostumeID = GetNewPlayerCostume();
            if(playerCostumeID == -1) //no costumes are available
            {
                playerCostumeID = 4;
            }
            costumeOwners[playerCostumeID] = clientId+1; //the +1 is so that 0 will 100% mean "unused"
            NetworkObject newPlayerObject = Instantiate(playerPrefabs[playerCostumeID]).GetComponent<NetworkObject>();
            newPlayerObject.SpawnAsPlayerObject(clientId);
        }

        private void DespawnPlayer(ulong clientId)
        {
            //only the server has the authority to despawn a player
            if (!networkManager.IsServer)
                return;
            for (int i = 0; i < costumeOwners.Length; i++)
            {
                if(costumeOwners[i] == clientId+1)
                {
                    costumeOwners[i] = 0;
                }
            }
        }

        private int GetNewPlayerCostume()
        {
            for (int i = 0; i < costumeOwners.Length; i++)
            {
                if (costumeOwners[i] == 0)
                {
                    return i;
                }
            }
            return -1;
        }

        private void ReevaluatePlayers()
        {
            for (int i = 0; i < costumeOwners.Length; i++)
            {
                if(costumeOwners[i] == 0)
                {
                    continue;
                }
                ulong owner = costumeOwners[i] - 1;
                if (!networkManager.ConnectedClients.ContainsKey(owner) || networkManager.ConnectedClients[owner] == null)
                {
                    costumeOwners[i] = 0;
                }
            }
        }
    }
}