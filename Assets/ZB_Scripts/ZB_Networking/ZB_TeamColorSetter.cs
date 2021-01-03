using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZB_TeamColorSetter : NetworkBehaviour 
{
    [SerializeField] private Renderer[] colorRenderer = new Renderer[0];

    [SyncVar(hook =nameof(HandleTeamColorUpdated))]
    private Color teamColor = new Color(); 

    #region Server

    public override void OnStartServer()
    {
        ZB_RTS_Player player = connectionToClient.identity.GetComponent<ZB_RTS_Player>();

        teamColor = player.GetTeamColor(); 
    }

    #endregion

    #region Client 

    private void HandleTeamColorUpdated(Color oldColor, Color newColor)
    {
        foreach(Renderer renderer in colorRenderer)
        {
            renderer.material.SetColor("_BaseColor", newColor); 
        }
    }

    #endregion
}