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
            print("Sensor2 인식");
        }
    }

    /*private void Update()
    {
        // Sensor1이 감지되고 Sensor3 또는 Sensor4가 감지되면 ResetSensors 코루틴을 시작합니다.
        if (sensor2.isObjectDetected && (sensor3.isObjectDetected || sensor4.isObjectDetected))
        {
            StartCoroutine(DelayAndResetCoroutine());
        }
    }

    private IEnumerator DelayAndResetCoroutine()
    {
        // 3초 동안 대기합니다.
        yield return new WaitForSeconds(3f);

        // 센서를 초기화합니다.
        ResetSensors();
    }

    private void ResetSensors()
    {
        sensor2.isObjectDetected = false;
        sensor3.isObjectDetected = false;
        sensor4.isObjectDetected = false;

        Debug.Log("센서가 초기화되었습니다.");
    }*/
    
}