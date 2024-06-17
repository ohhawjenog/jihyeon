using MPS;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RotaryCylinder : MonoBehaviour
{
    //상태
    [Header("현재 상태")]
    public string rearSwitchDeviceName;     //plc연동되는 부분
    public string frontSwitchDeviceName;    //plc연동되는 부분
    public int[] plcInputValues;

    public float rotationSpeed = 90.0f; // 초당 회전 속도
    private bool isRotating = false; // 회전 중인지 확인
    private bool isStartPosition = true;

    private void Start()
    {
        plcInputValues = new int[2];
    }

    void Update()
    {

        if (MxComponent.instance.connection == MxComponent.Connection.Connected) //plc에서 신호를 받아옴
        {
            if (plcInputValues[0] > 0 && isStartPosition && !isRotating)
            {
                StartCoroutine(RotateCylinder(90.0f));
                isStartPosition = false;
            }

            if (plcInputValues[1] > 0 && !isStartPosition && !isRotating)
            { 
                StartCoroutine(RotateCylinder(-90.0f));
                isStartPosition = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.D) && Input.GetKey(KeyCode.LeftShift) && !isRotating)
        {
            StartCoroutine(RotateCylinder(90.0f)); // 90도 회전
        }
        else if (Input.GetKeyDown(KeyCode.F) && Input.GetKey(KeyCode.LeftShift) && !isRotating)
        {
            StartCoroutine(RotateCylinder(-90.0f)); // -90도 회전 (역방향)
        }
    }

    IEnumerator RotateCylinder(float rotationAngle)
    {
        isRotating = true;

        float rotatedAngle = 0.0f; // 회전된 각도
        float rotationDirection = Mathf.Sign(rotationAngle); // 회전 방향 (1 또는 -1)
        float targetAngle = Mathf.Abs(rotationAngle); // 목표 회전 각도

        while (rotatedAngle < targetAngle)
        {
            float rotationThisFrame = rotationSpeed * Time.deltaTime; // 현재 프레임에서 회전할 각도
            rotationThisFrame = Mathf.Min(rotationThisFrame, targetAngle - rotatedAngle); // 남은 각도만큼만 회전
            transform.Rotate(Vector3.forward, rotationThisFrame * rotationDirection); // 회전 적용
            rotatedAngle += rotationThisFrame; // 누적 회전 각도 업데이트
            yield return null; // 다음 프레임까지 대기
        }

        isRotating = false;
    }
}


