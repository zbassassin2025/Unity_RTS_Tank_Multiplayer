using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ZB_BuildingButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler 
{
    [SerializeField] private ZB_Building building = null;
    [SerializeField] private Image iconImg = null;
    [SerializeField] private TMP_Text priceText = null;
    [SerializeField] private LayerMask mask = new LayerMask();

    private Camera mainCamera;
    private ZB_RTS_Player player;

    private GameObject buildingPreviewInstance;
    private Renderer buildingRendererInstance;

    private void Start()
    {
        mainCamera = Camera.main;
        iconImg.sprite = building.GetIcon();
        priceText.text = building.GetPrice().ToString();

        player = NetworkClient.connection.identity.GetComponent<ZB_RTS_Player>();
    }

    private void Update()
    { 
        if(buildingPreviewInstance == null)
        {
            return; 
        }

        UpdateBuildingPreview(); 
    }

    public void OnPointerDown(PointerEventData eventData)
    {
       if(eventData.button != PointerEventData.InputButton.Left)
        {
            return; 
        }

        buildingPreviewInstance = Instantiate(building.GetBuildingPreview());
        buildingRendererInstance = buildingPreviewInstance.GetComponentInChildren<Renderer>();

        buildingPreviewInstance.SetActive(true); 
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(buildingPreviewInstance == null)
        {
            return; 
        }

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()); 

        if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, mask))
        {
            // place building
            // make command, client tell server to do something 
            player.CmdTryPlaceBuilding(building.GetID(), hit.point); 
        }

        Destroy(buildingPreviewInstance); 
    }

    private void UpdateBuildingPreview()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, mask))
        {
            return; 
        }

        buildingPreviewInstance.transform.position = hit.point; // where we hit with ray place there
        Debug.Log(hit.point); 

        if(!buildingPreviewInstance.activeSelf)
        {
            buildingPreviewInstance.SetActive(false); 
        }
        else
        {
            buildingPreviewInstance.SetActive(true); 
        }
    }

}