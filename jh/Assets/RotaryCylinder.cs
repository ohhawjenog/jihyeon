using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RotaryCylinder : MonoBehaviour
{
    public float rotationAngle = 90.0f; // 회전할 각도
    public float rotationSpeed = 1.0f; // 회전 속도
    private bool isRotating = false; // 회전 중인지 확인
    private float currentRotation = 0.0f;

    void Update()
    {
        if (isRotating)
        {
            float step = rotationSpeed * Time.deltaTime; // 프레임 당 회전 속도
            float rotationThisFrame = step * rotationAngle; // 현재 프레임에서 회전할 각도

            if (currentRotation + rotationThisFrame >= rotationAngle)
            {
                rotationThisFrame = rotationAngle - currentRotation; // 남은 각도만큼만 회전
                isRotating = false; // 회전 완료
            }

            transform.Rotate(Vector3.up, rotationThisFrame); // 회전 적용
            currentRotation += rotationThisFrame; // 현재 회전 각도 업데이트
        }

        // 예시로 스페이스바를 눌렀을 때 회전을 시작하는 코드
        if (Input.GetKeyDown(KeyCode.Space) && !isRotating)
        {
            StartRotation();
        }
    }

    public void StartRotation()
    {
        isRotating = true;
        currentRotation = 0.0f;
    }
}

