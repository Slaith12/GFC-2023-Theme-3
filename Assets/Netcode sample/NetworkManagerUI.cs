using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] Button serverBtn;
    [SerializeField] Button hostBtn;
    [SerializeField] Button clientBtn;

    private void Awake()
    {
        serverBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
        });
        hostBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });
        clientBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
    }
}
