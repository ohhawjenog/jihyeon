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
            // �����̽��ٸ� �������� Ȯ��
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Q))
            {
                // ���� ����
                Renderer renderer = GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = Color.red;
                    colorChanged = true; // ������ ����Ǿ����� ǥ��
                }
                else
                {
                    Debug.LogError("�� ������Ʈ�� Renderer ������Ʈ�� �����ϴ�.");
                }
            }
        }
    }

}

