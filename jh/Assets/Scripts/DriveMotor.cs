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

    [Header("Device Info")]
    public Direction direction;
    public string deviceName;
    public string deviceNameReversed;
    public int[] plcInputValues;
    public int plcInputBoxAQuantity;
    public int plcInputBoxBQuantity;
    public bool isDriveMoving;
    public bool isDriveReversed;
    public bool isDriveArrived;
    public bool isInSafeZone;
    public int boxACount;
    public int boxAFloor = 1;
    public int boxBCount;
    public int boxBFloor = 1;
    public MxComponent mxComponent;
    public BoxManager boxManager;
    public TransferManager transferManager;
    public Sensor loadingDetector;

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
    float location;
    public Vector3 nowPos;
    public Vector3 minPos;
    public Vector3 maxPos;
    public Vector3 destination;

    [Space(20)]
    [Header("Palletizing Setting")]
    public float boxAOddFloorDefaultLocation;
    public float boxAEvenFloorDefaultLocation;
    public float boxAHorizontalDistance;
    public float boxAHorizontalQuantity;
    public float boxAVerticalDistance;
    public float boxAVerticalQuantity;
    public float boxAHeight;
    public float boxBOddFloorDefaultLocation;
    public float boxBEvenFloorDefaultLocation;
    public float boxBHorizontalDistance;
    public float boxBHorizontalQuantity;
    public float boxBVerticalDistance;
    public float boxBVerticaQuantity;
    public float boxBHeight;

    void Start()
    {
        plcInputValues = new int[2];

        isInSafeZone = false;

        transferTime = maxRange - minRange;

        switch (direction)
        {
            case Direction.MoveLocalX:
                minPos = new Vector3(minRange, transfer.transform.localPosition.y, transfer.transform.localPosition.z);
                maxPos = new Vector3(maxRange, transfer.transform.localPosition.y, transfer.transform.localPosition.z);
                break;
            case Direction.MoveLocalY:
                minPos = new Vector3(transfer.transform.localPosition.x, minRange, transfer.transform.localPosition.z);
                maxPos = new Vector3(transfer.transform.localPosition.x, maxRange, transfer.transform.localPosition.z);
                break;
            case Direction.MoveLocalZ:
                minPos = new Vector3(transfer.transform.localPosition.x, transfer.transform.localPosition.y, minRange);
                maxPos = new Vector3(transfer.transform.localPosition.x, transfer.transform.localPosition.y, maxRange);
                break;
        }
    }

    private void Update()
    {
        boxACount = mxComponent.boxACount;
        boxBCount = mxComponent.boxBCount;

        if (MxComponent.instance.connection == MxComponent.Connection.Connected)
        {
            if (plcInputValues[0] > 0)
            {
                isDriveReversed = false;
                StartCoroutine(CoTransfer());
            }

            if (plcInputValues[1] > 0)
            {
                isDriveReversed = true;
                StartCoroutine(CoTransfer());
            }

            if (plcInputValues[0] == 0 && plcInputValues[1] == 0)
            {
                StopCoroutine(CoTransfer());
                isDriveMoving = false;
            }

            if (loadingDetector.isObjectDetected == true)
            {
                StartCoroutine(CoTransfer());
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
            boxBFloor++;
        }

        if (loadingDetector.isObjectDetected == true && direction == Direction.MoveLocalZ && isInSafeZone == false)
        {
            StartCoroutine(CoTransferToSafeZone());
        }
    }

    public void MoveDrive(Vector3 startPos, Vector3 endPos, float _elapsedTime, float _runTime)
    {
        Vector3 newPos = Vector3.Lerp(startPos, endPos, _elapsedTime / _runTime);
        transfer.transform.localPosition = newPos;

        if (isDriveReversed == false)
        {
            mxComponent.SetDevice(deviceName, 1);
        }
        else
        {
            mxComponent.SetDevice(deviceNameReversed, 1);
        }
    }

    public void SetToMove(float location)
    {
        nowPos = new Vector3(transfer.transform.localPosition.x, transfer.transform.localPosition.y, transfer.transform.localPosition.z);

        switch (direction)
        {
            case Direction.MoveLocalX:
                transportRate = (transfer.transform.localPosition.x - minRange) / (maxRange - minRange) * 100;
                destination = new Vector3(location, transfer.transform.localPosition.y, transfer.transform.localPosition.z);
                break;
            case Direction.MoveLocalY:
                transportRate = (transfer.transform.localPosition.y - minRange) / (maxRange - minRange) * 100;
                destination = new Vector3(transfer.transform.localPosition.x, location, transfer.transform.localPosition.z);
                break;
            case Direction.MoveLocalZ:
                transportRate = (transfer.transform.localPosition.z - minRange) / (maxRange - minRange) * 100;
                destination = new Vector3(transfer.transform.localPosition.x, transfer.transform.localPosition.y, location);
                break;
        }
    }

    IEnumerator CoTransfer()
    {
        //switch (direction)
        //{
        //    case Direction.MoveLocalX:
        //        if (boxManager.boxADetected == true && boxBDetected == false)
        //        {
        //            if (boxAFloor % 2 == 1)
        //            {
        //                SetToMove(boxAOddFloorDefaultLocation + (boxAHorizontalDistance * (boxACount - 1)));
        //            }
        //            else if (boxAFloor % 2 == 0 && boxAFloor != 0)
        //            {
        //                SetToMove(boxAEvenFloorDefaultLocation + (boxAVerticalDistance * (boxACount - 1)));
        //            }

        //        }
        //        else if (boxManager.boxADetected == false && boxBDetected == true)
        //        {
        //            if (boxBFloor % 2 == 1)
        //            {
        //                SetToMove(boxBOddFloorDefaultLocation + (boxBHorizontalDistance * (boxBCount - 1)));
        //            }
        //            else if (boxBFloor % 2 == 0 && boxBFloor != 0)
        //            {
        //                SetToMove(boxBEvenFloorDefaultLocation + (boxBVerticalDistance * (boxBCount - 1)));
        //            }
        //        }
        //        break;
        //    case Direction.MoveLocalZ:
        //        if (boxManager.boxADetected == true && boxBDetected == false)
        //        {
        //            if (boxAFloor % 2 == 1)
        //            {
        //                SetToMove(boxAOddFloorDefaultLocation + (boxAVerticalDistance * (boxACount - 1)));
        //            }
        //            else if (boxAFloor % 2 == 0 && boxAFloor != 0)
        //            {
        //                SetToMove(boxAEvenFloorDefaultLocation + (boxAHorizontalDistance * (boxACount - 1)));
        //            }

        //        }
        //        else if (boxManager.boxADetected == false && boxBDetected == true)
        //        {
        //            if (boxBFloor % 2 == 1)
        //            {
        //                SetToMove(boxBOddFloorDefaultLocation + (boxBVerticalDistance * (boxBCount - 1)));
        //            }
        //            else if (boxBFloor % 2 == 0 && boxBFloor != 0)
        //            {
        //                SetToMove(boxBEvenFloorDefaultLocation + (boxBHorizontalDistance * (boxBCount - 1)));
        //            }
        //        }
        //        break;
        //}

        isDriveMoving = true;

        elapsedTime = 0;

        while (plcInputValues[0] > 0 || plcInputValues[1] > 0 || destination != new Vector3(transfer.transform.localPosition.x, transfer.transform.localPosition.y, transfer.transform.localPosition.z))
        {
            elapsedTime += Time.deltaTime;

            if (isDriveReversed)
            {
                MoveDrive(nowPos, minPos, elapsedTime, transferTime * transportRate * speed);
            }
            else
            {
                MoveDrive(nowPos, maxPos, elapsedTime, transferTime * (100 - transportRate) * speed);
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }

        if (isDriveReversed == false)
        {
            mxComponent.SetDevice(deviceName, 0);
        }
        else
        {
            mxComponent.SetDevice(deviceNameReversed, 0);
        }

        isDriveMoving = false;

        switch (direction)
        {
            case Direction.MoveLocalX:
                transferManager.positionStatus = TransferManager.Position.XMoved;
                break;
            case Direction.MoveLocalY:
                transferManager.positionStatus = TransferManager.Position.YMoved;
                break;
            case Direction.MoveLocalZ:
                transferManager.positionStatus = TransferManager.Position.ZMoved;
                break;
        }
    }

    IEnumerator CoTransferToSafeZone()
    {
        SetToMove(maxRange);

        isDriveMoving = true;

        elapsedTime = 0;

        while (plcInputValues[0] > 0 || plcInputValues[1] > 0 || destination != new Vector3(transfer.transform.localPosition.x, transfer.transform.localPosition.y, transfer.transform.localPosition.z))
        {
            elapsedTime += Time.deltaTime;

            MoveDrive(nowPos, maxPos, elapsedTime, transferTime * (100 - transportRate) * speed);

            yield return new WaitForSeconds(Time.deltaTime);
        }

        if (isDriveReversed == false)
        {
            mxComponent.SetDevice(deviceName, 0);
        }
        else
        {
            mxComponent.SetDevice(deviceNameReversed, 0);
        }

        isDriveMoving = false;
        isInSafeZone = true;
        transferManager.positionStatus = TransferManager.Position.Safe;
    }
}
