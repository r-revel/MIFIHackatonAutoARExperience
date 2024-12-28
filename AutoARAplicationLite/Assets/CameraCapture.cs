using UnityEngine;
using System.IO;

public class CameraCapture : MonoBehaviour
{
    public WebCamTexture webcamTexture;
    public Renderer renderer;

    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length > 0)
        {
            webcamTexture = new WebCamTexture(devices[0].name, 640, 480);
            renderer.material.mainTexture = webcamTexture;
            webcamTexture.Play();
        }
        else
        {
            Debug.LogError("No webcam found!");
        }
    }

    public Texture2D CaptureImage()
    {
        Texture2D photo = new Texture2D(webcamTexture.width, webcamTexture.height);
        photo.SetPixels(webcamTexture.GetPixels());
        photo.Apply();
        return photo;
    }
}