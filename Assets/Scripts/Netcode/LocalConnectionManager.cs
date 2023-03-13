using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

namespace SKGG.Netcode
{
    public static class LocalConnectionManager
    {
        const string localAddress = "127.0.0.1";
        const ushort localPort = 7777;

        public static void Initialize()
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(localAddress, localPort);
        }

        public static void StartHost()
        {
            Initialize();
            NetworkManager.Singleton.StartHost();
        }

        public static void StartClient()
        {
            Initialize();
            NetworkManager.Singleton.StartClient();
        }
    }
}