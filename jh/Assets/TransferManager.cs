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
        Default = 0,
        Safe    = 1,
        XMoved  = 2,
        YMoved  = 3,
        ZMoved  = 4
    }

    public Position positionStatus;
    public BoxManager boxManager;
    public Sensor loadingDetector;
    public DriveMotor xTransfer;
    public DriveMotor yTransfer;
    public DriveMotor zTransfer;
    public bool isInSafeZone;

    private void Start()
    {
        isInSafeZone = false;
        positionStatus = Position.Default;
    }

    private void Update()
    {
        if (loadingDetector.isObjectDetected == true && isInSafeZone == false)
        {
            StartCoroutine(zTransfer.CoTransferToSafeZone());
        }
    }
}
