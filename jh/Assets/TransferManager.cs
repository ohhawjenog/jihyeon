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
    public bool isInSafeZone;
    public int boxACount;
    public int boxAFloor;
    public int boxBCount;
    public int boxBFloor;

    public string rotaryCylinderDeviceName;
    public string rotaryCylinderReversedDeviceName;
    public string loadCylinderForwardDeviceName;
    public string loadCylinderBackwardDeviceName;

    [Space(20)]
    [Header("Palletizing Setting")]
    //public float boxAOddFloorDefaultLocation;
    //public float boxAEvenFloorDefaultLocation;
    public float boxAHorizontalDistance;
    public int boxAHorizontalQuantity;
    public float boxAVerticalDistance;
    public int boxAVerticalQuantity;
    public float boxAHeight;
    //public float boxBOddFloorDefaultLocation;
    //public float boxBEvenFloorDefaultLocation;
    public float boxBHorizontalDistance;
    public int boxBHorizontalQuantity;
    public float boxBVerticalDistance;
    public int boxBVerticalQuantity;
    public float boxBHeight;

    private void Start()
    {
        isInSafeZone = false;
        positionStatus = Position.Default;
    }

    private void Update()
    {
        boxACount = mxComponent.boxACount;
        boxBCount = mxComponent.boxBCount;
        boxAFloor = (int)(boxACount / (boxAHorizontalQuantity * boxAVerticalQuantity)) + 1;
        boxBFloor = (int)(boxBCount / (boxBHorizontalQuantity * boxBVerticalQuantity)) + 1;

        if (loadingDetector.isObjectDetected == true && isInSafeZone == false && positionStatus == Position.Default)
        {
            StartCoroutine(xTransfer.CoTransferToSafeZone());
        }

        if (loadingDetector.isObjectDetected == true && isInSafeZone == true && positionStatus == Position.XMoved)
        {
            StartCoroutine(yTransfer.CoTransfer());

            if (boxManager.isBoxADetected == true && boxAFloor % 2 == 0)
            {
                mxComponent.SetDevice(rotaryCylinderDeviceName, 1);
            }
            else if (boxManager.isBoxBDetected == true && boxBFloor % 2 == 0)
            {
                mxComponent.SetDevice(rotaryCylinderDeviceName, 1);
            }
        }

        if (loadingDetector.isObjectDetected == true && isInSafeZone == false && positionStatus == Position.YMoved)
        {
            StartCoroutine(xTransfer.CoTransfer());
        }

        if (loadingDetector.isObjectDetected == true && isInSafeZone == false && positionStatus == Position.XMoved)
        {
            StartCoroutine(zTransfer.CoTransfer());
        }

        if (loadingDetector.isObjectDetected == true && isInSafeZone == false && positionStatus == Position.ZMoved)
        {
            mxComponent.SetDevice(loadCylinderForwardDeviceName, 0);
            mxComponent.SetDevice(loadCylinderBackwardDeviceName, 1);
            positionStatus = Position.BoxLoaded;
        }

        if (loadingDetector.isObjectDetected == false && isInSafeZone == false && positionStatus == Position.BoxLoaded)
        {
            StartCoroutine(zTransfer.CoTransfer());
        }

        if (loadingDetector.isObjectDetected == false && isInSafeZone == false && positionStatus == Position.ZMoved)
        {
            StartCoroutine(xTransfer.CoTransfer());
        }

        if (loadingDetector.isObjectDetected == false && isInSafeZone == false && positionStatus == Position.XMoved)
        {
            StartCoroutine(yTransfer.CoTransfer());

            if (boxManager.isBoxADetected == true && boxAFloor % 2 == 0)
            {
                mxComponent.SetDevice(rotaryCylinderReversedDeviceName, 1);
            }
            else if (boxManager.isBoxBDetected == true && boxBFloor % 2 == 0)
            {
                mxComponent.SetDevice(rotaryCylinderReversedDeviceName, 1);
            }

            mxComponent.SetDevice(loadCylinderForwardDeviceName, 1);
            mxComponent.SetDevice(loadCylinderBackwardDeviceName, 0);
        }

        if (loadingDetector.isObjectDetected == false && isInSafeZone == false && positionStatus == Position.YMoved)
        {
            StartCoroutine(xTransfer.CoTransferToDefault());
        }
    }
}
