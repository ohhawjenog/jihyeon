using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public float speed = 1.0f;
    public Sensor sensor1;
    public Transform cylinder2;
    public Transform destination; // 목적지를 나타내는 변수 추가
    public float restartDistance = 1.0f; // 목적지에 도달하였다고 판단하는 거리

    private bool isStopped = false;

    void Update()
    {
        Vector3 front = new Vector3(1, 0, 0);

        if (!isStopped)
        {
            transform.Translate(front * speed * Time.deltaTime);
        }

        if (sensor1.isObjectDetected)
        {
            isStopped = true;
            speed = 0;
        }

        // cylinder2가 목적지에 충분히 가까워졌는지 확인
        if (isStopped && Vector3.Distance(cylinder2.position, destination.position) <= restartDistance)
        {
            isStopped = false;
            speed = 1.0f; // 원래 속도로 다시 움직이기 시작
        }
    }
}