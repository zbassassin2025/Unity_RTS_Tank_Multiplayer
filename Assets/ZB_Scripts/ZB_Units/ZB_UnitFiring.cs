using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ZB_UnitFiring : NetworkBehaviour
{
    [SerializeField] private ZB_Targeting targeting = null;
    [SerializeField] private GameObject projPrefab = null;
    [SerializeField] private Transform projSpawn = null;
    [SerializeField] private float fireRange = 2f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float rotSpeed = 20f;

    private float lastFireTime;

    [ServerCallback] // won't log warnings every frame 
    private void Update()
    {
       /* Targetable */ ZB_Target target = targeting.GetTarget(); 

        if(target == null)
        {
            return; 
        }

        if(!CanFireAtTarget())
        {
            return; 
        }

        Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotSpeed * Time.deltaTime); 

        if(Time.time > (1 / fireRate) + lastFireTime) // each second fire
        {
            Quaternion projectileRotation = Quaternion.LookRotation(target.GetAimAtPoint().position - projSpawn.position); 

            GameObject projectileInstance = Instantiate(projPrefab, projSpawn.position, projectileRotation);

            NetworkServer.Spawn(projectileInstance, connectionToClient); // ownership of this script 

            lastFireTime = Time.time; 
        }
    }

    [Server]
    private bool CanFireAtTarget()
    {
        return (targeting.GetTarget().transform.position - transform.position).sqrMagnitude <= fireRange * fireRange; 
    }
}