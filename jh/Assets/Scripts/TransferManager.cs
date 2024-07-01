using MPS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TransferManager : MonoBehaviour
{
    public enum Position
    {
        Default,
        Safe,
        Rotated,
        XMoved,
        YMoved,
        ZMoved,
        BoxLoaded
    }

    public Position positionStatus;
    public MxComponent mxComponent;
    public BoxManager boxManager;
    public Sensor loadingDetector;
    public DriveMotor xTransfer;
    public DriveMotor yTransfer;
    public DriveMotor zTransfer;
    public bool isInSafeZone = false;
    public bool isCounted = false;
    public bool isRotated = false;
    public bool isLoaded = false;

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

    private void Start()
    {
        isInSafeZone = false;
    }

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
            positionStatus = Position.Default;
        }

        if (loadingDetector.isObjectDetected == true && isInSafeZone == false && isCounted == false && positionStatus == Position.Default)
        {
            xTransfer.CoCountBoxQuantity();
            yTransfer.CoCountBoxQuantity();
            zTransfer.CoCountBoxQuantity();

            xTransfer.isSetted = false;
            yTransfer.isSetted = false;
            zTransfer.isSetted = false;
        }

        if (loadingDetector.isObjectDetected == true && isInSafeZone == false && isCounted == true && positionStatus == Position.Default)
        {
            xTransfer.isDriverMoving = true;
            StartCoroutine(xTransfer.CoTransfer());
        }

        if (loadingDetector.isObjectDetected == true && isCounted == true && positionStatus == Position.Safe)
        {
            yTransfer.isDriverMoving = true;
            StartCoroutine(yTransfer.CoTransfer());

            if (boxAFloor % 2 == 0 && isBoxADetected)
            {
                mxComponent.SetDevice(rotaryCylinderDeviceName, 1);
                mxComponent.SetDevice(rotaryCylinderReversedDeviceName, 0);
                isRotated = true;
            }
            else if (boxBFloor % 2 == 0 && isBoxBDetected)
            {
                mxComponent.SetDevice(rotaryCylinderDeviceName, 1);
                mxComponent.SetDevice(rotaryCylinderReversedDeviceName, 0);
                isRotated = true;
            }
        }

        if (loadingDetector.isObjectDetected == true && isLoaded == false && positionStatus == Position.YMoved)
        {
            xTransfer.isDriverMoving = true;
            StartCoroutine(xTransfer.CoTransfer());
        }

        if (loadingDetector.isObjectDetected == true && isLoaded == false && positionStatus == Position.XMoved)
        {
            zTransfer.isDriverMoving = true;
            StartCoroutine(zTransfer.CoTransfer());
        }

        if (loadingDetector.isObjectDetected == true && isLoaded == false && positionStatus == Position.ZMoved)
        {
            mxComponent.SetDevice(loadCylinderForwardDeviceName, 0);
            mxComponent.SetDevice(loadCylinderBackwardDeviceName, 1);
            positionStatus = Position.BoxLoaded;
        }

        if (loadingDetector.isObjectDetected == true && isLoaded == false && positionStatus == Position.BoxLoaded)
        {
            zTransfer.isDriverMoving = true;
            StartCoroutine(zTransfer.CoTransfer());
        }

        if (loadingDetector.isObjectDetected == true && isLoaded == true && positionStatus == Position.ZMoved)
        {
            xTransfer.isDriverMoving = true;
            StartCoroutine(xTransfer.CoTransfer());
        }

        if (loadingDetector.isObjectDetected == true && isLoaded == true && positionStatus == Position.XMoved)
        {
            yTransfer.isDriverMoving = true;
            StartCoroutine(yTransfer.CoTransfer());

            if (isRotated == true)
            {
                mxComponent.SetDevice(rotaryCylinderDeviceName, 0);
                mxComponent.SetDevice(rotaryCylinderReversedDeviceName, 1);
                isRotated = false;
            }
        }

        if (loadingDetector.isObjectDetected == true && isLoaded == true && positionStatus == Position.YMoved)
        {
            xTransfer.isDriverMoving = true;
            StartCoroutine(xTransfer.CoTransfer());
        }
    }
}
