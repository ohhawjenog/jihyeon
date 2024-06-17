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
    [Header("현재 상태")]
    public string rearSwitchDeviceName;     //plc연동되는 부분
    public string frontSwitchDeviceName;    //plc연동되는 부분
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

        if (MxComponent.instance.connection == MxComponent.Connection.Connected) //plc에서 신호를 받아옴
        {
            // 실린더 전진
            if (plcInputValues[0] > 0 && !isCylinderMoving && isStartPosition)
            {
                currentCoroutine = StartCoroutine(CoForward());
                isStartPosition = false;
            }

            // 실린더 후진
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
            rb.isKinematic = true; // 물리 엔진의 영향을 받지 않도록 설정
        }

        while (Vector3.Distance(transform.position, startPosition.position) > distanceLimit)
        {
            Vector3 newPos = Vector3.MoveTowards(transform.position, startPosition.position, speed * Time.deltaTime);
            transform.position = newPos;

            yield return null;
        }

        if (rb != null)
        {
            rb.isKinematic = false; // 다시 물리 엔진의 영향을 받도록 설정
        }

        isCylinderMoving = false;
        isStartPosition = true;
    }
}

