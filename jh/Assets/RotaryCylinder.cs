using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RotaryCylinder : MonoBehaviour
{
    public float rotationAngle = 90.0f; // ȸ���� ����
    public float rotationSpeed = 1.0f; // ȸ�� �ӵ�
    private bool isRotating = false; // ȸ�� ������ Ȯ��
    private float currentRotation = 0.0f;

    void Update()
    {
        if (isRotating)
        {
            float step = rotationSpeed * Time.deltaTime; // ������ �� ȸ�� �ӵ�
            float rotationThisFrame = step * rotationAngle; // ���� �����ӿ��� ȸ���� ����

            if (currentRotation + rotationThisFrame >= rotationAngle)
            {
                rotationThisFrame = rotationAngle - currentRotation; // ���� ������ŭ�� ȸ��
                isRotating = false; // ȸ�� �Ϸ�
            }

            transform.Rotate(Vector3.up, rotationThisFrame); // ȸ�� ����
            currentRotation += rotationThisFrame; // ���� ȸ�� ���� ������Ʈ
        }

        // ���÷� �����̽��ٸ� ������ �� ȸ���� �����ϴ� �ڵ�
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

