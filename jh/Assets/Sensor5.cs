using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor5 : MonoBehaviour
{
    public bool isObjectDetected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Object"))
        {
            isObjectDetected = true;
            print("Sensor5 ÀÎ½Ä");
        }
    }

}