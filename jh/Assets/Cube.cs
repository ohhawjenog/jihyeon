using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public float speed = 1.0f;
    public Sensor sensor1;
    public Transform cylinder2;
    public Transform destination; // �������� ��Ÿ���� ���� �߰�
    public float restartDistance = 1.0f; // �������� �����Ͽ��ٰ� �Ǵ��ϴ� �Ÿ�

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

        // cylinder2�� �������� ����� ����������� Ȯ��
        if (isStopped && Vector3.Distance(cylinder2.position, destination.position) <= restartDistance)
        {
            isStopped = false;
            speed = 1.0f; // ���� �ӵ��� �ٽ� �����̱� ����
        }
    }
}