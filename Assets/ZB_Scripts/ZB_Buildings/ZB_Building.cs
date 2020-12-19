using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class ZB_Building : NetworkBehaviour
{
    [SerializeField] private Sprite icon = null;
    [SerializeField] private int id = -1; 
    [SerializeField] private int cost = 100; // price 
    [SerializeField] private GameObject buildingPreview = null; 

    public static event Action<ZB_Building> ServerOnBuildingSpawn;
    public static event Action<ZB_Building> ServerOnBuildingDeSpawned;

    public static event Action<ZB_Building> AuthorityOnBuildingSpawn;
    public static event Action<ZB_Building> AuthorityOnBuildingDeSpawn;

    // Geters 
    public GameObject GetBuildingPreview()
    {
        return buildingPreview; 
    }

    public Sprite GetIcon() 
    {
        return icon;
    }

    public int GetID()
    {
        return id; 
    }

    public int GetPrice()
    {
        return cost;
    }

    #region Server
    public override void OnStartServer()
    {
        ServerOnBuildingSpawn?.Invoke(this); // this is our building 
    }

    public override void OnStopServer()
    {
        ServerOnBuildingDeSpawned?.Invoke(this); 
    }

    #endregion

    #region Client 
    public override void OnStartAuthority()
    {
        AuthorityOnBuildingSpawn?.Invoke(this);
    }

    public override void OnStopClient()
    {
        if (!hasAuthority)
        {
            return;
        }

        AuthorityOnBuildingDeSpawn?.Invoke(this);
    }

    #endregion
}