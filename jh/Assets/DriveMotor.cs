using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveMotor : MonoBehaviour
{
    public Transform transfer;
    public float minRange;
    public float maxRange;
    Vector3 nowPos;
    Vector3 minPos;
    Vector3 maxPos;
    // Start is called before the first frame update
    void Start()
    {
        nowPos = new Vector3(transfer.transform.localPosition.x, transfer.transform.localPosition.y, transfer.transform.localPosition.z);
        minPos = new Vector3(transfer.transform.localPosition.x, minRange, transfer.transform.localPosition.z);
        maxPos = new Vector3(transfer.transform.localPosition.x, maxRange, transfer.transform.localPosition.z);

        print("nowPos = " + nowPos);
        print("minPos = " + minPos);
        print("maxPos = " + maxPos);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("nowPos = " + nowPos);
            print("minPos = " + minPos);
            print("maxPos = " + maxPos);
            Transfer(nowPos, maxPos);
        }
    }

    public void Transfer(Vector3 startPos, Vector3 endPos)
    {
        Vector3 newpos = Vector3.Lerp(startPos, endPos, 3);
    }
}
