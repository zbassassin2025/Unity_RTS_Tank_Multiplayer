using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class ZB_UnitBase : NetworkBehaviour 
{
    [SerializeField] private ZB_Health health = null;

    public static event Action<ZB_UnitBase> ServerOnBaseSpawn;
    public static event Action<ZB_UnitBase> ServerOnBaseDeSpawn;
    public static event Action<int> ServerOnPlayerDie; 

    #region Server 

    public override void OnStartServer()
    {
        health.ServerOnDie += ServerHandleDie;
        ServerOnBaseSpawn?.Invoke(this); 
    }

    public override void OnStopServer()
    {
        ServerOnBaseDeSpawn?.Invoke(this);
        health.ServerOnDie -= ServerHandleDie;
    }

    [Server]
    private void ServerHandleDie()
    {
        ServerOnPlayerDie?.Invoke(connectionToClient.connectionId); 

        NetworkServer.Destroy(gameObject); 
    }

    #endregion

    #region Client 

    #endregion 
}
