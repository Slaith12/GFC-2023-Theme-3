using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace SKGG.Netcode
{
    public static class RPCParamHelper
    {
        private static IReadOnlyList<ulong> clientIDs => NetworkManager.Singleton.ConnectedClientsIds;

        //used as optimization so that the code doesn't repeatedly instantiate new arrays for every rpc
        private static ulong[] singleInstanceArray;
        private static ulong[] excludeOneArray;
        private static ulong[] includeAllArray;

        static RPCParamHelper()
        {
            singleInstanceArray = new ulong[1];
            excludeOneArray = new ulong[3];
            includeAllArray = new ulong[4];
        }

        public static ClientRpcParams SendToOneClient(ulong clientID)
        {
            singleInstanceArray[0] = clientID;
            return new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = singleInstanceArray } };
        }

        public static ClientRpcParams SendToAllButOneClient(ulong excludedClientID)
        {
            if (excludeOneArray.Length != clientIDs.Count - 1)
            {
                //this shouldn't run too often so it shouldn't cause any performance problems
                excludeOneArray = new ulong[clientIDs.Count - 1];
            }
            bool foundExcludedClient = false;
            for(int i = 0; i < clientIDs.Count; i++)
            {
                if(clientIDs[i] == excludedClientID)
                {
                    foundExcludedClient = true;
                    continue;
                }
                int index = i;
                if (foundExcludedClient)
                    index--;
                excludeOneArray[index] = clientIDs[i];
            }
            return new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = excludeOneArray } };
        }

        public static ClientRpcParams SendToAllClients()
        {
            return new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = clientIDs } };
        }
    }
}