using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel_Anim : MonoBehaviour
{
    private Animation bM;

    private void Start()
    {
        bM = GetComponent<Animation>(); 
    }
    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            bM.Play();
            return; 
        }
    }
}
