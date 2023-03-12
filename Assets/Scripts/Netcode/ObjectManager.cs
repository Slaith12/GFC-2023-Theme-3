using SKGG.ObjectInfo;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace SKGG.Netcode
{
    public class ObjectManager : NetworkBehaviour
    {
        public static ObjectManager Singleton { get; private set; }

        //this override is needed because the host being both a client and the server causes more problems than it should
        private new bool IsServer => NetworkManager.Singleton.IsServer;

        [Header("Base Prefabs")]
        [SerializeField] GameObject playerPrefab;
        [SerializeField] GameObject npcPrefab;
        [SerializeField] GameObject itemPrefab;
        [SerializeField] GameObject objectPrefab;

        [Header("Object Info")]
        [SerializeField] PlayerDescriptor[] playerInfos;
        private IDictionary<PlayerDescriptor, int> playerCostumeIDs;

        private void Awake()
        {
            Singleton = this;
            playerCostumeIDs = new Dictionary<PlayerDescriptor, int>();
            for(int i = 0; i < playerInfos.Length; i++)
            {
                playerCostumeIDs[playerInfos[i]] = i;
            }
        }

        public PlayerInfoContainer SpawnPlayer(PlayerDescriptor playerInfo, Vector2 position, float rotation)
        {
            return SpawnPlayer(playerCostumeIDs[playerInfo], position, rotation);
        }

        public PlayerInfoContainer SpawnPlayer(int costumeID, Vector2 position, float rotation)
        {
            if (!IsServer)
            {
                Debug.LogError("Attempted to spawn a player from a client. Only the server has the authority to spawn a player.\n" +
                    "If you want to spawn an NPC astronaut, you'll need to create one separately and use SpawnNPC");
                return null;
            }
            SpawnPlayerClientRPC((byte)costumeID, position, rotation);
            return SpawnPlayerInternal(playerInfos[costumeID], position, rotation);
        }

        [ClientRpc]
        private void SpawnPlayerClientRPC(byte costumeID, Vector2 position, float rotation)
        {
            //SpawnPlayerInternal(playerInfos[costumeID], position, rotation);
        }

        private PlayerInfoContainer SpawnPlayerInternal(PlayerDescriptor descriptor, Vector2 position, float rotation)
        {
            PlayerInfoContainer newPlayer = Instantiate(playerPrefab).GetComponent<PlayerInfoContainer>();
            newPlayer.descriptor = descriptor;
            newPlayer.transform.position = position;
            newPlayer.transform.eulerAngles = new Vector3(0, 0, rotation);
            return newPlayer;
        }

        public void SpawnNPC(NPCDescriptor npcInfo)
        {

        }

        public void SpawnNPC(int npcID)
        {

        }

        private void SpawnItem(int itemID)
        {

        }

        //I don't think this function will be used since I can't think of a time when a client needs to tell the server to spawn something
        [ServerRpc]
        private void SpawnItemServerRPC(byte itemID)
        {

        }

        [ClientRpc]
        private void SpawnItemClientRPC(byte itemID)
        {

        }
    }
}