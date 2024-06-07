using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stopper : MonoBehaviour
{
    public float speed = 1.0f;
    public float distanceLimit = 0.3f;
    public Transform startPosition;
    public Transform destination;
    public Sensor sensor2;

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (sensor2.isObjectDetected)
        {
            Vector3 front = (destination.position - transform.position).normalized;
            float distance = (destination.position - transform.position).magnitude;

            if (distance > distanceLimit)
                transform.position += front * Time.deltaTime * speed;
            else
            {
                sensor2.isObjectDetected = false;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }

    }
}
