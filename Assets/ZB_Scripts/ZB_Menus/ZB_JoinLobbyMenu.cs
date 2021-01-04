using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class ZB_JoinLobbyMenu : MonoBehaviour
{
    [SerializeField] private GameObject landingPage = null;
    [SerializeField] private TMP_InputField addressInput = null;
    [SerializeField] private Button joinButton = null;

    private void OnEnable()
    {
        ZB_RTS_NetworkManager.ClientOnConnected += HandleClientConnected;
        ZB_RTS_NetworkManager.ClientOnDisconnected += HandleClientDisconnected; 
    }

    private void OnDisable()
    {
        ZB_RTS_NetworkManager.ClientOnConnected -= HandleClientConnected;
        ZB_RTS_NetworkManager.ClientOnDisconnected -= HandleClientDisconnected;
    }

    public void JoinButton()
    {
        string address = addressInput.text;

        NetworkManager.singleton.networkAddress = address; // get address id 
        NetworkManager.singleton.StartClient();

        joinButton.interactable = false;
    }

    private void HandleClientConnected()
    {
        joinButton.interactable = true;

        gameObject.SetActive(false);
        landingPage.SetActive(false); 
    }

    private void HandleClientDisconnected()
    {
        joinButton.interactable = true;
    }
}
