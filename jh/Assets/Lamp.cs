using MPS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lamp : MonoBehaviour
{
    public string forwardName;
    public string backwardName;
    public int[] plcInputValues; // 편솔의 경우 입력 1개, 양솔의 경우 입력 2개
    public Image forwardButtonImg;
    public Image backwardButtonImg;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if (MxComponent.instance.connection == MxComponent.Connection.Connected)
        {
            // 스페이스바를 눌렀는지 확인
            if (plcInputValues[0] > 0)
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
            //forwardButtonImg.color = Color.white;
            //forwardButtonImg.color = Color.white;
        }

        
    }

}