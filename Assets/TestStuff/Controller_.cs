using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_ : MonoBehaviour
{
    public float speed; 

    private void Update()
    {
        if (Input.GetKey(KeyCode.W)) // up 
        {
            GetComponent<Rigidbody>().AddForce(Vector3.forward * speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.A)) // left
        {
            GetComponent<Rigidbody>().AddForce(Vector3.left * speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S)) // down
        {
            GetComponent<Rigidbody>().AddForce(Vector3.back * speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D)) // right
        {
            GetComponent<Rigidbody>().AddForce(Vector3.right * speed * Time.deltaTime);
        }
    }
}
