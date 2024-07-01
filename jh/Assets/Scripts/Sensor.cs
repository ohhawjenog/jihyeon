using MPS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public MxComponent mxComponent;

    public string sensorDeviceName;
    public Sensor alignSensor;
    public BoxManager boxManager;

    public int plcInputValue;

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
            mxComponent.SetDevice(sensorDeviceName, 1);
            isObjectDetected = true;
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Size") && boxManager.resultPrinted == false)
        {
            mxComponent.SetDevice(sensorDeviceName, 1);
            isSizeDetected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Object"))
        {
            mxComponent.SetDevice(sensorDeviceName, 0);
            isObjectDetected = false;
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Size"))
        {
            mxComponent.SetDevice(sensorDeviceName, 0);
            isSizeDetected = false;
        }
    }
}
