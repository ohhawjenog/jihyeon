using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Aligner : MonoBehaviour
{
    public float speed = 1.0f;
    public Transform startPosition;
    public float distanceLimit = 0.003f;

    public bool isCylinderMoving = false;
    private Coroutine currentCoroutine;

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
    }
}

