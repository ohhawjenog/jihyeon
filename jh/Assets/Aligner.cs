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
        if (sensor2.isObjectDetected)
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, speed * Time.deltaTime);

            // 초기 위치에 도달했는지 확인
            if (transform.position == initialPosition)
            {
                // 초기 위치에 도달하면 더 이상 움직이지 않음
                return;
            }
        }
        else if (sensor1.isObjectDetected)
        {
            Vector3 front = new Vector3(0, 1, 0);
            transform.Translate(front * speed * Time.deltaTime);
        }
    }
}