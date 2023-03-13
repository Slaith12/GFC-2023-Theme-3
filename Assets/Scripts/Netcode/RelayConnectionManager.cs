using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace SKGG.Netcode
{
    //it would be more reasonable to call this "ConnectionManager" and call the current ConnectionManager "ServerManager"
    //but I have changes to ConnectionManager on another branch so changing the name now will cause merge conflicts
    public class RelayConnectionManager : MonoBehaviour
    {
        [SerializeField] GameObject playerTypeUI;
        private bool isHost;
        [SerializeField] GameObject connectionTypeUI;
        [SerializeField] TMP_Text connectionPrompt;
        [SerializeField] TMP_InputField joinCodeInput;
        [SerializeField] TMP_Text joinCodeText;

        const string defaultConnectionPrompt = "Select Connection Type";

        private void Awake()
        {
            ShowPlayerTypeUI();
        }

        public void ShowPlayerTypeUI()
        {
            playerTypeUI.SetActive(true);
            connectionTypeUI.SetActive(false);
            joinCodeText.gameObject.SetActive(false);
        }

        public void ShowConnectionTypeUI(bool host)
        {
            isHost = host;
            playerTypeUI.SetActive(false);
            connectionTypeUI.SetActive(true);
            connectionPrompt.text = defaultConnectionPrompt;
            joinCodeInput.gameObject.SetActive(!host);
            joinCodeText.gameObject.SetActive(false);
        }

        public void StartGameLocal()
        {
            connectionPrompt.text = "No. Choose Relay Server";
        }

        public void StartGameRelay()
        {
            connectionPrompt.text = "Loading...";
            if (isHost)
            {
                StartCoroutine(StartGameRelayHost());
            }
            else
            {
                StartCoroutine(StartGameRelayClient());
            }
        }

        private IEnumerator StartGameRelayHost()
        {
            Task<string> hostTask = RelayConnection.StartHost(3);
            while(!hostTask.IsCompleted)
            {
                yield return null;
            }
            string joinCode = hostTask.Result;
            joinCodeText.text = $"Join Code: {joinCode}";
            playerTypeUI.SetActive(false);
            connectionTypeUI.SetActive(false);
            joinCodeText.gameObject.SetActive(true);
        }

        private IEnumerator StartGameRelayClient()
        {
            string joinCode = joinCodeInput.text;
            joinCodeText.text = $"Join Code: {joinCode}";
            Task<RelayConnection.ConnectionResult> clientTask = RelayConnection.StartClient(joinCode);
            while (!clientTask.IsCompleted)
            {
                yield return null;
            }
            RelayConnection.ConnectionResult result = clientTask.Result;
            switch (result)
            {
                case RelayConnection.ConnectionResult.Successful:
                    break;
                case RelayConnection.ConnectionResult.EmptyCode:
                    connectionPrompt.text = "Please input a code";
                    yield break;
                case RelayConnection.ConnectionResult.InvalidCode:
                    connectionPrompt.text = "Invalid code entered";
                    yield break;
                case RelayConnection.ConnectionResult.UnknownError:
                    connectionPrompt.text = "An unknown error has occured";
                    yield break;
            }
            playerTypeUI.SetActive(false);
            connectionTypeUI.SetActive(false);
            joinCodeText.gameObject.SetActive(true);
        }
    }
}