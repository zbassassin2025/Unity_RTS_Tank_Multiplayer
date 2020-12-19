using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;
using System;

public class ZB_Unit : NetworkBehaviour
{
    [SerializeField] private UnityEvent onSelected = null;
    [SerializeField] private UnityEvent onDeselected = null;
    [SerializeField] private ZB_UnitMovement unitMovement = null;
    [SerializeField] private ZB_Health health; 

    [SerializeField] private ZB_Targeting target = null; // Targeter 

    public static event Action<ZB_Unit> ServerOnUnitSpawn;
    public static event Action<ZB_Unit> ServerOnUnitDeSpawned;

    public static event Action<ZB_Unit> AuthorityOnUnitSpawn;
    public static event Action<ZB_Unit> AuthorityOnUnitDeSpawn;

    #region Server

    public override void OnStartServer()
    {
        ServerOnUnitSpawn?.Invoke(this);

        health.ServerOnDie += ServerHandleDie; 
    }

    public override void OnStopServer()
    {
        ServerOnUnitDeSpawned?.Invoke(this);

        health.ServerOnDie -= ServerHandleDie;
    }

    [Server]
    private void ServerHandleDie()
    {
        NetworkServer.Destroy(gameObject); 
    }

    #endregion

    #region Client 

    public override void OnStartAuthority()
    {
        AuthorityOnUnitSpawn?.Invoke(this); 
    }

    public override void OnStopClient()
    {
        if (!hasAuthority)
        {
            return;
        }

        AuthorityOnUnitDeSpawn?.Invoke(this); 
    }

    public ZB_UnitMovement GetUnitMovement()
    {
        return unitMovement;
    }

    public ZB_Targeting GetTarget()
    {
        return target; 
    }

    [Client]
    public void Select()
    {
        if(!hasAuthority)
        {
            return; 
        }
        onSelected?.Invoke(); 
    }

    [Client]
    public void Deselect()
    {
        if(!hasAuthority)
        {
            return; 
        }
        onDeselected?.Invoke(); 
    }

    #endregion
}