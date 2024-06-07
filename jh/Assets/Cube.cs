using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public float speed = 1.0f;
    public Sensor sensor1;

    // Update is called once per frame
    void Update()
    {
        Vector3 front = new Vector3(1, 0, 0);
        transform.Translate(front * speed * Time.deltaTime);

        if (sensor1.isObjectDetected)
        {
            speed = 0;
            transform.Translate(front * speed * Time.deltaTime);
        }
    }
}
