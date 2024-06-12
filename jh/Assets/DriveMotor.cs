using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveMotor : MonoBehaviour
{
    public enum Direction
    {
        MoveXAxis = 0,
        MoveYAxis = 1,
        MoveZAxis = 2
    }

    [Header("Device Info")]
    public Direction direction;
    public string plusMotorDeviceName;
    public string minusMotorDeviceName;
    public int motorStatus;

    [Space(20)]
    [Header("Transfer Position")]
    public Transform transfer;
    [Tooltip("이송 가능한 최대 위치입니다.")]
    public float minRange;
    [Tooltip("이송 가능한 최소 위치입니다.")]
    public float maxRange;
    Vector3 nowPos;
    Vector3 minPos;
    Vector3 maxPos;

    void Start()
    {
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTransferPosition()
    {
        nowPos = new Vector3(transfer.transform.localPosition.x, transfer.transform.localPosition.y, transfer.transform.localPosition.z);
        Vector3 newpos = Vector3.zero;

        switch (motorStatus)
        {
            case 0:
                newpos = minPos;
                break;
            case 1:
                newpos = maxPos;
                break;
        }
    }

    public void Transfer()
    {
        SetTransferPosition();


    }
}
