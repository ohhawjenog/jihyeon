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
            print("Sensor1 인식");
        }
    }

    /*private void Update()
    {
        // Sensor1이 감지되고 Sensor3 또는 Sensor4가 감지되면 ResetSensors 코루틴을 시작합니다.
        if (( sensor3.isObjectDetected || sensor4.isObjectDetected)&& !isCoroutineRunning)
        {
            StartCoroutine(DelayAndResetCoroutine());
        }
    }

    private IEnumerator DelayAndResetCoroutine()
    {
        isCoroutineRunning = true;  // 코루틴이 실행 중임을 표시
        // 3초 동안 대기합니다.
        yield return new WaitForSeconds(3f);
        // 센서를 초기화합니다.
        ResetSensors();
        isCoroutineRunning = false;  // 코루틴 실행 완료 표시
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

        Debug.Log("센서가 초기화되었습니다.");
    }*/

}