using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ZB_UnitSpawner : NetworkBehaviour, IPointerClickHandler 
{
    [SerializeField] private ZB_Unit unitPrefab = null; // was Gameobject Type 
    [SerializeField] private Transform unitSpawnTransform = null;
    [SerializeField] private ZB_Health health = null;

    [SerializeField] private TMP_Text remainingUnitsText = null;
    [SerializeField] private Image unitProgressImage = null;
    [SerializeField] private int maxUnitQueue = 5;
    [SerializeField] private float spawnMoveRange = 6;
    [SerializeField] private float unitSpawnDuration = 4;

    [SyncVar(hook =nameof(ClientHandleQueueUnitsUdated))]
    private int queuedUnts;
    [SyncVar]
    private float unitTimer; 
    private float progressImageVelocity; 

    private void Update()
    {
        if(isServer)
        {
            ProduceUnits(); 
        }

        if(isClient)
        {
            UpdateTimerDisplay(); 
        }
    }

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
    private void ProduceUnits()
    {
        if(queuedUnts == 0)
        {
            return; 
        }

        unitTimer += Time.deltaTime; 
        if(unitTimer < unitSpawnDuration)
        {
            return; 
        }

        GameObject unitInstance = Instantiate(unitPrefab.gameObject, unitSpawnTransform.position, unitSpawnTransform.rotation);

        NetworkServer.Spawn(unitInstance, connectionToClient); // give authority to connected Client 

        Vector3 spawnOffset = Random.insideUnitSphere * spawnMoveRange;
        spawnOffset.y = unitSpawnTransform.position.y;

        ZB_UnitMovement unitMovement = unitInstance.GetComponent<ZB_UnitMovement>();
        unitMovement.ServerMove(unitSpawnTransform.position + spawnOffset);

        queuedUnts--;
        unitTimer = 0; 
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
       if(queuedUnts == maxUnitQueue)
        {
            return; 
        }

        ZB_RTS_Player player = connectionToClient.identity.GetComponent<ZB_RTS_Player>(); 
        if(player.GetResources() < unitPrefab.GetResourceCost())
        {
            return; 
        }

        queuedUnts++;
        player.SetResources(player.GetResources() - unitPrefab.GetResourceCost()); 
    }

    #endregion

    #region Client // calls logic 

    private void UpdateTimerDisplay()
    {
        float newProgress = unitTimer / unitSpawnDuration; 

        if(newProgress < unitProgressImage.fillAmount)
        {
            unitProgressImage.fillAmount = newProgress; 
        }
        else
        {
            unitProgressImage.fillAmount = Mathf.SmoothDamp(unitProgressImage.fillAmount,
                newProgress, ref progressImageVelocity, 0.1f); 
        }
    }

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

    private void ClientHandleQueueUnitsUdated(int oldUnits, int newUnits)
    {
        remainingUnitsText.text = newUnits.ToString(); 
    }

    #endregion
}