using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 1f; // �̵� �ӵ� ����
    private bool stopMoving = false; // ���� ���� ����

    void Update()
    {
        // ���� ���°� �ƴ϶�� x�� �������� �ӵ���ŭ �̵�
        if (!stopMoving)
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // �浹�� ������Ʈ�� �̸��� "sensor1"�̸� ���� ���·� ��ȯ
        if (other.gameObject.name == "sensor1")
        {
            stopMoving = true;
        }
    }
}
