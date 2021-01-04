using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ZB_Minimap : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [SerializeField] private RectTransform minimapRect = null;
    [SerializeField] private float mapScale = 30;
    [SerializeField] private float offSet = -6f; 
    private Transform playerCameraTransform;

    private void Update()
    {
        if(playerCameraTransform != null)
        {
            return; 
        }

        //if(NetworkClient.isLocalClient || NetworkClient.connection.identity == null)
        //{
        //    return;
        //}

       // playerCameraTransform = NetworkClient.connection.identity.GetComponent<ZB_RTS_Player>().GetCameraTransform();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        MoveCamera(); 
    }

    public void OnDrag(PointerEventData eventData)
    {
        MoveCamera(); 
    }

    private void MoveCamera()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue(); 
        if(!RectTransformUtility.ScreenPointToLocalPointInRectangle(
          minimapRect, 
          mousePos, 
          null,
          out Vector2 localPoint))
        {
            return; 
        }

        Vector2 lerp = new Vector2((
            localPoint.x - minimapRect.rect.x) / minimapRect.rect.width,
            (localPoint.y - minimapRect.rect.y) / minimapRect.rect.height); // linear interpolation 

        Vector3 newCameraPos = new Vector3(Mathf.Lerp(-mapScale, mapScale, lerp.x), 
           0f/*playerCameraTransform.position.y*/, Mathf.Lerp(-mapScale, mapScale, lerp.y));

       // playerCameraTransform.transform.position = newCameraPos + new Vector3(0f, 0f, offSet); 
    }
}