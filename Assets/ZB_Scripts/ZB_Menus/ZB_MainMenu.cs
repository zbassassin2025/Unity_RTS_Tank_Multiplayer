﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks; 
using Mirror; 

public class ZB_MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject landingPagePanel = null;
    [SerializeField] private bool useSteam; // use Steam App 

    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
    protected Callback<LobbyEnter_t> lobbyEntered;

    private void Start()
    {
        if(!useSteam)
        {
            return; 
        }

        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEnter); 
    }

    public void HostLobby()
    {
        landingPagePanel.SetActive(false);

        if(useSteam)
        {
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 4);
            return; 
        }

        NetworkManager.singleton.StartHost(); 
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if(callback.m_eResult != EResult.k_EResultOK)
        {
            landingPagePanel.SetActive(true);
            return; 
        }

        NetworkManager.singleton.StartHost();

        // set lobby data 
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby),
            "HostAddress", SteamUser.GetSteamID().ToString()); 
    }

    private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby); 
    }

    private void OnLobbyEnter(LobbyEnter_t callback)
    {
        if(NetworkServer.active)
        {
            return; 
        }

        string hostAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "HostAddress");

        NetworkManager.singleton.networkAddress = hostAddress; // Server / Host 
        NetworkManager.singleton.StartClient(); // Client 

        landingPagePanel.SetActive(false); 
    }
}
