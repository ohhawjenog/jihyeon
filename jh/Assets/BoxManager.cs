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

public class BoxManager : MonoBehaviour
    {
        [Header("�ڽ� A ����")]
        public GameObject boxAPrefab;
        public Transform spawnPointA; // �ڽ� A�� ������ ��ġ

        [Header("�ڽ� B ����")]
        public GameObject boxBPrefab;
        public Transform spawnPointB; // �ڽ� B�� ������ ��ġ

        [Space(20)]
        [Header("���� ����")]
        public string switchDeviceName; // plc �����Ǵ� �κ�
        public int plcInputValue;
        public float speed = 1.0f;
        public Vector3 front = new Vector3(1, 0, 0);

        private List<Box> boxesA = new List<Box>(); // ������ �ڽ� A�� ������ ����Ʈ
        private List<Box> boxesB = new List<Box>(); // ������ �ڽ� B�� ������ ����Ʈ

        void Update()
        {
            if (MxComponent.instance.connection == MxComponent.Connection.Connected)
            {
                if (plcInputValue == 0)
                {
                    // PLC ��ȣ�� ������ ��� �ڽ� ����
                    foreach (var box in boxesA)
                    {
                        box.StopMoving();
                    }
                    foreach (var box in boxesB)
                    {
                        box.StopMoving();
                    }
                }
                else if (plcInputValue > 0)
            {
                    // PLC ��ȣ�� ������ ��� �ڽ� ������ ����
                    foreach (var box in boxesA)
                    {
                        box.StartMoving();
                    }
                    foreach (var box in boxesB)
                    {
                        box.StartMoving();
                    }
                }
            }
        }

        // ��ư Ŭ�� �� ȣ��� �޼���
        public void OnCreateBoxAButtonClick()
        {
            GameObject boxObj = Instantiate(boxAPrefab, spawnPointA.position, spawnPointA.rotation);
            Box boxScript = boxObj.AddComponent<Box>();
            boxScript.speed = speed;
            boxScript.direction = front;
            boxesA.Add(boxScript);
        }

        public void OnCreateBoxBButtonClick()
        {
            GameObject boxObj = Instantiate(boxBPrefab, spawnPointB.position, spawnPointB.rotation);
            Box boxScript = boxObj.AddComponent<Box>();
            boxScript.speed = speed;
            boxScript.direction = front;
            boxesB.Add(boxScript);
        }
    }


