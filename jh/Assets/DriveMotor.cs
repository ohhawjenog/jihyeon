using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public int[] plcInputValues;
    public bool isDriveReverse;

    [Space(20)]
    [Header("Transfer Position")]
    public Transform transfer;
    [Tooltip("�̼� ������ �ִ� ��ġ�Դϴ�.")]
    public float minRange;
    [Tooltip("�̼� ������ �ּ� ��ġ�Դϴ�.")]
    public float maxRange;
    [Tooltip("�̼� ������ ���� �� ���� ��ġ�Դϴ�. (0 ~ 1)")]
    public float transportRate;
    [Tooltip("�ּ� ��ġ���� �ִ� ��ġ���� �̼ۿ� �ҿ�Ǵ� �ð��Դϴ�.")]
    public float transferTime;
    float elapsedTime;
    Vector3 nowPos;
    Vector3 minPos;
    Vector3 maxPos;

    void Start()
    {
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

        SetToMove();

        print((int)direction + ": " + transportRate * 100 + "%");
    }

    void Update()
    {
        
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
            case Direction.MoveXAxis:
                transportRate = (maxRange - transfer.transform.localPosition.x) / (maxRange - minRange);
                break;
            case Direction.MoveYAxis:
                transportRate = (maxRange - transfer.transform.localPosition.y) / (maxRange - minRange);
                break;
            case Direction.MoveZAxis:
                transportRate = (maxRange - transfer.transform.localPosition.z) / (maxRange - minRange);
                break;
        }
    }

    IEnumerator Transfer()
    {
        SetToMove();

        while ( elapsedTime < transferTime * transportRate)
        {
            elapsedTime += Time.deltaTime;


        }
    }
}
