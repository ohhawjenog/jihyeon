using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActUtlTypeLib;

public class MxComponent : MonoBehaviour
{
    //ActUtlType mxComponent;
    ActUtlType64Lib.ActUtlType64 mxComponent;

    void Start()
    {
        //mxComponent = new ActUtlType();
        mxComponent = new ActUtlType64Lib.ActUtlType64();
        mxComponent.ActLogicalStationNumber = 1;
    }

    public void OnConnectPLCBtnClkEvent()
    {
        int returnValue = mxComponent.Open();

        if (returnValue == 0)
        {
            print("연결에 성공했습니다.");
        }
        else
        {
            print("연결에 실패했습니다. returnValue: " +  returnValue);
        }
    }

    public void OnDisconnectPLCBtnClkEvent()
    {

    }
}
