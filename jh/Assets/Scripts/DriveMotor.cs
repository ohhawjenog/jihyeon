using MPS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DriveMotor : MonoBehaviour
{
    public enum Direction
    {
        MoveLocalX = 1,
        MoveLocalY = 2,
        MoveLocalZ = 3
    }

    public enum Position
    {
        Default = 0,
        Safe = 1,
        XMoved = 2,
        YMoved = 3,
        ZMoved = 4
    }

    public enum Detection
    {
        BoxA = 1,
        BoxB = 2
    }

    [Header("Device Info")]
    public Direction direction;
    public Detection detection;
    public int[] plcInputValues;
    public int plcInputBoxAQuantity;
    public int plcInputBoxBQuantity;
    public bool isDriveMoving;
    public bool isDriveReverse;
    public int boxACount;
    public int boxAFloor;
    public int boxBCount;
    public int boxBFloor;
    public MxComponent mxComponent;
    public Box boxDetector;

    [Space(20)]
    [Header("Transfer Position")]
    public Transform transfer;
    [Tooltip("이송 가능한 최대 위치입니다.")]
    public float minRange;
    [Tooltip("이송 가능한 최소 위치입니다.")]
    public float maxRange;
    [Tooltip("이송 가능한 범위 내 현재 위치를 백분율로 나타냅니다. (%)")]
    public float transportRate;
    [Tooltip("이송 가능한 범위 내 현재 목적지를 백분율로 나타냅니다. (%)")]
    public float destinationRate;
    [Tooltip("최소 위치에서 최대 위치까지 이송에 소요되는 시간입니다.")]
    public float transferTime;
    float elapsedTime;
    public float speed = 1;
    Vector3 nowPos;
    Vector3 defPosA;
    Vector3 defPosB;
    Vector3 minPos;
    Vector3 maxPos;

    [Space(20)]
    [Header("Palletizing Setting")]
    //public Transform boxADefault;
    public float boxAHorizontalDistance;
    public float boxAHorizontalQuantity;
    public float boxAVerticalDistance;
    public float boxAVerticalQuantity;
    public float boxAHeight;
    //public Transform boxBDefault;
    public float boxBHorizontalDistance;
    public float boxBHorizontalQuantity;
    public float boxBVerticalDistance;
    public float boxBVerticaQuantity;
    public float boxBHeight;

    void Start()
    {
        //boxACount = boxDetector.boxACount;
        //boxBCount = boxDetector.boxBCount;

        transferTime = maxRange - minRange;

        //switch (direction)
        //{
        //    case Direction.MoveLocalX:
        //        minPos = new Vector3(minRange, transfer.transform.localPosition.y, transfer.transform.localPosition.z);
        //        maxPos = new Vector3(maxRange, transfer.transform.localPosition.y, transfer.transform.localPosition.z);
        //        defPosA = new Vector3(boxADefault.transform.localPosition.x, transfer.transform.localPosition.y, transfer.transform.localPosition.z);
        //        defPosB = new Vector3(boxBDefault.transform.localPosition.x, transfer.transform.localPosition.y, transfer.transform.localPosition.z);
        //        break;
        //    case Direction.MoveLocalY:
        //        minPos = new Vector3(transfer.transform.localPosition.x, minRange, transfer.transform.localPosition.z);
        //        maxPos = new Vector3(transfer.transform.localPosition.x, maxRange, transfer.transform.localPosition.z);
        //        defPosA = new Vector3(transfer.transform.localPosition.x, boxADefault.transform.localPosition.y, transfer.transform.localPosition.z);
        //        defPosB = new Vector3(transfer.transform.localPosition.x, boxBDefault.transform.localPosition.y, transfer.transform.localPosition.z);
        //        break;
        //    case Direction.MoveLocalZ:
        //        minPos = new Vector3(transfer.transform.localPosition.x, transfer.transform.localPosition.y, minRange);
        //        maxPos = new Vector3(transfer.transform.localPosition.x, transfer.transform.localPosition.y, maxRange);
        //        defPosA = new Vector3(transfer.transform.localPosition.x, transfer.transform.localPosition.y, boxADefault.transform.localPosition.z);
        //        defPosB = new Vector3(transfer.transform.localPosition.x, transfer.transform.localPosition.y, boxBDefault.transform.localPosition.z);
        //        break;
        //}

        plcInputValues = new int[2];

        //SetToMove();
    }

    private void Update()
    {
        if (MxComponent.instance.connection == MxComponent.Connection.Connected)
        {
            if (plcInputValues[0] > 0)
            {
                isDriveReverse = false;
                StartCoroutine(CoTransfer());
            }

            if (plcInputValues[1] > 0)
            {
                isDriveReverse = true;
                StartCoroutine(CoTransfer());
            }

            if (plcInputValues[0] == 0 && plcInputValues[1] == 0)
            {
                StopCoroutine(CoTransfer());
                isDriveMoving = false;
            }
        }

        if (boxACount > boxAHorizontalQuantity * boxAHorizontalQuantity)
        {
            boxACount = 0;
            boxAFloor++;
        }
        if (boxBCount > boxBHorizontalQuantity * boxBHorizontalQuantity)
        {
            boxBCount = 0;
            boxAFloor++;
        }
    }

    public void MoveDrive(Vector3 startPos, Vector3 endPos, float _elapsedTime, float _runTime)
    {
        Vector3 newPos = Vector3.Lerp(startPos, endPos, _elapsedTime / _runTime);
        transfer.transform.localPosition = newPos;
    }

    public void SetToMove(Detection detection, int boxCount)
    {
        nowPos = new Vector3(transfer.transform.localPosition.x, transfer.transform.localPosition.y, transfer.transform.localPosition.z);

        switch (direction)
        {
            case Direction.MoveLocalX:
                transportRate = (transfer.transform.localPosition.x - minRange) / (maxRange - minRange) * 100;
                break;
            case Direction.MoveLocalY:
                transportRate = (transfer.transform.localPosition.y - minRange) / (maxRange - minRange) * 100;
                break;
            case Direction.MoveLocalZ:
                transportRate = (transfer.transform.localPosition.z - minRange) / (maxRange - minRange) * 100;
                break;
        }

        if (detection == Detection.BoxA)
        {
            switch (direction)
            {
                case Direction.MoveLocalX:
                    transportRate = (transfer.transform.localPosition.x - minRange) / (maxRange - minRange) * 100;
                    break;
                case Direction.MoveLocalY:
                    transportRate = (transfer.transform.localPosition.y - minRange) / (maxRange - minRange) * 100;
                    break;
                case Direction.MoveLocalZ:
                    transportRate = (transfer.transform.localPosition.z - minRange) / (maxRange - minRange) * 100;
                    break;
            }
        }
        else if (detection == Detection.BoxB)
        {
            switch (direction)
            {
                case Direction.MoveLocalX:
                    transportRate = (transfer.transform.localPosition.x - minRange) / (maxRange - minRange) * 100;
                    break;
                case Direction.MoveLocalY:
                    transportRate = (transfer.transform.localPosition.y - minRange) / (maxRange - minRange) * 100;
                    break;
                case Direction.MoveLocalZ:
                    transportRate = (transfer.transform.localPosition.z - minRange) / (maxRange - minRange) * 100;
                    break;
            }
        }
    }

    IEnumerator CoTransfer()
    {
        //if (boxDetector.boxADetected == true && boxBDetected == false)
        //{
        //    SetToMove(Detection.boxA, boxACount - 1);
        //}
        //else if (boxDetector.boxADetected == false && boxBDetected == true)
        //{
        //    SetToMove(Detection.boxB, boxBCount - 1);
        //}

        //SetToMove();
        isDriveMoving = true;

        elapsedTime = 0;

        while (plcInputValues[0] > 0 || plcInputValues[1] > 0)
        {
            elapsedTime += Time.deltaTime;

            if (isDriveReverse)
            {
                MoveDrive(nowPos, minPos, elapsedTime, transferTime * transportRate * speed);
            }
            else
            {
                MoveDrive(nowPos, maxPos, elapsedTime, transferTime * (100 - transportRate) * speed);
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }

        isDriveMoving = false;
    }
}
