﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror; 

public class ZB_MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject landingPagePanel = null;

    public void HostLobby()
    {
        landingPagePanel.SetActive(false);

        NetworkManager.singleton.StartHost(); 
    }
}
