using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies;
using UnityEngine;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] GameObject createLobbyPrompt;
    [SerializeField] TMP_Dropdown selectNumPlayers;
    [SerializeField] TMP_InputField choosePassword;
    [Space]
    [SerializeField] GameObject inputCodePrompt;
    [SerializeField] TMP_Text inputCodeLabel;
    [SerializeField] TMP_InputField inputCodeField;
    [SerializeField] TMP_Text inputCodeSubmitText;
    private bool inputtingPassword;
    [Space]
    [SerializeField] GameObject joinLobbyPrompt;
    [SerializeField] TMP_Text playerCountText;
    [SerializeField] TMP_Text hasPasswordText;

    private void Awake()
    {
        createLobbyPrompt.SetActive(false);
        inputCodePrompt.SetActive(false);
        joinLobbyPrompt.SetActive(false);
    }

    public void BeginCreateLobby()
    {
        inputCodePrompt.SetActive(false);
        joinLobbyPrompt.SetActive(false);
        selectNumPlayers.value = 3; //sets players to 4 (value of 0 = 1 player)
        choosePassword.text = "";
        createLobbyPrompt.SetActive(true);
    }

    public void FinishCreateLobby()
    {
        int numPlayers = selectNumPlayers.value + 1; //the "value" number will be 1 less than the number that the user inputted
        string password = choosePassword.text;
        LobbyManager.CreateLobby(numPlayers, false);
    }

    public void BeginSearchWithCode()
    {
        createLobbyPrompt.SetActive(false);
        joinLobbyPrompt.SetActive(false);
        //the input code prompt is shared with password inputting so set the appropriate labels
        inputtingPassword = false;
        inputCodeLabel.text = "Input Lobby Code";
        ((TMP_Text)inputCodeField.placeholder).text = "Lobby Code";
        inputCodeField.text = "";
        inputCodeSubmitText.text = "Find Lobby";
    }

    public void BeginInputPassword()
    {
        createLobbyPrompt.SetActive(false);
        joinLobbyPrompt.SetActive(false);
        //the input code prompt is shared with password inputting so set the appropriate labels
        inputtingPassword = true;
        inputCodeLabel.text = "Input Password";
        ((TMP_Text)inputCodeField.placeholder).text = "Password";
        inputCodeField.text = "";
        inputCodeSubmitText.text = "Join Lobby";
    }

    public void FinishInputCode()
    {
        string code = inputCodeField.text;
        if(inputtingPassword)
        {
            //join lobby using password
        }
        else
        {
            //search for lobby with code
        }
    }

    public void OpenLobbyDetails(int lobbyID)
    {
        createLobbyPrompt.SetActive(false);
        inputCodePrompt.SetActive(false);
        //update player count text and password text
        joinLobbyPrompt.SetActive(true);
    }

    public void JoinSelectedLobby()
    {

    }

    public void ClosePrompt()
    {
        createLobbyPrompt.SetActive(false);
        inputCodePrompt.SetActive(false);
        joinLobbyPrompt.SetActive(false);
    }
}
