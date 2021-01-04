using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ZB_LobbyMenu : MonoBehaviour
{
    [SerializeField] private GameObject lobbyUI = null;
    [SerializeField] private Button startGameButton = null;
    [SerializeField] private TMP_Text[] playerNameText = new TMP_Text[4]; 

    private void Start()
    {
        ZB_RTS_NetworkManager.ClientOnConnected += HandleClientConnected;
        ZB_RTS_Player.AuthorityOnPartyOwnerStateUpdated += AuthorityHandlePartyOwnerStateUpdated;
        ZB_RTS_Player.ClientOnInfoUpdated += ClientHandleInfoUpdated; 
    }

    private void OnDestroy()
    {
        ZB_RTS_NetworkManager.ClientOnConnected -= HandleClientConnected;
        ZB_RTS_Player.AuthorityOnPartyOwnerStateUpdated -= AuthorityHandlePartyOwnerStateUpdated;
        ZB_RTS_Player.ClientOnInfoUpdated -= ClientHandleInfoUpdated;
    }

    private void ClientHandleInfoUpdated()
    {
        List<ZB_RTS_Player> players = ((ZB_RTS_NetworkManager)NetworkManager.singleton).Players;

        for(int i = 0; i < players.Count; i++)
        {
            playerNameText[i].text = players[i].GetDisplayName(); 
        }

        for (int i = players.Count; i < playerNameText.Length; i++)
        {
            playerNameText[i].text = "Waiting for player..."; 
        }

        startGameButton.interactable = players.Count >= 2; // if more than 1 player
    }

    private void HandleClientConnected()
    {
        lobbyUI.SetActive(true); 
    }

    private void AuthorityHandlePartyOwnerStateUpdated(bool state)
    {
        startGameButton.gameObject.SetActive(state); 
    }

    public void StartGame()
    {
        NetworkClient.connection.identity.GetComponent<ZB_RTS_Player>().CmdStartGame(); 
    }

    public void LeaveLobby()
    {
        if(NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost(); 
        }
        else
        {
            NetworkManager.singleton.StopClient();

            SceneManager.LoadScene(0); 
        }
    }
}