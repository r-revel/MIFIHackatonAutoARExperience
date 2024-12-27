namespace OpenCvSharp.Demo {
using UnityEngine;
using UnityEngine.UI;
using OpenCvSharp;
using OpenCvSharp.Aruco;

public class MarkerDetector : WebCamera
{
    public RawImage rawImage; // Ссылка на RawImage для отображения результата

    protected override void Awake()
    {
        base.Awake();
        this.forceFrontalCamera = true;
    }

    protected override bool ProcessTexture(WebCamTexture input, ref Texture2D output)
    {
        Mat img = Unity.TextureToMat(input, TextureParameters);

        // Create default parameters for detection
        DetectorParameters detectorParameters = DetectorParameters.Create();
        // Dictionary holds set of all available markers
        Dictionary dictionary = CvAruco.GetPredefinedDictionary(PredefinedDictionaryName.Dict6X6_250);
        // Variables to hold results
        Point2f[][] corners;
        int[] ids;
        Point2f[][] rejectedImgPoints;

        // Convert image to grayscale
        Mat grayMat = new Mat();
        Cv2.CvtColor(img, grayMat, ColorConversionCodes.BGR2GRAY);

        // Detect markers
        CvAruco.DetectMarkers(grayMat, dictionary, out corners, out ids, detectorParameters, out rejectedImgPoints);

        // Draw detected markers
        if (ids != null && ids.Length > 0)
        {
						Debug.Log("Hello: " + ids);
            CvAruco.DrawDetectedMarkers(img, corners, ids);
        }

        // Convert the processed image back to a Unity texture
        output = Unity.MatToTexture(img, output);

        // Set texture to see the result
        rawImage.texture = output;

        return true;
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