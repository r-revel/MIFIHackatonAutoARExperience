// using UnityEngine;
// using UnityEngine.XR.ARFoundation;
// using UnityEngine.XR.ARSubsystems;
// using OpenCVForUnity.CoreModule;
// using OpenCVForUnity.ImgprocModule;
// using OpenCVForUnity.UnityUtils;
// using OpenCVForUnity.ObjdetectModule;

// public class FrameDetector : MonoBehaviour
// {
//     public GameObject arCamera; // Ссылка на AR Camera
//     public GameObject imageTargetPrefab; // Prefab для 3D модели
//     private ARCameraManager cameraManager;
//     private Texture2D cameraTexture;
//     private Mat frameMat;
//     private Mat grayMat;
//     // private CascadeClassifier cascadeClassifier;

//     void Start()
//     {
//         cameraManager = arCamera.GetComponent<ARCameraManager>();
//         cameraTexture = new Texture2D(640, 480, TextureFormat.RGBA32, false);
//         frameMat = new Mat(480, 640, CvType.CV_8UC4);
//         grayMat = new Mat(480, 640, CvType.CV_8UC1);

//         // Загрузите каскадный классификатор для детектирования рамки
//         // string cascadePath = "path/to/your/cascade.xml";
//         // cascadeClassifier = new CascadeClassifier(cascadePath);
//     }

//     void Update()
//     {
//         if (cameraManager.subsystem != null && cameraManager.subsystem.TryAcquireLatestCpuImage(out XRCpuImage image))
//         {
//             // Преобразуем изображение в формат,compatible с OpenCV
//             NativeArray<byte> data = image.GetPixelData<byte>(XRCpuImage.ConvertParams.Format.RGBA32);
//             cameraTexture.LoadRawTextureData(data);
//             cameraTexture.Apply();
//             Utils.texture2DToMat(cameraTexture, frameMat);

//             // Преобразуем изображение в оттенки серого
//             Imgproc.cvtColor(frameMat, grayMat, Imgproc.COLOR_RGBA2GRAY);

//             // Детектируем рамку
//             // Rect[] rects = cascadeClassifier.detectMultiScale(grayMat);

//             // foreach (Rect rect in rects)
//             // {
//             //     // Создаем 3D модель на позиции рамки
//             //     Vector3 position = new Vector3(rect.x, rect.y, 0);
//             //     Quaternion rotation = Quaternion.identity;
//             //     Instantiate(imageTargetPrefab, position, rotation);
//             // }

//             // Освобождаем ресурсы
//             data.Dispose();
//         }
//     }
// }