using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveMotor : MonoBehaviour
{
    [Header("Device Info")]
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
    // Start is called before the first frame update
    void Start()
    {

        minRange = 0;
        maxRange = -1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Transfer()
    {
        nowPos = new Vector3(transfer.transform.localPosition.x, transfer.transform.localPosition.y, transfer.transform.localPosition.z);
        minPos = new Vector3(transfer.transform.localPosition.x, transfer.transform.localPosition.y, minRange);
        maxPos = new Vector3(transfer.transform.localPosition.x, transfer.transform.localPosition.y, maxRange);
        Vector3 newpos = new Vector3(transfer.transform.localPosition.x, transfer.transform.localPosition.y, transfer.transform.localPosition.z);

        switch (motorStatus)
        {
            case 0:
                newpos = Vector3.Lerp(nowPos, minPos, 1);
                break;
            case 1:
                newpos = Vector3.Lerp(nowPos, maxPos, 1);
                break;
        }
    }
}
