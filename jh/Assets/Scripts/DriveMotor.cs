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
    public BoxManager boxManager;
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
    [Tooltip("이송 가능한 범위 내 현재 위치를 백분율로 나타냅니다. (%)")]
    public float transportRate;
    //[Tooltip("이송 가능한 범위 내 현재 목적지를 백분율로 나타냅니다. (%)")]
    //public float destinationRate;
    float transferTime;
    float elapsedTime;
    public float speed = 1;
    float location;
    Vector3 nowPos;
    Vector3 minPos;
    Vector3 maxPos;
    Vector3 destination;

    [Space(20)]
    [Header("Palletizing Setting")]
    public float boxAOddFloorDefaultLocation;
    public float boxAEvenFloorDefaultLocation;
    public float boxBOddFloorDefaultLocation;
    public float boxBEvenFloorDefaultLocation;

    void Start()
    {
        StartCoroutine(CoCountBoxQuantity());

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
    }

    private void Update()
    {
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
            }
        }
    }

    public IEnumerator CoCountBoxQuantity()
    {
        boxACount = transferManager.boxACount;
        boxBCount = transferManager.boxBCount;

        for (int i = boxACount; i <= (transferManager.boxAHorizontalQuantity * transferManager.boxAVerticalQuantity); i = i - (transferManager.boxAHorizontalQuantity * transferManager.boxAVerticalQuantity))
            boxACount = boxACount - (transferManager.boxAHorizontalQuantity * transferManager.boxAVerticalQuantity);

        for (int i = boxBCount; i <= (transferManager.boxBHorizontalQuantity * transferManager.boxBVerticalQuantity); i = i - (transferManager.boxBHorizontalQuantity * transferManager.boxBVerticalQuantity))
            boxBCount = boxBCount - (transferManager.boxBHorizontalQuantity * transferManager.boxBVerticalQuantity);

        yield return new WaitForSeconds(0.2f);
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
        print(this.gameObject.name + "MoveDrive");
    }

    public void SetToMove(float location)
    {
        nowPos = new Vector3(transfer.transform.localPosition.x, transfer.transform.localPosition.y, transfer.transform.localPosition.z);

        switch (direction)
        {
            case Direction.MoveXAxis:
                transportRate = (transfer.transform.localPosition.x - minRange) / (maxRange - minRange) * 100;
                destination = new Vector3(location, transfer.transform.localPosition.y, transfer.transform.localPosition.z);
                break;
            case Direction.MoveYAxis:
                transportRate = (transfer.transform.localPosition.y - minRange) / (maxRange - minRange) * 100;
                destination = new Vector3(transfer.transform.localPosition.x, location, transfer.transform.localPosition.z);
                break;
            case Direction.MoveZAxis:
                transportRate = (transfer.transform.localPosition.z - minRange) / (maxRange - minRange) * 100;
                destination = new Vector3(transfer.transform.localPosition.x, transfer.transform.localPosition.y, location);
                break;
        }
        print(this.gameObject.name + "SetToMove");
    }

    public IEnumerator CoTransfer()
    {
        print(this.gameObject.name + "CoTransfer");
        switch (direction)
        {
            case Direction.MoveXAxis:
                if (boxManager.isBoxADetected == true && boxManager.isBoxBDetected == false)
                {
                    if (transferManager.boxAFloor % 2 == 1)
                    {
                        SetToMove(boxAOddFloorDefaultLocation + (transferManager.boxAHorizontalDistance * (boxACount - 1)));
                    }
                    else if (transferManager.boxAFloor % 2 == 0 && transferManager.boxAFloor != 0)
                    {
                        SetToMove(boxAEvenFloorDefaultLocation + (transferManager.boxAVerticalDistance * (boxACount - 1)));
                    }

                }
                else if (boxManager.isBoxADetected == false && boxManager.isBoxBDetected == true)
                {
                    if (transferManager.boxBFloor % 2 == 1)
                    {
                        SetToMove(boxBOddFloorDefaultLocation + (transferManager.boxBHorizontalDistance * (boxBCount - 1)));
                    }
                    else if (transferManager.boxBFloor % 2 == 0 && transferManager.boxBFloor != 0)
                    {
                        SetToMove(boxBEvenFloorDefaultLocation + (transferManager.boxBVerticalDistance * (boxBCount - 1)));
                    }
                }
                break;
            case Direction.MoveZAxis:
                if (boxManager.isBoxADetected == true && boxManager.isBoxBDetected == false)
                {
                    SetToMove(minRange + ((transferManager.boxAFloor - 1) * transferManager.boxAHeight));
                }
                else if (boxManager.isBoxADetected == false && boxManager.isBoxBDetected == true)
                {
                    SetToMove(minRange + ((transferManager.boxBFloor - 1) * transferManager.boxBHeight));
                }
                break;
        }

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


        transferManager.isInSafeZone = false;
    }

    public IEnumerator CoTransferToSafeZone()
    {
        print(this.gameObject.name + "CoTransferToSafeZone");
        SetToMove(maxRange);

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

        transferManager.isInSafeZone = true;
        transferManager.positionStatus = TransferManager.Position.Safe;
    }

    public IEnumerator CoTransferToDefault()
    {
        SetToMove(minRange);

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

        transferManager.positionStatus = TransferManager.Position.Default;
        print("CoTransferToDefault()");
    }
}
