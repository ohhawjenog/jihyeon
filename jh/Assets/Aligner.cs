using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aligner : MonoBehaviour
{
    public float speed = 1.0f;
    private Vector3 initialPosition;
    public Sensor sensor1;
    public Sensor sensor2;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        if (sensor1.isObjectDetected)
        {
            Vector3 front = new Vector3(0, 1, 0);
            transform.Translate(front * speed * Time.deltaTime);

            if (sensor2.isObjectDetected)
            {
                //Vector3 back = new Vector3(0, -1, 0);
                //transform.Translate(back * speed * Time.deltaTime);
                transform.position = Vector3.MoveTowards(transform.position, initialPosition, speed * Time.deltaTime);
            }
        }
    }
}