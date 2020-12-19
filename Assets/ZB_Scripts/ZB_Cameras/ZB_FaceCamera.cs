using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZB_FaceCamera : MonoBehaviour
{
    private Transform mainCamTransform;

    private void Start()
    {
        mainCamTransform = Camera.main.transform;
    }

    private void LateUpdate() // called after Update 
    {
        transform.LookAt(transform.position + mainCamTransform.rotation * Vector3.forward, // cam forward
            mainCamTransform.rotation * Vector3.up); // cam up 
    }
}
