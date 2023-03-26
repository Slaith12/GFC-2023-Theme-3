using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace SKGG.Netcode
{
    public static class RelayConnectionManager
    {
        public enum ConnectionResult { Successful, EmptyCode, InvalidCode, UnknownError }

        //dtls is a secure connection according to unity, so that's the connection we're using
        const string connectionType = "dtls";

        //"Task" is basically info on whether the function's finished yet.
        //you can either use "await" to wait for the task to finish or save it to a variable to do other things and check in later to see if it's done
        public static async Task Initialize()
        {
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                await UnityServices.InitializeAsync();
            }
            //Relay requires you sign in first
            if(!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }

        public static async Task<string> StartHost(int maxPlayers, string region = null)
        {
            await Initialize();

            //the first parameter to CreateAllocationAsync is the number of connections besides the host to open, so it's 1 less than the max players
            //null region = find closest available server
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers - 1, region);
            //minor(?) optimization because I don't know how long this or the next steps take (i don't even know if the task starts if you do it like this)
            Task<string> joinCode = RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.SetRelayServerData(new RelayServerData(allocation, connectionType));
            NetworkManager.Singleton.StartHost();
            return await joinCode;
        }

        public static async Task<ConnectionResult> ConnectClient(string joinCode)
        {
            if (string.IsNullOrEmpty(joinCode))
            {
                return ConnectionResult.EmptyCode;
            }
            await Initialize();

            JoinAllocation allocation;

            try { allocation = await RelayService.Instance.JoinAllocationAsync(joinCode); }
            catch (RelayServiceException ex)
            {
                Debug.LogError(ex.Message + "\n" + ex.StackTrace);
                return ConnectionResult.InvalidCode;
            }
            try
            {
                UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
                transport.SetRelayServerData(new RelayServerData(allocation, connectionType));
            }
            catch(Exception ex)
            {
                Debug.LogError(ex.Message + "\n" + ex.StackTrace);
                return ConnectionResult.UnknownError;
            }
            return ConnectionResult.Successful;
        }

        public static async Task<ConnectionResult> StartClient(string joinCode)
        {
            ConnectionResult result = await ConnectClient(joinCode);
            NetworkManager.Singleton.StartClient();
            return result;
        }
    }
}