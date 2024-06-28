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
        [Header("박스 생성")]
        public GameObject boxAPrefab;
        public Transform spawnPoint; // 박스를 생성할 위치
        public GameObject boxBPrefab;

    [Space(20)]
        [Header("박스 사이즈")]
        public Sensor Align_Sensor;
        public Sensor BoxA_Sensor;
        public Sensor BoxB_Sensor;

    [Space(20)]
        [Header("현재 상태")]
        public string switchDeviceName; // plc 연동되는 부분
        public int plcInputValue = 0;
        public float speed = 1.0f;
        public Vector3 front = new Vector3(1, 0, 0);

        public List<Box1> boxes = new List<Box1>(); // 생성된 박스를 관리할 리스트

        private bool resultPrinted = false;
        public bool isBoxADetected;
        public bool isBoxBDetected;

    void Update()
        {
            if (MxComponent.instance.connection == MxComponent.Connection.Connected)
            {
                if (plcInputValue == 0)
                {
                    // PLC 신호가 없으면 모든 박스 멈춤
                    foreach (var box in boxes)
                    {
                        box.StopMoving();
                    }
                }
                else if (plcInputValue > 0)
            {
                    // PLC 신호가 있으면 모든 박스 움직임 시작
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

        // 버튼 클릭 시 호출될 메서드
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


