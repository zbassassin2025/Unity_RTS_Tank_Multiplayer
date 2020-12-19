using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ZB_Target : NetworkBehaviour // Targetable 
{
    [SerializeField] private Transform aimAtPoint = null; 

    public Transform GetAimAtPoint()
    {
        return aimAtPoint; 
    }
}