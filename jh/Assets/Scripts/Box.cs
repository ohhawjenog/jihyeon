using MPS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class Box : MonoBehaviour
{
    public float speed = 1.0f;
    public Sensor sensor1;
    public Sensor sensor2;
    public Sensor sensor3;
    public Sensor sensor4;
    public Sensor sensor5;
    public DriveMotor xTransfer;
    public DriveMotor yTransfer;
    public DriveMotor zTransfer;
    public bool boxADetected;
    public bool boxBDetected;
    public Transform cylinder2;
    public float restartDistance = 1.0f; // 목적지에 도달하였다고 판단하는 거리
    private bool resultPrinted = false;
    private bool isStopped = false;
    private Renderer boxRenderer;

    public string forwardName;

    public int plcInputValue;

    private void Start()
    {
        boxRenderer = GetComponent<Renderer>();
        if (boxRenderer != null)
        {
            boxRenderer.enabled = false; // 오브젝트를 비활성화 (렌더링 비활성화)
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)&&Input.GetKey(KeyCode.L))
        {
            if (boxRenderer != null)
            {
                boxRenderer.enabled = true; // 오브젝트를 활성화 (렌더링 활성화)
                Debug.Log("Space key pressed, object activated");
            }
        }

        if (!isStopped && boxRenderer.enabled)
        {
            Vector3 front = new Vector3(1, 0, 0);
            transform.Translate(front * speed * Time.deltaTime);

            if (sensor1.isObjectDetected)
            {
                isStopped = true;
                speed = 0;
            }

            if (sensor2.isObjectDetected)
            {
                StartCoroutine(RestartAfterDelay(3.0f)); // 3초 후에 다시 시작
                if (!resultPrinted)
                {
                    if (sensor3.isSizeDetected)
                    {
                        Debug.Log("Box A");
                        xTransfer.boxACount++;
                        yTransfer.boxACount++;
                        zTransfer.boxACount++;
                        boxADetected = true;
                        boxBDetected = false;
                        resultPrinted = true;
                    }
                    else if (sensor4.isSizeDetected)
                    {
                        Debug.Log("Box B");
                        xTransfer.boxBCount++;
                        yTransfer.boxBCount++;
                        zTransfer.boxBCount++;
                        boxADetected = false;
                        boxBDetected = true;
                        resultPrinted = true;
                    }
                }
            }
            if (sensor5.isObjectDetected)
            {
                isStopped = true;
                speed = 0;
            }
        }

        
    }

    private IEnumerator RestartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isStopped = false;
        speed = 1.0f; // 원래 속도로 다시 움직이기 시작
    }
}
