using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class ZB_GameOverHandler : NetworkBehaviour
{
    private List<ZB_UnitBase> bases = new List<ZB_UnitBase>();
    public static event Action<string> ClientOnGameOver;
    public static event Action ServerOnGameOver; 

    #region Server 

    public override void OnStartServer()
    {
        ZB_UnitBase.ServerOnBaseSpawn += ServerHandleBaseSpawned; // subscribe
        ZB_UnitBase.ServerOnBaseDeSpawn += ServerHandleBaseDeSpawned; 
    }

    public override void OnStopServer()
    {
        ZB_UnitBase.ServerOnBaseSpawn -= ServerHandleBaseSpawned; // unsubscribe 
        ZB_UnitBase.ServerOnBaseDeSpawn -= ServerHandleBaseDeSpawned;
    }

    [Server]
    private void ServerHandleBaseSpawned(ZB_UnitBase unitBase) // add unit base
    {
        bases.Add(unitBase); 
    }

    [Server]
    private void ServerHandleBaseDeSpawned(ZB_UnitBase unitBase) // remove unit base 
    {
        bases.Remove(unitBase); 

        if(bases.Count != 1) 
        {
            return; 
        }

        int playerId = bases[0].connectionToClient.connectionId; // check for connection id (player id connected)

        if(playerId <= bases[0].connectionToClient.connectionId)
        {
            Debug.Log("Game Over");

            RpcGameOver($"Player {playerId}"); // $ allows for variables to insert after "Text... {variable}"; 

            ServerOnGameOver?.Invoke(); // is the game over 
        }
    }

    #endregion

    #region Client 

    [ClientRpc]
    private void RpcGameOver(string winner)
    {
        ClientOnGameOver?.Invoke(winner); 
    }

    #endregion
}
