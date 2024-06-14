using MPS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    //public Sensor sensor;

    public string forwardName;
    public string backwardName;

    public int plcInputValue;
    //public string sensorDeviceName;

    public bool isObjectDetected = false;
    public bool isSizeDetected = false;



    private void OnTriggerEnter(Collider other)
    {
        if(MxComponent.instance.connection == MxComponent.Connection.Connected)
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
}
