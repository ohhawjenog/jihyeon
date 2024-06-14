using MPS;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RotaryCylinder : MonoBehaviour
{
    //����
    [Header("���� ����")]
    public string rearSwitchDeviceName;     //plc�����Ǵ� �κ�
    public string frontSwitchDeviceName;    //plc�����Ǵ� �κ�
    public int[] plcInputValues;

    public float rotationSpeed = 90.0f; // �ʴ� ȸ�� �ӵ�
    private bool isRotating = false; // ȸ�� ������ Ȯ��
    private bool isStartPosition = true;

    private void Start()
    {
        plcInputValues = new int[2];
    }

    void Update()
    {

        if (MxComponent.instance.connection == MxComponent.Connection.Connected) //plc���� ��ȣ�� �޾ƿ�
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
            StartCoroutine(RotateCylinder(90.0f)); // 90�� ȸ��
        }
        else if (Input.GetKeyDown(KeyCode.F) && Input.GetKey(KeyCode.LeftShift) && !isRotating)
        {
            StartCoroutine(RotateCylinder(-90.0f)); // -90�� ȸ�� (������)
        }
    }

    IEnumerator RotateCylinder(float rotationAngle)
    {
        isRotating = true;

        float rotatedAngle = 0.0f; // ȸ���� ����
        float rotationDirection = Mathf.Sign(rotationAngle); // ȸ�� ���� (1 �Ǵ� -1)
        float targetAngle = Mathf.Abs(rotationAngle); // ��ǥ ȸ�� ����

        while (rotatedAngle < targetAngle)
        {
            float rotationThisFrame = rotationSpeed * Time.deltaTime; // ���� �����ӿ��� ȸ���� ����
            rotationThisFrame = Mathf.Min(rotationThisFrame, targetAngle - rotatedAngle); // ���� ������ŭ�� ȸ��
            transform.Rotate(Vector3.forward, rotationThisFrame * rotationDirection); // ȸ�� ����
            rotatedAngle += rotationThisFrame; // ���� ȸ�� ���� ������Ʈ
            yield return null; // ���� �����ӱ��� ���
        }

        isRotating = false;
    }
}


