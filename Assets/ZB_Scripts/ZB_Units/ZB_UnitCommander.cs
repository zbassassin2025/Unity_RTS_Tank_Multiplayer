using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ZB_UnitCommander : MonoBehaviour
{
    [SerializeField] private ZB_UnitSelectHandler unitSelect = null;
    private Camera mainCamera;
    [SerializeField] private LayerMask mask; 

    private void Start()
    {
        mainCamera = Camera.main;
        ZB_GameOverHandler.ClientOnGameOver += ClientHandleGameOver;
    }

    private void OnDestroy()
    {
        ZB_GameOverHandler.ClientOnGameOver -= ClientHandleGameOver;
    }

    private void Update()
    {
        if(!Mouse.current.rightButton.wasPressedThisFrame)
        {
            return; 
        }

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, mask))
        {
            return; 
        }

        if(hit.collider.TryGetComponent<ZB_Target>(out ZB_Target target))
        {
            if(target.hasAuthority)
            {
                TryMove(hit.point);
                return; 
            }

            TryTarget(target);
            return; 
        }

        TryMove(hit.point); 
    }

    private void TryTarget(ZB_Target target)
    {
        foreach (ZB_Unit unit in unitSelect.SelectedUnits)
        {
            unit.GetTarget().CmdSetTarget(target.gameObject); 
        }
    }

    private void TryMove(Vector3 point)
    {
        foreach(ZB_Unit unit in unitSelect.SelectedUnits)
        {
            unit.GetUnitMovement().CmdMove(point); 
        }
    }

    private void ClientHandleGameOver(string winnerName)
    {
        enabled = false; 
    }
}
