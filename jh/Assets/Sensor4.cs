using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor4 : MonoBehaviour
{
    public bool isSizeDetected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Size"))
        {
            isSizeDetected = true;
            print("BOX A");
        }
        
    }

}