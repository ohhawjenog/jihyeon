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
    public Sensor Sensor1;
    public Sensor Sensor2;
    public Sensor sensor3;
    public Sensor sensor4;
    public Sensor Sensor5;
    public Transform cylinder2;
    public float restartDistance = 1.0f; // �������� �����Ͽ��ٰ� �Ǵ��ϴ� �Ÿ�
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
            boxRenderer.enabled = false; // ������Ʈ�� ��Ȱ��ȭ (������ ��Ȱ��ȭ)
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)&&Input.GetKey(KeyCode.L))
        {
            if (boxRenderer != null)
            {
                boxRenderer.enabled = true; // ������Ʈ�� Ȱ��ȭ (������ Ȱ��ȭ)
                Debug.Log("Space key pressed, object activated");
            }
        }

        if (!isStopped && boxRenderer.enabled)
        {
            Vector3 front = new Vector3(1, 0, 0);
            transform.Translate(front * speed * Time.deltaTime);
        }

        if (Sensor1.isObjectDetected)
        {
            isStopped = true;
            speed = 0;
        }
        if (Sensor2.isObjectDetected)
        {
            StartCoroutine(RestartAfterDelay(3.0f)); // 3�� �Ŀ� �ٽ� ����
        }
        if (Sensor5.isObjectDetected)
        {
            isStopped = true;
            speed = 0;
        }
        if (Sensor2.isObjectDetected)
        {
            if (!resultPrinted)
            {
                if (sensor3.isSizeDetected && sensor4.isSizeDetected)
                {
                    Debug.Log("Box B");
                    resultPrinted = true;
                }
                else if (!sensor3.isSizeDetected && sensor4.isSizeDetected)
                {
                    Debug.Log("Box A");
                    resultPrinted = true;
                }
            }
        }
    }

    private IEnumerator RestartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isStopped = false;
        speed = 1.0f; // ���� �ӵ��� �ٽ� �����̱� ����
    }
}
