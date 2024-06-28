using MPS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class BoxManager : MonoBehaviour
    {
        [Header("�ڽ� ����")]
        public GameObject boxAPrefab;
        public Transform spawnPoint; // �ڽ��� ������ ��ġ
        public GameObject boxBPrefab;

    [Space(20)]
        [Header("�ڽ� ������")]
        public Sensor Align_Sensor;
        public Sensor BoxA_Sensor;
        public Sensor BoxB_Sensor;

    [Space(20)]
        [Header("���� ����")]
        public string switchDeviceName; // plc �����Ǵ� �κ�
        public int plcInputValue = 0;
        public float speed = 1.0f;
        public Vector3 front = new Vector3(1, 0, 0);

        public List<Box1> boxes = new List<Box1>(); // ������ �ڽ��� ������ ����Ʈ

        private bool resultPrinted = false;
        public bool isBoxADetected;
        public bool isBoxBDetected;

    void Update()
        {
            if (MxComponent.instance.connection == MxComponent.Connection.Connected)
            {
                if (plcInputValue == 0)
                {
                    // PLC ��ȣ�� ������ ��� �ڽ� ����
                    foreach (var box in boxes)
                    {
                        box.StopMoving();
                    }
                }
                else if (plcInputValue > 0)
            {
                    // PLC ��ȣ�� ������ ��� �ڽ� ������ ����
                    foreach (var box in boxes)
                    {
                        box.StartMoving();
                    }
                }
            }
        if (Align_Sensor.isObjectDetected == true)
        {
            if (!resultPrinted)
            {
                if (BoxA_Sensor.isSizeDetected == true && BoxB_Sensor.isSizeDetected == false)
                {
                    print("Box A");
                    isBoxADetected = true;
                    isBoxBDetected = false;
                    resultPrinted = true;
                }
                else if (BoxA_Sensor.isSizeDetected == false && BoxB_Sensor.isSizeDetected == true)
                {
                    print("Box A");
                    isBoxADetected = false;
                    isBoxBDetected = true;
                    resultPrinted = true;
                }
            }
        }
    }

        // ��ư Ŭ�� �� ȣ��� �޼���
        public void OnCreateBoxButtonClick()
        {
            GameObject boxObj = null;

            int temp = Random.Range(0, 2);
            if (temp == 0)
            {
                boxObj = Instantiate(boxAPrefab, spawnPoint.position, spawnPoint.rotation);

            }
            else if (temp == 1)
            {
                boxObj = Instantiate(boxBPrefab, spawnPoint.position, spawnPoint.rotation);
            }
            

            Box1 boxScript = boxObj.AddComponent<Box1>();
            boxScript.speed = speed;
            boxScript.direction = front;
            boxes.Add(boxScript);
        }
    }


