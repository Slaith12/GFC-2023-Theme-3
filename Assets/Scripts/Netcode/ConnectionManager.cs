using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace SKGG.Netcode
{
    public class ConnectionManager : MonoBehaviour
    {
        private NetworkManager networkManager => NetworkManager.Singleton;

        [SerializeField] GameObject[] playerPrefabs;
        private int numPlayers;

        //don't do things with the network manager in awake
        private void Start()
        {
            networkManager.OnClientConnectedCallback += SpawnPlayer;
            networkManager.NetworkConfig.PlayerPrefab = null; //the player prefab should be spawned by this script, not by NetworkManager
            numPlayers = 0;
        }

        private void SpawnPlayer(ulong clientId)
        {
            //only the server has the authority to spawn a player
            if (!networkManager.IsServer)
                return;
            //increment numPlayers AFTER setting playerCostumeID so that the id starts at 0
            int playerCostumeID = numPlayers;
            if (numPlayers > 4)
                playerCostumeID = 4; //grey
            NetworkObject newPlayerObject = Instantiate(playerPrefabs[playerCostumeID]).GetComponent<NetworkObject>();
            newPlayerObject.SpawnAsPlayerObject(clientId);
            numPlayers++;
        }
    }
}