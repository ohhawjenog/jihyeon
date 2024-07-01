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
    public Transform spawnPoint; // 박스를 생성할 위치
    public GameObject boxAPrefab;
    public GameObject boxBPrefab;

    public TransferManager transferManager;
    public Box box = null;
    public GameObject boxObj = null;
    public GameObject boxA;
    public GameObject boxB;
    public Sensor loadDetector;

    [Space(20)]
    [Header("박스 사이즈")]
    public Sensor alignSensor;
    public Sensor boxASensor;
    public Sensor boxBSensor;

    [Space(20)]
    [Header("현재 상태")]
    public int plcInputValue = 0;
    public float speed = 1.0f;
    Vector3 direction = new Vector3(1, 0, 0);
    public bool isMoving = false;
    public bool isStoppedBySensor = false; // 센서에 의해 멈춘 상태를 추적

    public bool resultPrinted = false;
    public bool isBoxADetected;
    public bool isBoxBDetected;

    void Update()
    {
        if (MxComponent.instance.connection == MxComponent.Connection.Connected)
        {
            if (plcInputValue == 0)
            {
                StopMoving();
            }
            else if (plcInputValue > 0)
        {
                StartMoving();
            }
        }

        if (alignSensor.isObjectDetected == true && resultPrinted == false)
        {
            if (boxASensor.isSizeDetected == true && boxBSensor.isSizeDetected == false)
            {
                print("Box A");
                isBoxADetected = true;
                isBoxBDetected = false;
                resultPrinted = true;
            }
            else if (boxASensor.isSizeDetected == false && boxBSensor.isSizeDetected == true)
            {
                print("Box B");
                isBoxADetected = false;
                isBoxBDetected = true;
                resultPrinted = true;
            }
        }

        if (box != null)
        {
            if (box.isSensorCollider == true)
            {
                StopMoving();
                isStoppedBySensor = true;
            }

            if (box.isLoaderCollider == true)
            {
                StopMoving();
                isStoppedBySensor = true;
                resultPrinted = false;
            }

            if (transferManager.moved == TransferManager.Moved.BoxLoaded && transferManager.status == TransferManager.Status.BoxLoaded && box.isSensorCollider == false && box.isLoaderCollider == false)
            {
                if (isBoxADetected)
                {
                    boxObj.transform.parent = boxA.gameObject.transform;
                }
                else if (isBoxBDetected)
                {
                    boxObj.transform.parent = boxB.gameObject.transform;
                }
            }
        }
    }

    // 버튼 클릭 시 호출될 메서드
    public void OnCreateBoxButtonClick()
    {

        int temp = Random.Range(0, 2);
        if (temp == 0)
        {
            boxObj = Instantiate(boxAPrefab, spawnPoint.position, spawnPoint.rotation);

        }
        else if (temp == 1)
        {
            boxObj = Instantiate(boxBPrefab, spawnPoint.position, spawnPoint.rotation);
        }

        box = boxObj.GetComponent<Box>();
        boxObj.transform.parent = this.gameObject.transform;
    }

    public void StartMoving()
    {
        if (!isStoppedBySensor)
        {
            isMoving = true;
            StartCoroutine(CoMove());
        }
    }

    public void StopMoving()
    {
        isMoving = false;
        StopCoroutine(CoMove());
    }

    IEnumerator CoMove()
    {
        while (isMoving)
        {
            transform.GetChild(1).transform.position += direction * Time.deltaTime * speed;
            yield return null;
        }
    }
}


