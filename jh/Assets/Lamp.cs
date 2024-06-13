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
            Debug.LogError("�� ������Ʈ�� Renderer ������Ʈ�� �����ϴ�.");
            return;
        }

        if (!colorChanged)
        {
            // �����̽��ٸ� �������� Ȯ��
            if (Input.GetKey(KeyCode.A) && Input.GetKeyDown(KeyCode.LeftShift))
            {
                // ���� ����

                if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.A))
                {
                    // ���� ����
                    renderer.material.color = Color.red;
                    colorChanged = true; // ������ ����Ǿ����� ǥ��
                    Debug.Log("Color changed to red");
                }
            }
            
        }
        else
        {
            // S Ű�� LeftShift Ű�� �������� Ȯ��
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.S))
            {
                // ���� �������� ����
                renderer.material.color = originalColor;
                colorChanged = false; // ������ ������� ���ƿ����� ǥ��
                Debug.Log("Color reverted to original");
            }
        }
    }

}

