using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // ������, �� ������� ����� ��������� ������
    public Vector3 offset = new Vector3(0, 2.85f, 1.65f); // �������� ������ ������������ �������
    public float smoothTime = 0.5f; // ����� �������� ����������

    private Vector3 velocity = Vector3.zero; // �������� ��������� �������

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Target is not assigned!");
            return;
        }

        // ��������� �������� ������� ������
        Vector3 desiredPosition = target.position + offset;

        // ������ ���������� ������ � �������� ������� � �������������� SmoothDamp
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
        transform.position = smoothedPosition;

        // �����������: ������� �� ������
        transform.LookAt(target);
    }
}