using MPS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TransferManager : MonoBehaviour
{
    public enum Moved
    {
        Default,
        XMoved,
        YMoved,
        ZMoved
    }

    public enum Status
    {
        Default,
        Safe,
        Transfer
    }

    public Moved moved;
    public Status status;
    public MxComponent mxComponent;
    public BoxManager boxManager;
    public Sensor loadingDetector;
    public DriveMotor xTransfer;
    public DriveMotor yTransfer;
    public DriveMotor zTransfer;
    public bool isCounted = false;
    public bool isRotated = false;
    public bool isLoaded = false;
    public bool isXMoving;
    public bool isYMoving;
    public bool isZMoving;

    public bool isBoxADetected;
    public int boxACount;
    public int boxAFloor;
    public bool isBoxBDetected;
    public int boxBCount;
    public int boxBFloor;

    public string rotaryCylinderDeviceName;
    public string rotaryCylinderReversedDeviceName;
    public string loadCylinderForwardDeviceName;
    public string loadCylinderBackwardDeviceName;

    [Space(20)]
    [Header("Palletizing Setting")]
    public float speed;
    public float boxAHorizontalDistance;
    public int boxAHorizontalQuantity;
    public float boxAVerticalDistance;
    public int boxAVerticalQuantity;
    public float boxAHeight;
    public float boxBHorizontalDistance;
    public int boxBHorizontalQuantity;
    public float boxBVerticalDistance;
    public int boxBVerticalQuantity;
    public float boxBHeight;

    private void Update()
    {
        if (boxManager.isBoxADetected == true)
        {
            isBoxADetected = true;
            isBoxBDetected = false;
        }
        else if (boxManager.isBoxBDetected == true)
        {
            isBoxADetected = false;
            isBoxBDetected = true;
        }

        boxACount = mxComponent.boxACount;
        boxBCount = mxComponent.boxBCount;
        boxAFloor = (int)(boxACount / (boxAHorizontalQuantity * boxAVerticalQuantity)) + 1;
        boxBFloor = (int)(boxBCount / (boxBHorizontalQuantity * boxBVerticalQuantity)) + 1;

        if (xTransfer.transform.position == Vector3.zero && yTransfer.transform.position == Vector3.zero && zTransfer.transform.position == Vector3.zero)
        {
            isRotated = false;
            isLoaded = false;
            moved = Moved.Default;
            status = Status.Default;
        }

        if(loadingDetector.isObjectDetected == false)
        {
            isCounted = false;
        }

        if (loadingDetector.isObjectDetected == true && isCounted == false)
        {
            StartCoroutine(CoSet());
        }

        // 1. X축 이송
        if (loadingDetector.isObjectDetected == true && moved == Moved.Default && status == Status.Default && isRotated == false && isLoaded == false)
        {
            isXMoving = true;
            isYMoving = false;
            isZMoving = false;
            xTransfer.transform.Translate(Vector3.right * speed);
        }

        // 2. 로터리 실린더 회전
        if (loadingDetector.isObjectDetected == true && moved == Moved.XMoved && status == Status.Safe && isRotated == false && isLoaded == false)
        {
            if (boxAFloor % 2 == 0 && isBoxADetected)
            {
                mxComponent.SetDevice(rotaryCylinderDeviceName, 1);
                mxComponent.SetDevice(rotaryCylinderReversedDeviceName, 0);
            }
            else if (boxBFloor % 2 == 0 && isBoxBDetected)
            {
                mxComponent.SetDevice(rotaryCylinderDeviceName, 1);
                mxComponent.SetDevice(rotaryCylinderReversedDeviceName, 0);
            }

            isRotated = true;
        }

        // 3. Y축 이송
        if (loadingDetector.isObjectDetected == true && moved == Moved.XMoved && status == Status.Safe && isRotated == true && isLoaded == false)
        {
            isXMoving = false;
            isYMoving = true;
            isZMoving = false;
            yTransfer.transform.Translate(Vector3.back * speed);
        }

        // 4. X축 이송
        if (loadingDetector.isObjectDetected == true && moved == Moved.YMoved && status == Status.Transfer && isRotated == true && isLoaded == false)
        {
            isXMoving = true;
            isYMoving = false;
            isZMoving = false;
            xTransfer.transform.Translate(Vector3.left * speed);
        }

        // 5. Z축 이송
        if (loadingDetector.isObjectDetected == true && moved == Moved.XMoved && status == Status.Transfer && isRotated == true && isLoaded == false)
        {
            isXMoving = false;
            isYMoving = false;
            isZMoving = true;
            zTransfer.transform.Translate(Vector3.down * speed);
        }

        // 6. 로드 실린더 후진
        if (loadingDetector.isObjectDetected == true && moved == Moved.ZMoved && status == Status.Transfer && isRotated == true && isLoaded == false)
        {
            mxComponent.SetDevice(loadCylinderForwardDeviceName, 0);
            mxComponent.SetDevice(loadCylinderBackwardDeviceName, 1);
            isLoaded = true;
        }

        // 7. Z축 이송
        if (loadingDetector.isObjectDetected == true && moved == Moved.ZMoved && isRotated == true && isLoaded == true)
        {
            isYMoving = false;
            isZMoving = true;
            zTransfer.transform.Translate(Vector3.zero * speed);
        }

        // 8. X축 이송
        if (loadingDetector.isObjectDetected == true && moved == Moved.ZMoved && isRotated == true && isLoaded == true)
        {
            isXMoving = true;
            isYMoving = false;
            xTransfer.transform.Translate(Vector3.right * speed);
        }

        // 9. Y축 이송
        if (loadingDetector.isObjectDetected == true && moved == Moved.XMoved && isRotated == true && isLoaded == true)
        {
            isXMoving = false;
            isYMoving = true;
            isZMoving = false;
            yTransfer.transform.Translate(Vector3.zero * speed);
        }

        // 10. 로드 실린더 전진
        if (loadingDetector.isObjectDetected == true && moved == Moved.YMoved && status == Status.Safe && isRotated == true && isLoaded == true)
        {
            mxComponent.SetDevice(loadCylinderForwardDeviceName, 1);
            mxComponent.SetDevice(loadCylinderBackwardDeviceName, 0);
            isLoaded = false;
        }

        // 11. 로터리 실린더 역회전
        if (loadingDetector.isObjectDetected == false && status == Status.Safe && isRotated == true && isLoaded == false)
        {
            if (boxAFloor % 2 == 0 && isBoxADetected)
            {
                mxComponent.SetDevice(rotaryCylinderDeviceName, 0);
                mxComponent.SetDevice(rotaryCylinderReversedDeviceName, 1);
            }
            else if (boxBFloor % 2 == 0 && isBoxBDetected)
            {
                mxComponent.SetDevice(rotaryCylinderDeviceName, 0);
                mxComponent.SetDevice(rotaryCylinderReversedDeviceName, 1);
            }

            isRotated = false;
        }

        // 12. X축 이송
        if (loadingDetector.isObjectDetected == false && status == Status.Safe && isRotated == false && isLoaded == false)
        {
            isXMoving = true;
            isYMoving = false;
            isZMoving = false;
            xTransfer.transform.Translate(Vector3.zero * speed);
        }
    }

    IEnumerator CoSet()
    {
        xTransfer.CountBoxQuantity();
        yTransfer.CountBoxQuantity();
        zTransfer.CountBoxQuantity();

        if (isBoxADetected)
        {
            xTransfer.SetToMoveA();
            yTransfer.SetToMoveA();
            zTransfer.SetToMoveA();
        }
        else if (isBoxBDetected)
        {
            xTransfer.SetToMoveB();
            yTransfer.SetToMoveB();
            zTransfer.SetToMoveB();
        }

        isCounted = true;

        yield return null;
    }
}
    
