using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor1 : MonoBehaviour
{
    
    public Sensor1 sensor1;
    public Sensor2 sensor2;
    public Sensor3 sensor3;
    public Sensor4 sensor4;
    public bool isObjectDetected = false;
    private bool isCoroutineRunning = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Object"))
        {
            isObjectDetected = true;
            print("Sensor1 �ν�");
        }
    }

    /*private void Update()
    {
        // Sensor1�� �����ǰ� Sensor3 �Ǵ� Sensor4�� �����Ǹ� ResetSensors �ڷ�ƾ�� �����մϴ�.
        if (( sensor3.isObjectDetected || sensor4.isObjectDetected)&& !isCoroutineRunning)
        {
            StartCoroutine(DelayAndResetCoroutine());
        }
    }

    private IEnumerator DelayAndResetCoroutine()
    {
        isCoroutineRunning = true;  // �ڷ�ƾ�� ���� ������ ǥ��
        // 3�� ���� ����մϴ�.
        yield return new WaitForSeconds(3f);
        // ������ �ʱ�ȭ�մϴ�.
        ResetSensors();
        isCoroutineRunning = false;  // �ڷ�ƾ ���� �Ϸ� ǥ��
    }

    private void ResetSensors()
    {
        isObjectDetected = false;
        if (sensor2 != null)
        {
            sensor2.isObjectDetected = false;
        }
        if (sensor3 != null)
        {
            sensor3.isObjectDetected = false;
        }
        if (sensor4 != null)
        {
            sensor4.isObjectDetected = false;
        }

        Debug.Log("������ �ʱ�ȭ�Ǿ����ϴ�.");
    }*/

}