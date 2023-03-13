using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace SKGG.Netcode
{
    public class ConnectionUI : MonoBehaviour
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
            connectionPrompt.text = "Loading...";
            if(isHost)
            {
                StartGameLocalHost();
            }
            else
            {
                StartGameLocalClient();
            }
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

        private void StartGameLocalHost()
        {
            LocalConnectionManager.StartHost();
            playerTypeUI.SetActive(false);
            connectionTypeUI.SetActive(false);
            joinCodeText.gameObject.SetActive(false);
        }

        private void StartGameLocalClient()
        {
            LocalConnectionManager.StartClient();
            playerTypeUI.SetActive(false);
            connectionTypeUI.SetActive(false);
            joinCodeText.gameObject.SetActive(false);
        }

        private IEnumerator StartGameRelayHost()
        {
            Task<string> hostTask = RelayConnectionManager.StartHost(4);
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
            Task<RelayConnectionManager.ConnectionResult> clientTask = RelayConnectionManager.StartClient(joinCode);
            while (!clientTask.IsCompleted)
            {
                yield return null;
            }
            RelayConnectionManager.ConnectionResult result = clientTask.Result;
            switch (result)
            {
                case RelayConnectionManager.ConnectionResult.Successful:
                    break;
                case RelayConnectionManager.ConnectionResult.EmptyCode:
                    connectionPrompt.text = "Please input a code";
                    yield break;
                case RelayConnectionManager.ConnectionResult.InvalidCode:
                    connectionPrompt.text = "Invalid code entered";
                    yield break;
                case RelayConnectionManager.ConnectionResult.UnknownError:
                    connectionPrompt.text = "An unknown error has occured";
                    yield break;
            }
            playerTypeUI.SetActive(false);
            connectionTypeUI.SetActive(false);
            joinCodeText.gameObject.SetActive(true);
        }
    }
}