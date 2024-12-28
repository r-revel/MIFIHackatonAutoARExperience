using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using  FAClient;

public class ImageUploader : MonoBehaviour
{
    public string serverUrl = "http://yourserver.com/upload";
    public CameraCapture cameraCapture;
    public float interval = 3.0f; // Интервал отправки изображений в секундах

    private float timer = 0.0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            UploadImage();
            timer = 0.0f;
        }
    }

    public void UploadImage()
    {
        Texture2D image = cameraCapture.CaptureImage();
        byte[] bytes = image.EncodeToJPG();

        string baseUrl = "http://127.0.0.1:8000";
        IFAClient client = FA.GetClient(baseUrl);

        // Используем MemoryStream для отправки байтов
        // using (MemoryStream stream = new MemoryStream(bytes))
        // {
            var response = client.Detect(bytes);

            if (response.Probability >= 0.9)
            {
                Debug.Log(response.ClassName);
            }
            else
            {
                Debug.Log("Недостаточная вероятность: " + response.ClassName);
            }
        // }
    }
}