using UnityEngine;

public class MaxDistanceTracker : MonoBehaviour
{
    private Vector3 startPosition;
    private float maxDistance = 0f;

    void Start()
    {
        // ���� ��ġ�� ���� ��ġ�� �����մϴ�.
        startPosition = transform.position;
    }

    void Update()
    {
        // ���� ��ġ�� ���� ��ġ ������ �Ÿ��� ����մϴ�.
        float currentDistance = Vector3.Distance(startPosition, transform.position);

        // ���� ���� �Ÿ��� �ִ� �Ÿ����� ũ��, �ִ� �Ÿ��� ������Ʈ�մϴ�.
        if (currentDistance > maxDistance)
        {
            maxDistance = currentDistance;
        }
    }

    void OnDestroy()
    {
        // ������Ʈ�� �ı��� �� �ִ� �̵� �Ÿ��� �ֿܼ� ����մϴ�.
        Debug.Log("�ִ� �̵� �Ÿ�: " + maxDistance);
    }
}