using UnityEngine;

public class MaxDistanceTracker : MonoBehaviour
{
    private Vector3 startPosition;
    private float maxDistance = 0f;

    void Start()
    {
        // 시작 위치를 현재 위치로 설정합니다.
        startPosition = transform.position;
    }

    void Update()
    {
        // 현재 위치와 시작 위치 사이의 거리를 계산합니다.
        float currentDistance = Vector3.Distance(startPosition, transform.position);

        // 만약 현재 거리가 최대 거리보다 크면, 최대 거리를 업데이트합니다.
        if (currentDistance > maxDistance)
        {
            maxDistance = currentDistance;
        }
    }

    void OnDestroy()
    {
        // 오브젝트가 파괴될 때 최대 이동 거리를 콘솔에 출력합니다.
        Debug.Log("최대 이동 거리: " + maxDistance);
    }
}