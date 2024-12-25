using UnityEngine;

public class WheelSync : MonoBehaviour
{
    public WheelCollider wheelCollider; // ������ �� Wheel Collider
    public Transform wheelMesh; // ������ �� ���������� ������ ������

    void Update()
    {
        if (wheelCollider && wheelMesh)
        {
            // �������� ������� � ������� ������ �� Wheel Collider
            Vector3 position;
            Quaternion rotation;
            wheelCollider.GetWorldPose(out position, out rotation);

            // ��������� ������� � ������� � ���������� ������
            wheelMesh.position = position;
            wheelMesh.rotation = rotation;
        }
    }
}