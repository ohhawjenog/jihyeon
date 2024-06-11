using MPS;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Cylinder : MonoBehaviour
{
    // 상태
    [Header("현재 상태")]
    [Tooltip("GX Works의 Global Device Comment에 설정한 디바이스 값을 입력해 주세요.")]
    public string rearSwitchDeviceName;     //plc연동되는 부분
    public string frontSwitchDeviceName;    //plc연동되는 부분
    public int[] plcInputValues;
    public bool isStartPosition = true;
    public bool isCylinderMoving = false;
    [Tooltip("실린더가 이동을 마치는 데 걸리는 시간입니다.")]
    //public float runTime = 2;
    //float elapsedTime;                      //경과 시간
    public float speed = 1.0f;
    public float distanceLimit = 0.0003f;

    // 초기화
    [Space(20)]
    [Header("초기화")]
    public Transform pistonRod;
    public Transform startPosition;
    public Transform endPosition;
    //public Image forwardButtonImg;
    //public Image backwardButtonImg;
    //public float minRange; //필요한지 아닌지 모르겠다.
    //public float maxRange; //필요한지 아닌지 모르겠다.
    //[Tooltip("실린더가 후진했을 때의 위치입니다.")]
    //Vector3 minPos;
    //[Tooltip("실린더가 전진했을 때의 위치입니다.")]
    //Vector3 maxPos;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CoMove(isStartPosition)); //나중에 지우기
        //InitEquipment();
    }

    /*private void InitEquipment()
    {
        //SetCylinderBtnActive(true);

        //minPos = new Vector3(pistonRod.transform.localPosition.x, minRange, pistonRod.transform.localPosition.z); //필요한지 아닌지 모르겠다.
        //maxPos = new Vector3(pistonRod.transform.localPosition.x, maxRange, pistonRod.transform.localPosition.z); //필요한지 아닌지 모르겠다.

        //minPos = (endPosition.position - transform.position).normalized;
        //maxPos = (startPosition.position - transform.position).normalized;
    }*/

    private void Update()
    {
        if (MxComponent.instance.connection == MxComponent.Connection.Connected) //plc에서 신호를 받아옴
        {
            // 실린더 전진
            if (plcInputValues[0] == 1)
                StartCoroutine(CoMove(isStartPosition));

            // 실린더 후진
            if (plcInputValues[1] == 1)
                StartCoroutine(CoMove(!isStartPosition));
        }
    }

    public void MovePistonRod(Transform destination) //(Vector3 startPos, Vector3 endPos, float _elapsedTime, float _runTime)
    {
        Vector3 newPos = (destination.position - transform.position).normalized;
        float distance = (destination.position - transform.position).magnitude;

        if (distance > distanceLimit)
            transform.position += newPos * Time.deltaTime * speed;
        else
            GetComponent<Rigidbody>().velocity = Vector3.zero;

        //Vector3 newPos = Vector3.Lerp(startPos, endPos, _elapsedTime / _runTime); // t값이 0(minPos) ~ 1(maxPos) 로 변화
        //pistonRod.transform.localPosition = newPos;
    }

    /*public void OnDischargeObjectBtnEvent()
    {
        print("작동!");
        if(sensor != null && sensor.isMetalObject)
        {
            print("배출 완료");
            OnCylinderButtonClickEvent(true);
        }
    }

    // PistonRod가 Min, Max 까지
    // 참고: LocalTransform.position.y가 -0.3 ~ 1.75 까지 이동
    public void OnCylinderButtonClickEvent(bool direction)
    {
        StartCoroutine(CoMove(isBackward));

        audioSource.Play();
    }*/

    public void SetSwitchDevicesByCylinderMoving(bool _isCylinderMoving, bool _isStartPosition)
    {
        if (_isCylinderMoving)
        {
            //MxComponent.instance.SetDevice(rearSwitchDeviceName, 0);
            //MxComponent.instance.SetDevice(frontSwitchDeviceName, 0);
            print($"isBackward: {_isStartPosition}, {rearSwitchDeviceName}: 0");
            print($"isBackward: {_isStartPosition}, {frontSwitchDeviceName}: 0");

            return;
        }

        if (_isStartPosition)
        {
            //MxComponent.instance.SetDevice(rearSwitchDeviceName, 1);
            print($"isBackward: {_isStartPosition}, {rearSwitchDeviceName}: 1");
        }
        else
        {
            //MxComponent.instance.SetDevice(frontSwitchDeviceName, 1);
            print($"isBackward: {_isStartPosition}, {frontSwitchDeviceName}: 1");
        }
    }

    IEnumerator CoMove(bool direction)
    {
        isCylinderMoving = true;

        //SetButtonActive(false);
        //SetCylinderBtnActive(false);
        //SetSwitchDevicesByCylinderMoving(isCylinderMoving, isStartPosition); // 스위치 값 변경

        //elapsedTime = 0;

        while (true) //elapsedTime < runTime
        {
            //elapsedTime += Time.deltaTime;

            if (isStartPosition)
            {
                MovePistonRod(endPosition);
            }
            else
            {
                MovePistonRod(startPosition);
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }


        isStartPosition = !isStartPosition; // 초기값(true) -> false
        isCylinderMoving = false;

        SetSwitchDevicesByCylinderMoving(isCylinderMoving, isStartPosition);
        //SetCylinderBtnActive(true);
        //SetButtonActive(true);
    }

    /*void SetCylinderBtnActive(bool isActive)
    {
        if (isActive)
        {
            if (isBackward)
            {
                forwardButtonImg.color = Color.white;
                backwardButtonImg.color = Color.green;
            }
            else
            {
                forwardButtonImg.color = Color.green;
                backwardButtonImg.color = Color.white;
            }
        }
        else
        {
            forwardButtonImg.color = Color.white;
            forwardButtonImg.color = Color.white;
        }

    }

    void SetButtonActive(bool isActive)
    {
        if (MPSMxComponent.instance.connection == MPSMxComponent.Connection.Connected)
            return;

        forwardButtonImg.GetComponent<Button>().interactable = isActive;
        backwardButtonImg.GetComponent<Button>().interactable = isActive;
    }*/
}
