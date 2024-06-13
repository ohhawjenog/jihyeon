using MPS;
using System;
using System.Collections;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Cylinder : MonoBehaviour
{
    // 상태
    [Header("현재 상태")]
    public string rearSwitchDeviceName;     //plc연동되는 부분
    public string frontSwitchDeviceName;    //plc연동되는 부분
    public int[] plcInputValues;

    public bool isCylinderMoving = false;   //실린더가 움직이고 있는지 확인
    public bool isStartPosition = true;
    public float speed = 1.0f;
    public float distanceLimit = 0.0003f;

    // 초기화
    [Space(20)]
    [Header("초기화")]
    public Transform pistonRod;         //움직이는 부분
    public Transform startPosition;     //초기위치
    public Transform endPosition;       //목표위치

    private void Start()
    {
        plcInputValues = new int[2];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && Input.GetKey(KeyCode.LeftShift))
        {
            StartCoroutine(CoMove(endPosition));
        }
        else if (Input.GetKeyDown(KeyCode.S) && Input.GetKey(KeyCode.LeftShift))
        {
            StartCoroutine(CoMove(startPosition));
        }

        if (MxComponent.instance.connection == MxComponent.Connection.Connected) //plc에서 신호를 받아옴
        {
            // 실린더 전진
            if (plcInputValues[0] > 0 && !isCylinderMoving)
                StartCoroutine(CoMove(endPosition));

            // 실린더 후진
            if (plcInputValues[1] > 0 && !isCylinderMoving)
                StartCoroutine(CoMove(startPosition));
        }
    }

    IEnumerator CoMove(Transform destination)
    {
        isCylinderMoving = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // 물리 엔진의 영향을 받지 않도록 설정
        }

        while (Vector3.Distance(transform.position, destination.position) > distanceLimit)
        {
            Vector3 newPos = Vector3.MoveTowards(transform.position, destination.position, speed * Time.deltaTime);
            transform.position = newPos;

            yield return null;
        }

        if (rb != null)
        {
            rb.isKinematic = false; // 다시 물리 엔진의 영향을 받도록 설정
        }

        isCylinderMoving = false;
    }

    public void SetSwitchDevicesByCylinderMoving(bool _isCylinderMoving, bool _isStartPosition)
    {
        if (_isCylinderMoving)
        {
            MxComponent.instance.SetDevice(rearSwitchDeviceName, 0);
            MxComponent.instance.SetDevice(frontSwitchDeviceName, 0);

            return;
        }

        if (isStartPosition)
        {
            MxComponent.instance.SetDevice(rearSwitchDeviceName, 1);
        }
        else
        {
            MxComponent.instance.SetDevice(frontSwitchDeviceName, 1);
        }
    }
}
