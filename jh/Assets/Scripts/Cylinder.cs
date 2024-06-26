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
    public Sensor alignSensor;

    public bool isCylinderMoving = false;   //실린더가 움직이고 있는지 확인
    public bool isStartPosition = true;
    public float speed = 1.0f;
    public float distanceLimit = 0.03f;

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
            if (plcInputValues[0] > 0 && !isCylinderMoving && isStartPosition)
            {
                StartCoroutine(CoMove(endPosition));
                isStartPosition = false;
            }

            // 실린더 후진
            if (plcInputValues[1] > 0 && !isCylinderMoving && !isStartPosition)
            {
                StartCoroutine(CoMove(startPosition));
                isStartPosition = true;
            }
        }
    }

    IEnumerator CoMove(Transform destination)
    {
        isCylinderMoving = true;

        if (this.gameObject.layer == LayerMask.NameToLayer("Aligner") && alignSensor.isObjectDetected == false)
        {
            while (alignSensor.isObjectDetected == false)
            {
                Vector3 newPos = Vector3.MoveTowards(transform.position, destination.position, speed * Time.deltaTime);
                transform.position = newPos;
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }

        else
        {
            while (Vector3.Distance(transform.position, destination.position) > distanceLimit)
            {
                Vector3 newPos = Vector3.MoveTowards(transform.position, destination.position, speed * Time.deltaTime);
                transform.position = newPos;

                yield return new WaitForSeconds(Time.deltaTime);
            }
        }

        isCylinderMoving = false;
    }
}
