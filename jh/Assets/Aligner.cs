using System;
using System.Collections;
using System.Collections.Generic;
<<<<<<< Updated upstream
using System.Threading;
using UnityEngine;

public class Aligner : MonoBehaviour
{
    public float speed = 1.0f;
    public Transform startPosition;
    public Sensor sensor1;
    public Sensor sensor2;
    public float distanceLimit = 0.3f;

    void Update()
    {
        if (sensor1.isObjectDetected)
        {
            Vector3 front = new Vector3(0, 0, -1);
            transform.position += front * Time.deltaTime * speed;

            if (sensor2.isObjectDetected)
            {
                sensor1.isObjectDetected = false;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
        else
        {
            Vector3 back = (startPosition.position - transform.position).normalized;
            float distance = (startPosition.position - transform.position).magnitude;

            if (distance > distanceLimit)
            {
                transform.position += back * Time.deltaTime * speed;
            }
            else
            {
                sensor1.isObjectDetected = false;
                sensor2.isObjectDetected = false;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
=======
using UnityEngine;


// ���۽� Player�� �� �������� �̵��Ѵ�.
public class Aligner: MonoBehaviour
{
    public float speed = 1.0f;
    private Vector3 initialPosition;
    private bool movingForward = true;

    void Start()
    {
        // �ʱ� ��ġ ����
        initialPosition = transform.position;
    }

    // �������� ���ŵ� �� ����Ǵ� �޼��� 0.002 ~ 0.004�ʿ� �ѹ��� ����
    void Update()
    {
        if (movingForward)
        {
            // ������ �̵�
            Vector3 front = new Vector3(0, 1, 0);
            transform.Translate(front * speed * Time.deltaTime);
>>>>>>> Stashed changes
        }
    }
}