using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror; 

public class ZB_UnitProjectile : NetworkBehaviour
{
    [SerializeField] private Rigidbody rb = null;
    [SerializeField] private float fireForce = 10f;
    [SerializeField] private float destroyTime = 5f;
    [SerializeField] private int dealDamage = 20; 

    private void Start()
    {
        rb.velocity = transform.forward * fireForce; 
    }

    public override void OnStartServer()
    {
        Invoke(nameof(DestroySelf), destroyTime); 
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<NetworkIdentity>(out NetworkIdentity networkIdentity))
        {
            if(networkIdentity.connectionToClient == connectionToClient)
            {
                return; 
            }
        }

        if(other.TryGetComponent<ZB_Health>(out ZB_Health health))
        {
            health.DealDamage(dealDamage); // 5 hits to destroy object  
        }

        DestroySelf(); 
    }

    [Server]
    private void DestroySelf()
    {
        NetworkServer.Destroy(gameObject); 
    }

}