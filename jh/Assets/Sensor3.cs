using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor3 : MonoBehaviour
{
    public bool isSizeDetected = false;
    public Sensor3 sensor3;
    public Sensor4 sensor4;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Size"))
        {
            isSizeDetected = true;
            if (sensor4.isSizeDetected && sensor3.isSizeDetected)
            {
                print("BOX B");
            }
        }
        
    }
}