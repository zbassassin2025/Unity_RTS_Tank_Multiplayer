using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyberCar_Controller : MonoBehaviour
{
    private Rigidbody rB;
    public float speed = 45;
    public bool forward;
    public bool back;
    public bool left;
    public bool right;
    [SerializeField] public MiniGun_Controller mC;
    public Transform gunP;
    public GameObject mG; 

    private void Start()
    {
        rB = GetComponent<Rigidbody>(); 
    }

    private void Update()
    {
        MoveCar(); 
    }

    private void FixedUpdate()
    {
        if(forward == true)
        {
            rB.AddForce(Vector3.forward * speed * 2 * Time.deltaTime); 
        }
        else if(back == true)
        {
            rB.AddForce(Vector3.back * speed * 2 * Time.deltaTime);
        }
        else if(left == true)
        {
            rB.AddTorque(Vector3.left * speed * 5 * Time.deltaTime);
            rB.angularVelocity += Vector3.up * 500; 
        }
        else if (right == true)
        {
            rB.AddTorque(Vector3.right * speed * 5 * Time.deltaTime);
            rB.angularVelocity -= Vector3.up * 500;
        }
    }

    private void MoveCar()
    {
        if(Input.GetKey(KeyCode.W))
        {
            forward = true;
            rB.drag -= 0.1f;
        }
        else
        {
            rB.drag = 1;
            forward = false;
        }
        if (Input.GetKey(KeyCode.S))
        {
            back = true;
            rB.drag -= 0.1f;
        }
        else
        {
            rB.drag = 1;
            back = false;
        }
        if (Input.GetKey(KeyCode.A))
        {
            left = true;
            rB.drag -= 0.1f;
        }
        else
        {
            rB.drag = 1;
            left = false;
        }
        if (Input.GetKey(KeyCode.D))
        {
            right = true;
            rB.drag -= 0.1f;
        }
        else
        {
            rB.drag = 1;
            right = false;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            mG.gameObject.transform.position = gunP.transform.position;
            mC.enabled = true; 
        }
    }
}