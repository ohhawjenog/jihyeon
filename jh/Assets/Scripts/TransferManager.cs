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
        ZMoved,
        BoxLoaded
    }

    public enum Status
    {
        Default,
        Safe,
        Transfer,
        BoxLoaded
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

        if (loadingDetector.isObjectDetected == true && moved == Moved.Default && status == Status.Default && isRotated == false)
        {
            xTransfer.transform.Translate(Vector3.right * speed);
        }

        if (loadingDetector.isObjectDetected == true && moved == Moved.XMoved && status == Status.Safe && isRotated == false)
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

        if (loadingDetector.isObjectDetected == true && moved == Moved.XMoved && status == Status.Safe && isRotated == true)
        {
            yTransfer.transform.Translate(Vector3.back * speed);
        }

        if (loadingDetector.isObjectDetected == true && moved == Moved.YMoved && status == Status.Transfer && isRotated == true)
        {
            xTransfer.transform.Translate(Vector3.left * speed);
            print(xTransfer.location);
        }

        if (loadingDetector.isObjectDetected == true && moved == Moved.XMoved && status == Status.Transfer && isRotated == true)
        {
            zTransfer.transform.Translate(Vector3.down * speed);
        }

        if (loadingDetector.isObjectDetected == true && moved == Moved.ZMoved && status == Status.Transfer && isRotated == true)
        {
            mxComponent.SetDevice(loadCylinderForwardDeviceName, 0);
            mxComponent.SetDevice(loadCylinderBackwardDeviceName, 1);
            moved = Moved.BoxLoaded;
            status = Status.BoxLoaded;
        }

        if (loadingDetector.isObjectDetected == true && moved == Moved.BoxLoaded && status == Status.BoxLoaded && isRotated == true)
        {
            zTransfer.transform.Translate(Vector3.zero * speed);
        }

        if (loadingDetector.isObjectDetected == true && moved == Moved.ZMoved && status == Status.BoxLoaded && isRotated == true)
        {
            xTransfer.transform.Translate(Vector3.right * speed);
        }

        if (loadingDetector.isObjectDetected == true && moved == Moved.XMoved && status == Status.BoxLoaded && isRotated == true)
        {
            yTransfer.transform.Translate(Vector3.zero * speed);
        }

        if (loadingDetector.isObjectDetected == true && moved == Moved.YMoved && status == Status.BoxLoaded && isRotated == true)
        {
            mxComponent.SetDevice(loadCylinderForwardDeviceName, 1);
            mxComponent.SetDevice(loadCylinderBackwardDeviceName, 0);
            moved = Moved.BoxLoaded;
            status = Status.Safe;
        }

        if (loadingDetector.isObjectDetected == false && moved == Moved.BoxLoaded && status == Status.Safe && isRotated == true)
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

            isRotated = true;
        }

        if (loadingDetector.isObjectDetected == false && moved == Moved.BoxLoaded && status == Status.Safe && isRotated == false)
        {
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
    
