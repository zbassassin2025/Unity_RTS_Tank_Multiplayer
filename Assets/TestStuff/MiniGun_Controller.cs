using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGun_Controller : MonoBehaviour
{
    public float rotateSpeed = 15;

    private void Update()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        transform.Rotate((y * rotateSpeed * Time.deltaTime),
            (x * rotateSpeed * Time.deltaTime), 0, Space.World); 
    }
}