using MPS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public int[] plcInputValues;
    public int plcInputBoxAQuantity;
    public int plcInputBoxBQuantity;
    public bool isDriveMoving;
    public bool isDriveReverse;
    public int boxACount;
    public int boxBCount;
    public MxComponent mxComponent;

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
    Vector3 minPos;
    Vector3 maxPos;

    void Start()
    {
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

        plcInputValues = new int[2];

        SetToMove();

    }

    private void Update()
    {
        boxACount = mxComponent.boxACount;
        boxBCount = mxComponent.boxBCount;

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
    }

    public void MoveDrive(Vector3 startPos, Vector3 endPos, float _elapsedTime, float _runTime)
    {
        Vector3 newPos = Vector3.Lerp(startPos, endPos, _elapsedTime / _runTime);
        transfer.transform.localPosition = newPos;
    }

    public void SetToMove()
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

    }

    IEnumerator CoTransfer()
    {
        SetToMove();
        isDriveMoving = true;

        elapsedTime = 0;

        while ( plcInputValues[0] > 0 || plcInputValues[1] > 0)
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
