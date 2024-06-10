using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 1.0f;
    public Sensor1 Sensor1;
    public Sensor2 Sensor2;
    public Sensor5 Sensor5;
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

        if (Sensor1.isObjectDetected)
        {
            isStopped = true;
            speed = 0;
        }

        if (Sensor2.isObjectDetected)
        {
            StartCoroutine(RestartAfterDelay(2.0f)); // 3�� �Ŀ� �ٽ� ����
        }

        if (Sensor5.isObjectDetected)
        {
            isStopped = true;
            speed = 0;
        }
    }

    private IEnumerator RestartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isStopped = false;
        speed = 1.0f; // ���� �ӵ��� �ٽ� �����̱� ����
    }
}
