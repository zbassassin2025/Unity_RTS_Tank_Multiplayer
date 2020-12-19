using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ZB_ResourceGenerator : NetworkBehaviour
{
    [SerializeField] private ZB_Health health = null;
    [SerializeField] private int resourcesPerInterval = 10; // + 10 resources 
    [SerializeField] private float interval = 2; // each 2 seconds 

    private float timer;
    private ZB_RTS_Player player;

    public override void OnStartServer()
    {
        timer = interval;
        player = connectionToClient.identity.GetComponent<ZB_RTS_Player>();

        health.ServerOnDie += ServerHandleDie;
        ZB_GameOverHandler.ServerOnGameOver += ServerHandleGameOver; 
    }

    public override void OnStopServer()
    {
        health.ServerOnDie -= ServerHandleDie;
        ZB_GameOverHandler.ServerOnGameOver -= ServerHandleGameOver;
    }

    [ServerCallback]
    private void Update()
    {
        timer -= Time.deltaTime; 

        if(timer <= 0)
        {
            player.SetResources(player.GetResources() + resourcesPerInterval); 
            timer += interval; 
        }
    }

    private void ServerHandleDie()
    {
        NetworkServer.Destroy(gameObject); 
    }

    private void ServerHandleGameOver()
    {
        enabled = false;
    }
}
