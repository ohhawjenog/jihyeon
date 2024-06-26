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
        Safe = 1,
        XMoved = 2,
        YMoved = 3,
        ZMoved = 4
    }

    public Position positionStatus;

    private void Start()
    {
        positionStatus = Position.Default;
    }
}
