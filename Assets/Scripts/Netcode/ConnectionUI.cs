using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        [SerializeField] GameObject disconnectButton;
        [Space]
        [SerializeField] bool changeSceneOnConnect;
        [SerializeField] int sceneToGoTo;
        [SerializeField] GameObject loadingPanel;

        const string defaultConnectionPrompt = "Select Connection Type";
        public const string savedCodeKey = "Saved Passcode";

        private void Awake()
        {
            if(changeSceneOnConnect && PlayerPrefs.HasKey(savedCodeKey))
            {
                joinCodeInput.text = PlayerPrefs.GetString(savedCodeKey);
            }
            ShowPlayerTypeUI();
        }

        public void ShowPlayerTypeUI()
        {
            playerTypeUI.SetActive(true);
            connectionTypeUI.SetActive(false);
            joinCodeText.gameObject.SetActive(false);
            disconnectButton.SetActive(false);
        }

        public void ShowConnectionTypeUI(bool host)
        {
            isHost = host;
            playerTypeUI.SetActive(false);
            connectionTypeUI.SetActive(true);
            connectionPrompt.text = defaultConnectionPrompt;
            joinCodeInput.gameObject.SetActive(!host);
            //joinCodeInput.text = "";
            joinCodeText.gameObject.SetActive(false);
            disconnectButton.SetActive(false);
        }

        public void StartGameLocal()
        {
            if (changeSceneOnConnect)
            {
                loadingPanel.SetActive(true);
                DontDestroyOnLoad(transform.parent.gameObject);
            }
            else
            {
                connectionPrompt.text = "Loading...";
            }
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
            if (changeSceneOnConnect)
            {
                loadingPanel.SetActive(true);
                //it has to use the parent gameobject because dontdestroyonload only works on root objects
                DontDestroyOnLoad(transform.parent.gameObject);
            }
            else
            {
                connectionPrompt.text = "Loading...";
            }
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
            disconnectButton.SetActive(true);
        }

        private void StartGameLocalClient()
        {
            LocalConnectionManager.StartClient();
            playerTypeUI.SetActive(false);
            connectionTypeUI.SetActive(false);
            joinCodeText.gameObject.SetActive(false);
            disconnectButton.SetActive(true);
        }

        private IEnumerator StartGameRelayHost()
        {
            if (changeSceneOnConnect)
            {
                AsyncOperation loadScene = SceneManager.LoadSceneAsync(sceneToGoTo);
                while (!loadScene.isDone)
                {
                    yield return null;
                }
            }
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
            disconnectButton.SetActive(true);
            if(changeSceneOnConnect)
            {
                loadingPanel.SetActive(false);
            }
        }

        private IEnumerator StartGameRelayClient()
        {
            string joinCode = joinCodeInput.text.ToUpper();
            joinCodeText.text = $"Join Code: {joinCode}";

            Task<RelayConnectionManager.ConnectionResult> clientTask = RelayConnectionManager.ConnectClient(joinCode);
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
                    if (changeSceneOnConnect)
                    {
                        loadingPanel.SetActive(false);
                    }
                    yield break;
                case RelayConnectionManager.ConnectionResult.InvalidCode:
                    connectionPrompt.text = "Invalid code entered";
                    if (changeSceneOnConnect)
                    {
                        loadingPanel.SetActive(false);
                    }
                    yield break;
                case RelayConnectionManager.ConnectionResult.UnknownError:
                    connectionPrompt.text = "An unknown error has occured";
                    if (changeSceneOnConnect)
                    {
                        loadingPanel.SetActive(false);
                    }
                    yield break;
            }

            if (changeSceneOnConnect)
            {
                AsyncOperation loadScene = SceneManager.LoadSceneAsync(sceneToGoTo);
                while (!loadScene.isDone)
                {
                    yield return null;
                }
            }

            NetworkManager.Singleton.StartClient();
            while(!NetworkManager.Singleton.IsConnectedClient || NetworkManager.Singleton.LocalClient.PlayerObject == null)
            {
                yield return null;
            }

            playerTypeUI.SetActive(false);
            connectionTypeUI.SetActive(false);
            joinCodeText.gameObject.SetActive(true);
            disconnectButton.SetActive(true);
            if (changeSceneOnConnect)
            {
                loadingPanel.SetActive(false);
            }
        }

        public void Disconnect()
        {
            bool isHost = NetworkManager.Singleton.IsHost;
            NetworkManager.Singleton.Shutdown();
            if(changeSceneOnConnect)
            {
                if (isHost)
                {
                    PlayerPrefs.SetString(savedCodeKey, joinCodeInput.text.ToUpper());
                }
                SceneManager.LoadSceneAsync(1);
                Destroy(transform.parent.gameObject);
                return;
            }
            ShowPlayerTypeUI();
        }
    }
}