namespace OpenCvSharp.Demo
{
    using UnityEngine;
    using FAClient;
    using UnityEngine.UI;
    using OpenCvSharp;
    using OpenCvSharp.Aruco;
    using System.IO;
    using System.Threading.Tasks;
    using System;

    public class MarkerDetector : WebCamera
    {
        public RawImage rawImage; // Ссылка на RawImage для отображения результата
        private IFAClient client;
        private int frameCounter = 0; // Счетчик кадров
        private const int framesPerSecond = 5; // Частота отправки изображений в кадрах в секунду
        private float frameInterval = 1.0f / framesPerSecond; // Интервал между отправками в секундах
        private float lastSendTime = 0.0f; // Время последней отправки
        public Texture2D outputTexture;
         public int framesBetweenSending = 20; // Количество кадров между отправками
        protected override void Awake()
        {
            base.Awake();
            this.forceFrontalCamera = true;
            string baseUrl = "http://127.0.0.1:8000";
            client = FA.GetClient(baseUrl);
        }
        private void Update() {
            if (webCamTexture.isPlaying) {
                // Обрабатываем изображение каждый кадр
                ProcessTexture(webCamTexture, ref outputTexture);

                // Увеличиваем счетчик кадров
                frameCounter++;

                // Проверяем, нужно ли отправлять изображение на сервер
                if (frameCounter >= framesBetweenSending) {
                    frameCounter = 0;
                    SendImageToServer(outputTexture);
                }
            }
        }
        protected override bool ProcessTexture(WebCamTexture input, ref Texture2D output)
        {
            Mat img = Unity.TextureToMat(input, TextureParameters);


            // Увеличение резкости
            Mat sharpMat = new Mat();
            float[] kernelData = { 0, -1, 0, -1, 5, -1, 0, -1, 0 };
            Mat kernel = new Mat(3, 3, MatType.CV_32F, kernelData);
            Cv2.Filter2D(img, sharpMat, -1, kernel);

            // Уменьшение шума
            Mat denoisedMat = new Mat();
            Cv2.FastNlMeansDenoisingColored(sharpMat, denoisedMat, 10, 10, 7, 21);

            // Создание параметров для детектирования
            DetectorParameters detectorParameters = DetectorParameters.Create();
            detectorParameters.AdaptiveThreshWinSizeMin = 3;
            detectorParameters.AdaptiveThreshWinSizeMax = 23;
            detectorParameters.AdaptiveThreshWinSizeStep = 10;
            detectorParameters.AdaptiveThreshConstant = 7;
            detectorParameters.MinMarkerPerimeterRate = 0.03;
            detectorParameters.MaxMarkerPerimeterRate = 4.0;
            detectorParameters.PolygonalApproxAccuracyRate = 0.05;
            detectorParameters.MinCornerDistanceRate = 0.05;
            detectorParameters.MinDistanceToBorder = 3;
            detectorParameters.MinMarkerDistanceRate = 0.05;
            detectorParameters.DoCornerRefinement = true;
            detectorParameters.CornerRefinementWinSize = 5;
            detectorParameters.CornerRefinementMaxIterations = 30;
            detectorParameters.CornerRefinementMinAccuracy = 0.1;
            detectorParameters.MarkerBorderBits = 1;
            detectorParameters.PerspectiveRemovePixelPerCell = 8;
            detectorParameters.PerspectiveRemoveIgnoredMarginPerCell = 0.13;
            detectorParameters.MaxErroneousBitsInBorderRate = 0.35;
            detectorParameters.MinOtsuStdDev = 5.0;
            detectorParameters.ErrorCorrectionRate = 0.6;

            // Словарь содержит все доступные маркеры
            Dictionary dictionary = CvAruco.GetPredefinedDictionary(PredefinedDictionaryName.Dict6X6_250);

            // Переменные для хранения результатов
            Point2f[][] corners;
            int[] ids;
            Point2f[][] rejectedImgPoints;

            // Преобразование изображения в оттенки серого
            Mat grayMat = new Mat();
            Cv2.CvtColor(sharpMat, grayMat, ColorConversionCodes.BGR2GRAY);

            // Детектирование маркеров
            CvAruco.DetectMarkers(grayMat, dictionary, out corners, out ids, detectorParameters, out rejectedImgPoints);


            // Преобразование обработанного изображения обратно в текстуру Unity
            output = Unity.MatToTexture(img, output);
            // SendImageToServer(output);
            // Установка текстуры для отображения результата
            rawImage.texture = output;

            return true;
        }

        private async void SendImageToServer(Texture2D texture) {
            // Кодируем текстуру в JPEG
            byte[] buffer;
            if (Cv2.ImEncode(".jpg", Unity.TextureToMat(texture), out buffer)) {
               Debug.Log("SendImageForDetection: " + buffer);
            } else {
                Debug.LogError("Failed to encode image to JPEG");
            }
        }

        private void SendImageForDetection(Mat img)
        {
            if (img.Empty()) {
                byte[] buffer;
                Debug.Log("SendImageForDetection: " + img);
                if (Cv2.ImEncode(".jpg", img, out buffer)) {
                        // Convert buffer to string for logging
                        string encodedImage = System.Convert.ToBase64String(buffer);
                        Debug.Log("SendImageForDetection: " + encodedImage);
                } else {
                        Debug.LogError("Failed to encode image to JPEG");
                }
			 }
            // Debug.Log("SendImageForDetection" +  img.ImEncode(".png"));
            // MatOfByte buffer = new MatOfByte();
            // Cv2.ImEncode(".jpg", img, out buffer, new int[] { (int)ImwriteFlags.JpegQuality, 90 });

            // Преобразование Mat в массив байтов
            // MatOfByte imageBytes = new MatOfByte();
            // // byte[] imageBytes;
            // // if (img != null) {
            // //     try
            // //     {
            //         bool success = Cv2.ImEncode(".jpeg", img, out imageBytes, new int[] { (int)ImwriteFlags.JpegQuality, 90 });
            //     }
            //     catch(Exception e)
            //     {
            //         Debug.Log("Detected object: " + e);
            //     }
            // }
        
           
            // if (success)
            // {
            //     // Отправка изображения на сервер
            //     var response = client.Detect(imageBytes);

            //     // Обработка ответа сервера
            //     if (response.Probability >= 0.9)
            //     {
            //         Debug.Log("Detected object: " + response.ClassName);
            //     }
            //     else
            //     {
            //         Debug.Log("No confident detection: " + response.ClassName);
            //     }
            // }
            // else
            // {
            //     Debug.LogError("Failed to encode image to PNG format.");
            // }
        }
    }
}

// namespace OpenCvSharp.Demo {

// 	using UnityEngine;
// 	using System.Collections;
// 	using UnityEngine.UI;
// 	using Aruco;
// 	using OpenCvSharp;

// 	public class MarkerDetector : WebCamera
// 	{
// 		protected override void Awake()
// 		{
// 			base.Awake();
// 			this.forceFrontalCamera = true;
// 		}

// 		// Our sketch generation function
// 		protected override bool ProcessTexture(WebCamTexture input, ref Texture2D output)
// 		{
// 			Mat img = Unity.TextureToMat(input, TextureParameters);

// 			//Convert image to grayscale
// 			Mat imgGray = new Mat ();
// 			Cv2.CvtColor (img, imgGray, ColorConversionCodes.BGR2GRAY);
			
// 			// Clean up image using Gaussian Blur
// 			Mat imgGrayBlur = new Mat ();
// 			Cv2.GaussianBlur (imgGray, imgGrayBlur, new Size (5, 5), 0);

// 			//Extract edges
// 			Mat cannyEdges = new Mat ();
// 			Cv2.Canny (imgGrayBlur, cannyEdges, 10.0, 70.0);

// 			//Do an invert binarize the image
// 			Mat mask = new Mat ();
// 			Cv2.Threshold (cannyEdges, mask, 70.0, 255.0, ThresholdTypes.BinaryInv);

// 			// result, passing output texture as parameter allows to re-use it's buffer
// 			// should output texture be null a new texture will be created
// 			output = Unity.MatToTexture(mask, output);
// 			return true;
// 		}
// 	}

	// public class MarkerDetector : MonoBehaviour {

	// 	public Texture2D texture;

	// 	void Start () {
	// 		// Create default parameres for detection
	// 		DetectorParameters detectorParameters = DetectorParameters.Create();

	// 		// Dictionary holds set of all available markers
	// 		Dictionary dictionary = CvAruco.GetPredefinedDictionary (PredefinedDictionaryName.Dict6X6_250);

	// 		// Variables to hold results
	// 		Point2f[][] corners;
	// 		int[] ids;
	// 		Point2f[][] rejectedImgPoints;

	// 		// Create Opencv image from unity texture
	// 		Mat mat = Unity.TextureToMat (this.texture);

	// 		// Convert image to grasyscale
	// 		Mat grayMat = new Mat ();
	// 		Cv2.CvtColor (mat, grayMat, ColorConversionCodes.BGR2GRAY); 

	// 		// Detect and draw markers
	// 		CvAruco.DetectMarkers (grayMat, dictionary, out corners, out ids, detectorParameters, out rejectedImgPoints);
	// 		CvAruco.DrawDetectedMarkers (mat, corners, ids);

	// 		// Create Unity output texture with detected markers
	// 		Texture2D outputTexture = Unity.MatToTexture (mat);

	// 		// Set texture to see the result
	// 		RawImage rawImage = gameObject.GetComponent<RawImage> ();
	// 		rawImage.texture = outputTexture;
	// 	}
		
	// }
// }