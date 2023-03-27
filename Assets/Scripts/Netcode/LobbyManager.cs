using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    private static LobbyManager instance;

    private Lobby m_currentLobby;
    public static Lobby currentLobby { get => instance.m_currentLobby; private set => instance.m_currentLobby = value; }
    private Coroutine m_currentHeartBeat;
    private static Coroutine currentHeartBeat { get => instance.m_currentHeartBeat; set => instance.m_currentHeartBeat = value; }

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static async Task Initialize()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            await UnityServices.InitializeAsync();
        }
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    public static async Task CreateLobby(int numPlayers, bool isPrivate)
    {
        await Initialize();
        Lobby newLobby = await LobbyService.Instance.CreateLobbyAsync("Lobby", numPlayers, new CreateLobbyOptions { IsPrivate = isPrivate });
        if(currentLobby != null && currentLobby.HostId == AuthenticationService.Instance.PlayerId)
        {
            await EndLobby(currentLobby);
        }
        currentLobby = newLobby;
        currentHeartBeat = instance.StartCoroutine(HeartbeatLobby(newLobby.Id, 15));
    }

    private static IEnumerator HeartbeatLobby(string lobbyID, float waitTime)
    {
        var delay = new WaitForSecondsRealtime(waitTime);

        while (true)
        {
            LobbyService.Instance.SendHeartbeatPingAsync(lobbyID);
            yield return delay;
        }
    }

    public static async Task EndLobby(Lobby lobby)
    {
        instance.StopCoroutine(currentHeartBeat);
        currentHeartBeat = null;
        try
        {
            await LobbyService.Instance.DeleteLobbyAsync(lobby.Id);
        }
        catch(Exception ex)
        {
            Debug.LogError(ex);
        }
    }

    private void OnApplicationQuit()
    {
        if(currentLobby != null && currentLobby.HostId == AuthenticationService.Instance.PlayerId)
        {
            EndLobby(currentLobby);
        }
    }
}
