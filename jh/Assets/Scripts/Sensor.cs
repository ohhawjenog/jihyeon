using MPS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    //public Sensor sensor;
    public string sensorDeviceName;
    public Sensor alignSensor;

    public int plcInputValue;
    //public string sensorDeviceName;

    public bool isObjectDetected = false;
    public bool isSizeDetected = false;

    private void Update()
    {
        if (plcInputValue > 0)
        {
            if (this.gameObject.layer == LayerMask.NameToLayer("Size"))
                isSizeDetected = true;
            else
                isObjectDetected = true;
        }
        else
        {
            if (this.gameObject.layer == LayerMask.NameToLayer("Size"))
                isSizeDetected = false;
            else
                isObjectDetected = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Object"))
        {
            MxComponent.instance.SetDevice(sensorDeviceName, 1);
            isObjectDetected = true;
            print("Sensor °¨Áö");
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Size"))
        {
            if(alignSensor.isObjectDetected == true)
            {
                MxComponent.instance.SetDevice(sensorDeviceName, 1);
                isSizeDetected = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Object"))
        {
            MxComponent.instance.SetDevice(sensorDeviceName, 0);
            isObjectDetected = false;
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Size"))
        {
            MxComponent.instance.SetDevice(sensorDeviceName, 0);
            isSizeDetected = false;
        }
    }
}