using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 1f; // 이동 속도 설정
    private bool stopMoving = false; // 멈춤 상태 관리

    void Update()
    {
        // 멈춤 상태가 아니라면 x축 방향으로 속도만큼 이동
        if (!stopMoving)
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트의 이름이 "sensor1"이면 멈춤 상태로 전환
        if (other.gameObject.name == "sensor1")
        {
            stopMoving = true;
        }
    }
}
