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
        [Header("박스 A 생성")]
        public GameObject boxAPrefab;
        public Transform spawnPointA; // 박스 A를 생성할 위치

        [Header("박스 B 생성")]
        public GameObject boxBPrefab;
        public Transform spawnPointB; // 박스 B를 생성할 위치

        [Space(20)]
        [Header("현재 상태")]
        public string switchDeviceName; // plc 연동되는 부분
        public int plcInputValue;
        public float speed = 1.0f;
        public Vector3 front = new Vector3(1, 0, 0);

        private List<Box> boxesA = new List<Box>(); // 생성된 박스 A를 관리할 리스트
        private List<Box> boxesB = new List<Box>(); // 생성된 박스 B를 관리할 리스트

        void Update()
        {
            if (MxComponent.instance.connection == MxComponent.Connection.Connected)
            {
                if (plcInputValue == 0)
                {
                    // PLC 신호가 없으면 모든 박스 멈춤
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
                    // PLC 신호가 있으면 모든 박스 움직임 시작
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

        // 버튼 클릭 시 호출될 메서드
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


