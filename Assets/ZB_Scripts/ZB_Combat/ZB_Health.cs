using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;
using System;

public class ZB_Health : NetworkBehaviour
{
    [SerializeField] private int maxHealth = 100;

    [SyncVar(hook = nameof(HandleHealthUpdated))] private int currHealth;

    public event Action ServerOnDie;
    public event Action<int, int> ClientOnHealthUpdated;

    #region Server 

    public override void OnStartServer()
    {
        currHealth = maxHealth;
        ZB_UnitBase.ServerOnPlayerDie += ServerHandlePlayerDie;
    }

    public override void OnStopServer()
    {
        ZB_UnitBase.ServerOnPlayerDie -= ServerHandlePlayerDie;
    }

    [Server]
    private void ServerHandlePlayerDie(int connectionId)
    {
        if(connectionToClient.connectionId != connectionId)
        {
            return; 
        }

        DealDamage(currHealth); 
    }

    [Server]
    public void DealDamage(int damageAmount)
    {
        if(currHealth == 0)
        {
            return; 
        }

        currHealth = Mathf.Max(currHealth - damageAmount, 0);

        if(currHealth != 0)
        {
            return; 
        }

        ServerOnDie?.Invoke();

        Debug.Log("We Died"); 
    }

    #endregion

    #region Client 

    private void HandleHealthUpdated(int oldHealth, int newHealth)
    {
        ClientOnHealthUpdated?.Invoke(newHealth, maxHealth); // sync health 
    }

    #endregion
}
