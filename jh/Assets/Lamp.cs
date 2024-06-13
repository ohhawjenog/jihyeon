using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{

    private bool colorChanged = false;

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if (!colorChanged)
        {
            // 스페이스바를 눌렀는지 확인
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Q))
            {
                // 색상 변경
                Renderer renderer = GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = Color.red;
                    colorChanged = true; // 색상이 변경되었음을 표시
                }
                else
                {
                    Debug.LogError("이 오브젝트에 Renderer 컴포넌트가 없습니다.");
                }
            }
        }
    }

}

