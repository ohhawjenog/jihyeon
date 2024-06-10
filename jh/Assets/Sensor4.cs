using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor4 : MonoBehaviour
{
    public bool isSizeDetected = false;
    //public Sensor4 sensor4;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Size"))
        {
            isSizeDetected = true;
            print("Sensor4 ÀÎ½Ä");
        }
    }

}