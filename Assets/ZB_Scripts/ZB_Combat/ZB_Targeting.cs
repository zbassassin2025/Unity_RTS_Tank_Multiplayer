using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror; 

public class ZB_Targeting : NetworkBehaviour // Targeter 
{
   [SerializeField] private ZB_Target target; // Targetable 

    public ZB_Target GetTarget()
    {
        return target; 
    }

    public override void OnStartServer()
    {
        ZB_GameOverHandler.ServerOnGameOver += ServerHandleGameOver; 
    }

    public override void OnStopServer()
    {
        ZB_GameOverHandler.ServerOnGameOver -= ServerHandleGameOver;
    }

    #region Server 

    [Command]
    public void CmdSetTarget(GameObject targetGameObject)
    {
        if (target == null) // test 
        {
            target = FindObjectOfType<ZB_Target>().GetComponent<ZB_Target>();
        }

        if (!targetGameObject.TryGetComponent(out ZB_Target newTarget)) 
        {
            return; 
        }

        target = newTarget;
    }

    [Server]
    public void ClearTarget()
    {
        target = null; 
    }

    [Server]
    private void ServerHandleGameOver()
    {
        ClearTarget(); 
    }

    #endregion
}