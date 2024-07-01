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
    float transferTime;
    float elapsedTime;
    public float speed = 1;
    float location;
    Vector3 nowPos;
    Vector3 minPos;
    Vector3 maxPos;
    Vector3 destination;
    public bool isSetted = true;

    [Space(20)]
    [Header("Palletizing Setting")]
    public float boxAOddFloorDefaultLocation;
    public float boxAEvenFloorDefaultLocation;
    public float boxBOddFloorDefaultLocation;
    public float boxBEvenFloorDefaultLocation;

    void Start()
    {
        CoCountBoxQuantity();

        plcInputValues = new int[2];

        transferTime = maxRange - minRange;

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

        if (direction == Direction.MoveXAxis)
        {
            Vector3 sefeLocation = new Vector3(maxRange, transfer.transform.localPosition.y, transfer.transform.localPosition.z);
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

        if (transferManager.isBoxADetected == true && isSetted == false)
        {
            SetToMoveA();
        }
        else if (transferManager.isBoxBDetected == true && isSetted == false)
        {
            SetToMoveA();
        }
    }

    public void CoCountBoxQuantity()
    {
        boxACount = transferManager.boxACount;
        boxBCount = transferManager.boxBCount;

        for (int i = boxACount; i <= (transferManager.boxAHorizontalQuantity * transferManager.boxAVerticalQuantity); i = i - (transferManager.boxAHorizontalQuantity * transferManager.boxAVerticalQuantity))
            boxACount = boxACount - (transferManager.boxAHorizontalQuantity * transferManager.boxAVerticalQuantity);

        for (int i = boxBCount; i <= (transferManager.boxBHorizontalQuantity * transferManager.boxBVerticalQuantity); i = i - (transferManager.boxBHorizontalQuantity * transferManager.boxBVerticalQuantity))
            boxBCount = boxBCount - (transferManager.boxBHorizontalQuantity * transferManager.boxBVerticalQuantity);
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
                    destination = new Vector3(transfer.transform.localPosition.x, location, transfer.transform.localPosition.z);
                }
                else if (transferManager.boxAFloor % 2 == 0 && transferManager.boxAFloor != 0)
                {
                    location = (int)((boxACount - 1) / transferManager.boxAHorizontalQuantity) * transferManager.boxAHorizontalDistance + boxAEvenFloorDefaultLocation;
                    destination = new Vector3(location, transfer.transform.localPosition.y, transfer.transform.localPosition.z);
                }
                break;
        }
        isSetted = true;
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
                    destination = new Vector3(transfer.transform.localPosition.x, location, transfer.transform.localPosition.z);
                }
                else if (transferManager.boxBFloor % 2 == 0 && transferManager.boxBFloor != 0)
                {
                    location = (int)((boxBCount - 1) / transferManager.boxBHorizontalQuantity) * transferManager.boxBHorizontalDistance + boxBEvenFloorDefaultLocation;
                    destination = new Vector3(location, transfer.transform.localPosition.y, transfer.transform.localPosition.z);
                }
                break;
        }
        isSetted = true;
    }

    public void MoveDrive(Vector3 startPos, Vector3 endPos)
    {
        Vector3 newPos = Vector3.Lerp(startPos, endPos, speed * Time.deltaTime);
        transfer.transform.localPosition = newPos;
    }

    public IEnumerator CoTransfer()
    {
        if (!isDriveReversed)
        {
            MoveDrive(transfer.transform.localPosition, maxPos);
            mxComponent.SetDevice(deviceName, 1);
            if(transferManager.positionStatus == TransferManager.Position.Default && transfer.transform.localPosition == maxPos && direction == Direction.MoveZAxis)
            {
                StopCoroutine(CoTransfer());
                transferManager.positionStatus = TransferManager.Position.Safe;
            }
            else if (transfer.transform.localPosition == destination)
            {
                StopCoroutine(CoTransfer());
            }
        }
        else
        {
            MoveDrive(transfer.transform.localPosition, minPos);
            mxComponent.SetDevice(deviceNameReversed, 1);
            if (transferManager.positionStatus == TransferManager.Position.Safe && transfer.transform.localPosition == minPos && direction == Direction.MoveZAxis)
            {
                StopCoroutine(CoTransfer());
                transferManager.positionStatus = TransferManager.Position.Default;
            }
            else if (transfer.transform.localPosition == destination)
            {
                StopCoroutine(CoTransfer());
            }
        }

        yield return new WaitForSeconds(0.2f);

        if (transferManager.positionStatus != TransferManager.Position.Safe && transferManager.positionStatus != TransferManager.Position.Default)
        {
            switch (direction)
            {
                case Direction.MoveXAxis:
                    transferManager.positionStatus = TransferManager.Position.XMoved;
                    break;
                case Direction.MoveYAxis:
                    transferManager.positionStatus = TransferManager.Position.YMoved;
                    break;
                case Direction.MoveZAxis:
                    transferManager.positionStatus = TransferManager.Position.ZMoved;
                    break;
            }
        }
    }
}
