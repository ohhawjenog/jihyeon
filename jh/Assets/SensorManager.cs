using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorManager : MonoBehaviour
{
    public Sensor Sensor2;
    public Sensor sensor3;
    public Sensor sensor4;

    private bool resultPrinted = false;

    void Update()
    {
        if (Sensor2.isObjectDetected) {
            if (!resultPrinted)
            {
                if (sensor3.isSizeDetected && sensor4.isSizeDetected)
                {
                    Debug.Log("Box B");

                    resultPrinted = true;
                }
                else if (!sensor3.isSizeDetected && sensor4.isSizeDetected)
                {
                    Debug.Log("Box A");

                    resultPrinted = true;
                }

            }


        }
    }
}
