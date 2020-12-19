using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class ZB_GameOverDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text winnerNameText = null;
    [SerializeField] private GameObject gameOverDisplayObjectParent = null; 

    private void Start()
    {
        ZB_GameOverHandler.ClientOnGameOver += ClientHandleGameOver; // add event 
    }

    private void OnDestroy()
    {
        ZB_GameOverHandler.ClientOnGameOver -= ClientHandleGameOver; // remove event 
    }

    public void LeaveGame()
    {
        if(NetworkServer.active && NetworkClient.isConnected)
        {
            // stop hosting 
            NetworkManager.singleton.StopHost(); 
        }
        else
        {
            // stop client 
            NetworkManager.singleton.StopClient(); 
        }
    }


    private void ClientHandleGameOver(string winner)
    {
        winnerNameText.text = $"{winner} Has Won!";

        gameOverDisplayObjectParent.SetActive(true); 
    }
}
