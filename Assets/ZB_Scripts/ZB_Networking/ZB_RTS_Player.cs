﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class ZB_RTS_Player : NetworkBehaviour
{
    [SerializeField] private Transform cameraTransform = null; 
    [SerializeField] private ZB_Building[] buildings = new ZB_Building[0];
   // [SerializeField] private LayerMask buildingBlockLayer = new LayerMask();
    // [SerializeField] private float buildingRangeLimit = 5f; 
    // resources
    [SyncVar(hook = nameof(ClientHandleResourcesUpdated))]
    private int resources = 500;

    [SyncVar(hook =nameof(AuthorityHandlePartyOwnerStateUpdated))]
    private bool isPartyOwner;

    [SyncVar(hook =(nameof(ClientHandleDisplayNameUpdated)))]
    private string displayName; 

    private List<ZB_Unit> myUnits = new List<ZB_Unit>();
    private List<ZB_Building> myBuildings = new List<ZB_Building>();

    public event Action<int> ClientOnResourcesUpdated;

    public static event Action ClientOnInfoUpdated;
    public static event Action<bool> AuthorityOnPartyOwnerStateUpdated; 
    private Color teamColor = new Color(); 

    public string GetDisplayName()
    {
        return displayName; 
    }

    public bool GetIsPartyOwner()
    {
        return isPartyOwner; 
    }

    public Transform GetCameraTransform()
    {
        return cameraTransform; 
    }

    public Color GetTeamColor()
    {
        return teamColor; 
    }

    public int GetResources()
    {
        return resources; 
    }

    public List<ZB_Unit> GetMyUnits()
    {
        return myUnits;
    }

    public List<ZB_Building> GetMyBuildings()
    {
        return myBuildings; 
    }

    #region Server 

    public override void OnStartServer()
    {
        ZB_Unit.ServerOnUnitSpawn += ServerHandleUnitSpawned; // when method spawned on Server 
        ZB_Unit.ServerOnUnitDeSpawned += ServerHandleUnitDeSpawned;

        ZB_Building.ServerOnBuildingSpawn += ServerHandleBuildingSpawn;
        ZB_Building.ServerOnBuildingDeSpawned += ServerOnBuildingDeSpawned;

        DontDestroyOnLoad(gameObject); 
    }

    public override void OnStopServer()
    {
        ZB_Unit.ServerOnUnitSpawn -= ServerHandleUnitSpawned; 
        ZB_Unit.ServerOnUnitDeSpawned -= ServerHandleUnitDeSpawned;

        ZB_Building.ServerOnBuildingSpawn -= ServerHandleBuildingSpawn;
        ZB_Building.ServerOnBuildingDeSpawned -= ServerOnBuildingDeSpawned;
    }

    [Server]
    public void SetDisplayName(string displayName)
    {
        this.displayName = displayName; 
    }

    [Server]
    public void SetPartyOwner(bool state)
    {
        isPartyOwner = state; 
    }

    [Server]
    public void SetTeamColor(Color newTeamColor)
    {
        teamColor = newTeamColor;
    }

    [Server]
    public void SetResources(int newResources) // Seter 
    {
        resources = newResources;
    }

    [Command]
    public void CmdStartGame()
    {
        if(!isPartyOwner)
        {
            return; 
        }

        ((ZB_RTS_NetworkManager)NetworkManager.singleton).StartGame(); 
    }

    [Command]
    public void CmdTryPlaceBuilding(int buildingId, Vector3 point) // callback for Building Placement 
    {
        ZB_Building buildingToPlace = null; 

        foreach(ZB_Building building in buildings)
        {
            if(building.GetID() == buildingId)
            {
                buildingToPlace = building;
                break; // end this loop function 
            }
        }

        if(buildingToPlace == null)
        {
            return; 
        }

        GameObject buildingInstance = Instantiate(buildingToPlace.gameObject, point, buildingToPlace.transform.rotation);
        NetworkServer.Spawn(buildingInstance, connectionToClient); // spawn to client ownership 
    }

    private void ServerHandleUnitSpawned(ZB_Unit unit) // add unit 
    {
        if(unit.connectionToClient.connectionId != connectionToClient.connectionId)   // is client owner of this id / unit 
        {
            return;
        }
        myUnits.Add(unit);
    }

    private void ServerHandleUnitDeSpawned(ZB_Unit unit) // remove unit 
    {

        if (unit.connectionToClient.connectionId != connectionToClient.connectionId)  
        {
            return; 
        }
        myUnits.Remove(unit);
    }

    private void ServerHandleBuildingSpawn(ZB_Building building) // add building 
    {
        if (building.connectionToClient.connectionId != connectionToClient.connectionId)   // is client owner of this id / building 
        {
            return;
        }
        myBuildings.Add(building);
    }

    private void ServerOnBuildingDeSpawned(ZB_Building building) // remove building 
    {
        if (building.connectionToClient.connectionId != connectionToClient.connectionId)
        {
            return;
        }
        myBuildings.Remove(building);
    }

    #endregion

    #region Client 

    public override void OnStartAuthority()
    {
        if(NetworkServer.active) // if machine runs as a Server 
        {
            return; 
        }

        ZB_Unit.AuthorityOnUnitSpawn += AuthorityHandleUnitSpawned;  
        ZB_Unit.AuthorityOnUnitDeSpawn += AuthorityHandleUnitDeSpawned;
        ZB_Building.AuthorityOnBuildingSpawn += AuthorityHandleBuildingSpawned;
        ZB_Building.AuthorityOnBuildingDeSpawn += AuthorityHandleBuildingDeSpawned;
    }

    public override void OnStartClient()
    {
        if(NetworkServer.active)
        {
            return; 
        }

        DontDestroyOnLoad(gameObject); 

       ((ZB_RTS_NetworkManager) NetworkManager.singleton).Players.Add(this); 
    }

    public override void OnStopClient()
    {
        ClientOnInfoUpdated?.Invoke();

        if (!isClientOnly)
        {
            return;
        }

        ((ZB_RTS_NetworkManager)NetworkManager.singleton).Players.Remove(this);

        if(!hasAuthority)
        {
            return; 
        }

        ZB_Unit.AuthorityOnUnitSpawn -= AuthorityHandleUnitSpawned; 
        ZB_Unit.AuthorityOnUnitDeSpawn -= AuthorityHandleUnitDeSpawned;
        ZB_Building.AuthorityOnBuildingSpawn -= AuthorityHandleBuildingSpawned;
        ZB_Building.AuthorityOnBuildingDeSpawn -= AuthorityHandleBuildingDeSpawned;
    }

    private void ClientHandleDisplayNameUpdated(string oldDisplayName, string newDisplayName)
    {
        ClientOnInfoUpdated?.Invoke(); 
    }

    private void ClientHandleResourcesUpdated(int oldResources, int newResources) 
    {
        ClientOnResourcesUpdated?.Invoke(newResources);  
    }

    private void AuthorityHandlePartyOwnerStateUpdated(bool oldState, bool newState)
    {
        if(!hasAuthority)
        {
            return; 
        }

        AuthorityOnPartyOwnerStateUpdated?.Invoke(newState); 
    }

    private void AuthorityHandleUnitSpawned(ZB_Unit unit) 
    {
        myUnits.Add(unit);
    }

    private void AuthorityHandleUnitDeSpawned(ZB_Unit unit) 
    {
        myUnits.Remove(unit);
    }

    private void AuthorityHandleBuildingSpawned(ZB_Building building)
    {
        myBuildings.Add(building);
    }

    private void AuthorityHandleBuildingDeSpawned(ZB_Building building)
    {
        myBuildings.Remove(building);
    }

    #endregion
}