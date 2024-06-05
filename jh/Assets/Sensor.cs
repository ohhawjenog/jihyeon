using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public bool isObjectDetected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Object"))
        {
            isObjectDetected = true;

            /*if (this.gameObject.layer == LayerMask.NameToLayer("Destination"))
            {
                print(this.gameObject.name);
            }*/
        }
    }
}