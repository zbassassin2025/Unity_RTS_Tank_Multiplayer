using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInput_Test : MonoBehaviour
{
    public float runSpeed = 10f;
    public float jumpForce = 5f;
    private Rigidbody rb;
    private Vector3 movement = new Vector3();

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); 
    }

    private void FixedUpdate()
    {
        movement.x = runSpeed * Input.GetAxisRaw("Horizontal");
        movement.z = runSpeed * Input.GetAxisRaw("Vertical");

        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
        if(Input.GetButton("Jump"))
        {
            rb.velocity = Vector3.up * jumpForce;
        }
    }
}