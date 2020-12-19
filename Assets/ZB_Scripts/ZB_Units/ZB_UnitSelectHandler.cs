using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ZB_UnitSelectHandler : MonoBehaviour
{
    [SerializeField] private LayerMask mask = new LayerMask();
    [SerializeField] private RectTransform unitSelectionArea = null;
    [SerializeField] private ZB_RTS_Player player = null;

    private Vector2 startPos; 
    private Camera mainCamera;

    public List<ZB_Unit> SelectedUnits { get; } = new List<ZB_Unit>(); 

    private void Start()
    {
        mainCamera = Camera.main;

        ZB_Unit.AuthorityOnUnitDeSpawn += AuthorityHandleUnitDespawned;
        ZB_GameOverHandler.ClientOnGameOver += ClientHandleGameOver; 
    }

    private void OnDestroy()
    {
        ZB_Unit.AuthorityOnUnitDeSpawn -= AuthorityHandleUnitDespawned;
        ZB_GameOverHandler.ClientOnGameOver -= ClientHandleGameOver; 
    }

    private void Update()
    {
        if(player == null)
        {
            player = NetworkClient.connection.identity.GetComponent<ZB_RTS_Player>();
            // this is trying to get component and errors  // get player object for connection 
        }

        if (Mouse.current.leftButton.wasPressedThisFrame) // if pressed left mouse button 
        {
            StartSelectArea(); 
        }
        else if(Mouse.current.leftButton.wasReleasedThisFrame) // clear selection area 
        {
            ClearSelectionArea(); 
        }
        else  if(Mouse.current.leftButton.isPressed)
        {
            UpdateSelectArea(); 
        }
    }

    private void UpdateSelectArea()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        float areaWidth = mousePos.x - startPos.x;
        float areaHeight = mousePos.y - startPos.y;

        unitSelectionArea.sizeDelta = new Vector2(Mathf.Abs(areaWidth), Mathf.Abs(areaHeight));
        unitSelectionArea.anchoredPosition = startPos + new Vector2(areaWidth / 2, areaHeight / 2); 
    }

    private void StartSelectArea()
    {
        if(!Keyboard.current.leftShiftKey.isPressed)
        {
            // Start selection area of units
            foreach (ZB_Unit selectedUnit in SelectedUnits)
            {
                selectedUnit.Deselect();
            }

            SelectedUnits.Clear();
        }

        unitSelectionArea.gameObject.SetActive(true);
        startPos = Mouse.current.position.ReadValue();
        UpdateSelectArea(); 
    }

    private void ClearSelectionArea()
    {
        unitSelectionArea.gameObject.SetActive(false);

        if (unitSelectionArea.sizeDelta.magnitude == 0)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, mask))
            {
                return;
            }

            if (!hit.collider.TryGetComponent(out ZB_Unit unit))
            {
                return;
            }

            if (!unit.hasAuthority)
            {
                return;
            }

            SelectedUnits.Add(unit);

            foreach (ZB_Unit selectedUnit in SelectedUnits)
            {
                selectedUnit.Select();
            }
            return;
        }

        Vector2 min = unitSelectionArea.anchoredPosition - (unitSelectionArea.sizeDelta / 2);
        Vector2 max = unitSelectionArea.anchoredPosition + (unitSelectionArea.sizeDelta / 2); 

        foreach(ZB_Unit unit in player.GetMyUnits())
        {
            if(SelectedUnits.Contains(unit))
            {
                continue; 
            }

            Vector3 screenPos = mainCamera.WorldToScreenPoint(unit.transform.position); 
            if(screenPos.x > min.x && 
                screenPos.x < max.x && 
                screenPos.y > min.y && 
                screenPos.y < max.y)
            {
                SelectedUnits.Add(unit);
                unit.Select(); 
            }
        }
    }

    private void AuthorityHandleUnitDespawned(ZB_Unit unit)
    {
        SelectedUnits.Remove(unit); 
    }

    private void ClientHandleGameOver(string winnerName)
    {
        enabled = false; 
    }
}