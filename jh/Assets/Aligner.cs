using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aligner : MonoBehaviour
{
    public float speed = 1.0f;
    private Vector3 initialPosition;
    private bool movingForward = true;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        if (movingForward)
        {
            Vector3 front = new Vector3(0, 1, 0);
            transform.Translate(front * speed * Time.deltaTime);
        }
    }
}