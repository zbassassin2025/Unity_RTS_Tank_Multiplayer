using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZB_ResourcesDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text resourcesText = null;
    private ZB_RTS_Player player; 

    private void Update()
    {
        if (player == null)
        {
            return;
        }

        if (player != null)
        {
            ClientHandleResourcesUpdated(player.GetResources());

            player.ClientOnResourcesUpdated += ClientHandleResourcesUpdated;
        }

        player = NetworkClient.connection.identity.GetComponent<ZB_RTS_Player>();
        // this is trying to get component in Update without checking if player if there and errors  // get player object for connection 
    }

    private void OnDestroy()
    {
        player.ClientOnResourcesUpdated -= ClientHandleResourcesUpdated;
    }

    private void ClientHandleResourcesUpdated(int resources)
    {
        resourcesText.text = $"Resources:{resources}"; 
    }
}