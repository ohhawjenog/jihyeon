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
        MoveXAxis = 1,
        MoveYAxis = 2,
        MoveZAxis = 3
    }

    [Header("Device Info")]
    public Direction direction;
    public string deviceName;
    public string deviceNameReversed;
    public int[] plcInputValues;
    //public int plcInputBoxAQuantity;
    //public int plcInputBoxBQuantity;
    public bool isDriveReversed;
    public bool isDriveArrived;
    public MxComponent mxComponent;
    public TransferManager transferManager;
    public Sensor loadingDetector;
    int boxACount;
    int boxBCount;

    [Space(20)]
    [Header("Transfer Position")]
    public Transform transfer;
    [Tooltip("이송 가능한 최대 위치입니다.")]
    public float minRange;
    [Tooltip("이송 가능한 최소 위치입니다.")]
    public float maxRange;
    float elapsedTime;
    public float speed = 1;
    public float location;
    Vector3 minPos;
    Vector3 maxPos;
    Vector3 destination;
    public bool isDriverMoving;

    [Space(20)]
    [Header("Palletizing Setting")]
    public float boxAOddFloorDefaultLocation;
    public float boxAEvenFloorDefaultLocation;
    public float boxBOddFloorDefaultLocation;
    public float boxBEvenFloorDefaultLocation;

    void Start()
    {
        CountBoxQuantity();

        plcInputValues = new int[2];

        switch (direction)
        {
            case Direction.MoveXAxis:
                minPos = new Vector3(minRange, transfer.transform.localPosition.y, transfer.transform.localPosition.z);
                maxPos = new Vector3(maxRange, transfer.transform.localPosition.y, transfer.transform.localPosition.z);
                break;
            case Direction.MoveYAxis:
                minPos = new Vector3(transfer.transform.localPosition.x, minRange, transfer.transform.localPosition.z);
                maxPos = new Vector3(transfer.transform.localPosition.x, maxRange, transfer.transform.localPosition.z);
                break;
            case Direction.MoveZAxis:
                minPos = new Vector3(transfer.transform.localPosition.x, transfer.transform.localPosition.y, minRange);
                maxPos = new Vector3(transfer.transform.localPosition.x, transfer.transform.localPosition.y, maxRange);
                break;
        }
    }

    private void Update()
    {
        //if (MxComponent.instance.connection == MxComponent.Connection.Connected)
        //{
        //    if (plcInputValues[0] > 0)
        //    {
        //        isDriveReversed = false;
        //        StartCoroutine(CoTransfer());
        //    }

        //    if (plcInputValues[1] > 0)
        //    {
        //        isDriveReversed = true;
        //        StartCoroutine(CoTransfer());
        //    }

        //    if (plcInputValues[0] == 0 && plcInputValues[1] == 0)
        //    {
        //        StopCoroutine(CoTransfer());
        //    }
        //}

        switch (direction)
        {
            case Direction.MoveXAxis:
                if (transfer.transform.localPosition.x >= maxRange)
                {
                    transferManager.moved = TransferManager.Moved.XMoved;

                    if (transferManager.status == TransferManager.Status.Default)
                        transferManager.status = TransferManager.Status.Safe;
                }
                if (transfer.transform.localPosition.x <= location && transferManager.isRotated == true && transferManager.isXMoving == true)
                {
                    transferManager.moved = TransferManager.Moved.XMoved;
                }
                break;

            case Direction.MoveYAxis:
                if (transfer.transform.localPosition.y <= location && transferManager.isZMoving == true)
                {
                    transferManager.moved = TransferManager.Moved.ZMoved;
                }
                if (transform.position == Vector3.zero && transferManager.status != TransferManager.Status.Default
                        && transferManager.status != TransferManager.Status.Safe && transferManager.isZMoving == true)
                    transferManager.moved = TransferManager.Moved.ZMoved;
                break;

            case Direction.MoveZAxis:
                if (transfer.transform.localPosition.z <= location && transferManager.isRotated == true && transferManager.isYMoving == true)
                {
                    if (transferManager.status == TransferManager.Status.Safe)
                        transferManager.status = TransferManager.Status.Transfer;
                    transferManager.moved = TransferManager.Moved.YMoved;
                }
                if (transform.position == Vector3.zero && transferManager.isLoaded == true)
                    transferManager.moved = TransferManager.Moved.YMoved;
                break;
        }
    }

    public void CountBoxQuantity()
    {
        boxACount = transferManager.boxACount;
        boxBCount = transferManager.boxBCount;

        if (boxACount > transferManager.boxAHorizontalQuantity * transferManager.boxAVerticalQuantity)
        {
            boxACount = boxACount % (transferManager.boxAHorizontalQuantity * transferManager.boxAVerticalQuantity);
        }

        if (boxBCount > transferManager.boxBHorizontalQuantity * transferManager.boxBVerticalQuantity)
        {
            boxBCount = boxBCount % (transferManager.boxBHorizontalQuantity * transferManager.boxBVerticalQuantity);
        }
    }

    public void SetToMoveA()
    {
        switch (direction)
        {
            case Direction.MoveXAxis:
                if (transferManager.boxAFloor % 2 == 1)
                {
                    location = ((boxACount - 1) % transferManager.boxAHorizontalQuantity) * transferManager.boxAHorizontalDistance + boxAOddFloorDefaultLocation;
                    destination = new Vector3(location, transfer.transform.localPosition.y, transfer.transform.localPosition.z);
                }
                else if (transferManager.boxAFloor % 2 == 0 && transferManager.boxAFloor != 0)
                {
                    location = ((boxACount - 1) % transferManager.boxAVerticalQuantity) * transferManager.boxAVerticalDistance + boxAEvenFloorDefaultLocation;
                    destination = new Vector3(location, transfer.transform.localPosition.y, transfer.transform.localPosition.z);
                }
                break;
            case Direction.MoveYAxis:
                location = (transferManager.boxAFloor - 1) * transferManager.boxAVerticalDistance + boxAOddFloorDefaultLocation;
                break;
            case Direction.MoveZAxis:
                if (transferManager.boxAFloor % 2 == 1)
                {
                    location = (int)((boxACount - 1) / transferManager.boxAVerticalQuantity) * transferManager.boxAVerticalDistance + boxAOddFloorDefaultLocation;
                    destination = new Vector3(transfer.transform.localPosition.x, transfer.transform.localPosition.y, location);
                }
                else if (transferManager.boxAFloor % 2 == 0 && transferManager.boxAFloor != 0)
                {
                    location = (int)((boxACount - 1) / transferManager.boxAHorizontalQuantity) * transferManager.boxAHorizontalDistance + boxAEvenFloorDefaultLocation;
                    destination = new Vector3(transfer.transform.localPosition.x, transfer.transform.localPosition.y, location);
                }
                break;
        }

        print($"SetA: {direction}: {destination}");
    }

    public void SetToMoveB()
    {
        switch (direction)
        {
            case Direction.MoveXAxis:
                if (transferManager.boxBFloor % 2 == 1)
                {
                    location = ((boxBCount - 1) % transferManager.boxBHorizontalQuantity) * transferManager.boxBHorizontalDistance + boxBOddFloorDefaultLocation;
                    destination = new Vector3(location, transfer.transform.localPosition.y, transfer.transform.localPosition.z);
                }
                else if (transferManager.boxBFloor % 2 == 0 && transferManager.boxBFloor != 0)
                {
                    location = ((boxBCount - 1) % transferManager.boxBVerticalQuantity) * transferManager.boxBVerticalDistance + boxBEvenFloorDefaultLocation;
                    destination = new Vector3(location, transfer.transform.localPosition.y, transfer.transform.localPosition.z);
                }
                break;
            case Direction.MoveYAxis:
                location = (transferManager.boxBFloor - 1) * transferManager.boxBVerticalDistance + boxBOddFloorDefaultLocation;
                break;
            case Direction.MoveZAxis:
                if (transferManager.boxBFloor % 2 == 1)
                {
                    location = (int)((boxBCount - 1) / transferManager.boxBVerticalQuantity) * transferManager.boxBVerticalDistance + boxBOddFloorDefaultLocation;
                    destination = new Vector3(transfer.transform.localPosition.x, transfer.transform.localPosition.y, location);
                }
                else if (transferManager.boxBFloor % 2 == 0 && transferManager.boxBFloor != 0)
                {
                    location = (int)((boxBCount - 1) / transferManager.boxBHorizontalQuantity) * transferManager.boxBHorizontalDistance + boxBEvenFloorDefaultLocation;
                    destination = new Vector3(transfer.transform.localPosition.x, transfer.transform.localPosition.y, location);
                }
                break;
        }

        print($"SetB: {direction}: {destination}");
    }

    public IEnumerator CoMove(Vector3 startPos, Vector3 endPos)
    {
        isDriverMoving = true;

        if (direction == Direction.MoveXAxis && transferManager.status != TransferManager.Status.Transfer && transferManager.status != TransferManager.Status.Safe)
        {
            destination = maxPos;
        }

        while (transfer.transform.localPosition != destination)
        {
            print($"{direction} : CoMove {destination}");
            Vector3 newPos = Vector3.MoveTowards(startPos, endPos, speed * 0.2f);
            transfer.transform.localPosition = newPos;

            yield return new WaitForSeconds(0.2f);
        }

        if (transfer.transform.localPosition == destination)
        {
            switch (direction)
            {
                case Direction.MoveXAxis:
                    if (transferManager.status == TransferManager.Status.Default)
                        transferManager.status = TransferManager.Status.Safe;
                    transferManager.moved = TransferManager.Moved.XMoved;
                    break;
                case Direction.MoveYAxis:
                    if (transferManager.status == TransferManager.Status.Safe)
                        transferManager.status = TransferManager.Status.Transfer;
                    transferManager.moved = TransferManager.Moved.YMoved;
                    break;
                case Direction.MoveZAxis:
                    transferManager.moved = TransferManager.Moved.ZMoved;
                    break;
            }

            isDriveReversed = !isDriveReversed;
        }

        print($"{direction} : CoMove 끝 {destination}");
    }

    public void Transfer()
    {
        if (isDriveReversed == false)
        {
            StartCoroutine(CoMove(transfer.position, maxPos));
            mxComponent.SetDevice(deviceName, 1);
        }
        else
        {
            StartCoroutine(CoMove(transfer.position, minPos));
            mxComponent.SetDevice(deviceNameReversed, 1);
        }
    }

    public IEnumerator CoMoveToDefault()
    {
        while (transfer.transform.localPosition != Vector3.zero)
        {
            Vector3 newPos = Vector3.MoveTowards(maxPos, Vector3.zero, speed * 0.2f);
            transfer.transform.localPosition = newPos;

            yield return new WaitForSeconds(0.2f);
        }
    }
}
