using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{

    private bool colorChanged = false;
    private Color originalColor;

    // Start is called before the first frame update
    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            originalColor = renderer.material.color;
        }

    }

    // Update is called once per frame
    void Update()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null)
        {
            Debug.LogError("이 오브젝트에 Renderer 컴포넌트가 없습니다.");
            return;
        }

        if (!colorChanged)
        {
            // 스페이스바를 눌렀는지 확인
            if (Input.GetKey(KeyCode.A) && Input.GetKeyDown(KeyCode.LeftShift))
            {
                // 색상 변경

                if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.A))
                {
                    // 색상 변경
                    renderer.material.color = Color.red;
                    colorChanged = true; // 색상이 변경되었음을 표시
                    Debug.Log("Color changed to red");
                }
            }
            
        }
        else
        {
            // S 키와 LeftShift 키를 눌렀는지 확인
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.S))
            {
                // 원래 색상으로 복원
                renderer.material.color = originalColor;
                colorChanged = false; // 색상이 원래대로 돌아왔음을 표시
                Debug.Log("Color reverted to original");
            }
        }
    }

}

