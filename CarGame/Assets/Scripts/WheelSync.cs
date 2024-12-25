using UnityEngine;

public class WheelSync : MonoBehaviour
{
    public WheelCollider wheelCollider; // Ссылка на Wheel Collider
    public Transform wheelMesh; // Ссылка на визуальную модель колеса

    void Update()
    {
        if (wheelCollider && wheelMesh)
        {
            // Получаем позицию и поворот колеса из Wheel Collider
            Vector3 position;
            Quaternion rotation;
            wheelCollider.GetWorldPose(out position, out rotation);

            // Применяем позицию и поворот к визуальной модели
            wheelMesh.position = position;
            wheelMesh.rotation = rotation;
        }
    }
}