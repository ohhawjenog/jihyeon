using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor2 : MonoBehaviour
{
    public bool isObjectDetected = false;
    public Sensor2 sensor2;
    public Sensor3 sensor3;
    public Sensor4 sensor4;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Object"))
        {
            isObjectDetected = true;
            print("Sensor2 �ν�");
        }
    }

    /*private void Update()
    {
        // Sensor1�� �����ǰ� Sensor3 �Ǵ� Sensor4�� �����Ǹ� ResetSensors �ڷ�ƾ�� �����մϴ�.
        if (sensor2.isObjectDetected && (sensor3.isObjectDetected || sensor4.isObjectDetected))
        {
            StartCoroutine(DelayAndResetCoroutine());
        }
    }

    private IEnumerator DelayAndResetCoroutine()
    {
        // 3�� ���� ����մϴ�.
        yield return new WaitForSeconds(3f);

        // ������ �ʱ�ȭ�մϴ�.
        ResetSensors();
    }

    private void ResetSensors()
    {
        sensor2.isObjectDetected = false;
        sensor3.isObjectDetected = false;
        sensor4.isObjectDetected = false;

        Debug.Log("������ �ʱ�ȭ�Ǿ����ϴ�.");
    }*/
    
}