using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Объект, за которым будет следовать камера
    public Vector3 offset = new Vector3(0, 2.85f, 1.65f); // Смещение камеры относительно объекта
    public float smoothTime = 0.5f; // Время плавного следования

    private Vector3 velocity = Vector3.zero; // Скорость изменения позиции

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Target is not assigned!");
            return;
        }

        // Вычисляем желаемую позицию камеры
        Vector3 desiredPosition = target.position + offset;

        // Плавно перемещаем камеру к желаемой позиции с использованием SmoothDamp
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
        transform.position = smoothedPosition;

        // Опционально: Смотрим на объект
        transform.LookAt(target);
    }
}