using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box1 : MonoBehaviour
{
    public float speed = 1.0f;
    public Vector3 direction = new Vector3(1, 0, 0);
    private bool isMoving = false;
    private bool isStoppedBySensor = false; // ������ ���� ���� ���¸� ����

    public void StartMoving()
    {
        if (!isStoppedBySensor)
        {
            isMoving = true;
            StartCoroutine(CoMove());
        }
    }

    public void StopMoving()
    {
        isMoving = false;
        StopAllCoroutines();
    }

    IEnumerator CoMove()
    {
        while (isMoving)
        {
            transform.position += direction * Time.deltaTime * speed;
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sensor"))
        {
            StopMoving();
            isStoppedBySensor = true; // ������ ���� ���� ���·� ����
        }
    }


}