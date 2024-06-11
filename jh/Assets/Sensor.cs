using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    //public Sensor sensor;
  
    public bool isObjectDetected = false;
    public bool isSizeDetected = false;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Object"))
        {
            isObjectDetected = true;
            print("Sensor °¨Áö");
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Size"))
        {
            isSizeDetected = true;
           
        }

    }

}
