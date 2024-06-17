using MPS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Aligner : MonoBehaviour
{
    [Header("���� ����")]
    public string rearSwitchDeviceName;     //plc�����Ǵ� �κ�
    public string frontSwitchDeviceName;    //plc�����Ǵ� �κ�
    public int[] plcInputValues;

    public float speed = 1.0f;
    public Transform startPosition;
    public float distanceLimit = 0.003f;

    public bool isCylinderMoving = false;
    public bool isStartPosition = true;

    private Coroutine currentCoroutine;

    private void Start()
    {
        plcInputValues = new int[2];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && Input.GetKey(KeyCode.LeftShift) && !isCylinderMoving)
        {
            currentCoroutine = StartCoroutine(CoForward());
        }
        else if (Input.GetKeyDown(KeyCode.H) && Input.GetKey(KeyCode.LeftShift) && isCylinderMoving)
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
                isCylinderMoving = false;
            }
            StartCoroutine(CoInitialize());
        }

        if (MxComponent.instance.connection == MxComponent.Connection.Connected) //plc���� ��ȣ�� �޾ƿ�
        {
            // �Ǹ��� ����
            if (plcInputValues[0] > 0 && !isCylinderMoving && isStartPosition)
            {
                currentCoroutine = StartCoroutine(CoForward());
                isStartPosition = false;
            }

            // �Ǹ��� ����
            if (plcInputValues[1] > 0 && isCylinderMoving && !isStartPosition)
            {
                if (currentCoroutine != null)
                {
                    StopCoroutine(currentCoroutine);
                    currentCoroutine = null;
                }
                currentCoroutine = StartCoroutine(CoInitialize());
            }
        }

    }

    IEnumerator CoForward()
    {
        isCylinderMoving = true;

        while (true)
        {
            Vector3 front = new Vector3(0, 0, -1);
            transform.position += front * Time.deltaTime * speed;

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    IEnumerator CoInitialize()
    {
        isCylinderMoving = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // ���� ������ ������ ���� �ʵ��� ����
        }

        while (Vector3.Distance(transform.position, startPosition.position) > distanceLimit)
        {
            Vector3 newPos = Vector3.MoveTowards(transform.position, startPosition.position, speed * Time.deltaTime);
            transform.position = newPos;

            yield return null;
        }

        if (rb != null)
        {
            rb.isKinematic = false; // �ٽ� ���� ������ ������ �޵��� ����
        }

        isCylinderMoving = false;
        isStartPosition = true;
    }
}

