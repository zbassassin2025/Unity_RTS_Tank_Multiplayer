using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZB_UnitSpawner : NetworkBehaviour, IPointerClickHandler 
{
    [SerializeField] private GameObject unitPrefab = null;
    [SerializeField] private Transform unitSpawnTransform = null;
    [SerializeField] private ZB_Health health = null;


    #region Server // handles logic  

    public override void OnStartServer()
    {
        health.ServerOnDie += ServerHandleDie; 
    }

    public override void OnStopServer()
    {
        health.ServerOnDie -= ServerHandleDie;
    }

    [Server] 
    private void ServerHandleDie()
    {
        // called in Unit Base script
         NetworkServer.Destroy(gameObject);  // destroy networked objects 
    }

    [Command]
    private void CmdSpawnUnit()
    {
        GameObject unitInstance = Instantiate(unitPrefab, unitSpawnTransform.position, unitSpawnTransform.rotation);

        NetworkServer.Spawn(unitInstance, connectionToClient); // give authority to connected Client 
    }

    #endregion

    #region Client // calls logic 

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        if (!hasAuthority)
        {
            return;
        }

        CmdSpawnUnit();
    }

    #endregion
}