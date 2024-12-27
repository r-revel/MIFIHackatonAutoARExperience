namespace OpenCvSharp.Demo
{
	using UnityEngine;
	using UnityEngine.EventSystems;
	using System;
	using OpenCvSharp;
	using OpenCvSharp.Tracking;

	/// <summary>
	/// Object tracking handler
	/// </summary>
	public class TrackingScript : WebCamera, IPointerClickHandler
	{
			// downscaling const
			const float downScale = 0.33f;
			const float minimumAreaDiagonal = 25.0f;

			// tracker
			Size frameSize = Size.Zero;
			Tracker tracker = null;

			// 3D object to track
			public GameObject objectToTrack;
			private GameObject trackedObjectInstance;

			/// <summary>
			/// Initialization
			/// </summary>
			protected override void Awake()
			{
					base.Awake();
					forceFrontalCamera = true;
			}

			/// <summary>
			/// Converts point from screen space into the image space
			/// </summary>
			/// <param name="coord"></param>
			/// <param name="size"></param>
			/// <returns></returns>
			Vector2 ConvertToImageSpace(Vector2 coord, Size size)
			{
					var ri = GetComponent<UnityEngine.UI.RawImage>();

					Vector2 output = new Vector2();
					RectTransformUtility.ScreenPointToLocalPointInRectangle(ri.rectTransform, coord, null, out output);

					// pivot is in the center of the rectTransform, we need { 0, 0 } origin
					output.x += size.Width / 2;
					output.y += size.Height / 2;

					// now our image might have various transformations of its own
					if (!TextureParameters.FlipVertically)
							output.y = size.Height - output.y;

					// downscaling
					output.x *= downScale;
					output.y *= downScale;

					return output;
			}

			/// <summary>
			/// Our main function to process the tracking:
			/// 1. If there is no active tracker - it does nothing useful, just renders the image
			/// 2. If there is an active tracker - it draws image with tracked object rect over it (green color)
			/// </summary>
			/// <param name="input"></param>
			/// <param name="output"></param>
			/// <returns></returns>
			protected override bool ProcessTexture(WebCamTexture input, ref Texture2D output)
			{
					Mat image = Unity.TextureToMat(input, TextureParameters);
					Mat downscaled = image.Resize(Size.Zero, downScale, downScale);

					Rect2d obj = Rect2d.Empty;

					if (tracker != null)
					{
							if (frameSize.Height != 0 && frameSize.Width != 0 && downscaled.Size() != frameSize)
							{
									DropTracking();
							}
							else
							{
									if (!tracker.Update(downscaled, ref obj))
									{
											obj = Rect2d.Empty;
									}

									if (obj.Width != 0 && obj.Height != 0)
									{
											var areaRect = new OpenCvSharp.Rect((int)obj.X, (int)obj.Y, (int)obj.Width, (int)obj.Height);
											Cv2.Rectangle((InputOutputArray)image, areaRect * (1.0f / downScale), Scalar.LightGreen);

											// Update the position of the 3D object
											if (trackedObjectInstance != null)
											{
													Vector2 trackedPoint = new Vector2((float)obj.X + (float)obj.Width / 2, (float)obj.Y + (float)obj.Height / 2);
													Vector2 worldPoint = ConvertToImageSpace(trackedPoint, new Size(webCamTexture.width, webCamTexture.height));
													worldPoint /= downScale;

													// Convert to world coordinates
													Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(worldPoint.x, worldPoint.y, Camera.main.nearClipPlane));
													trackedObjectInstance.transform.position = worldPosition;
											}
									}
							}
					}

					output = Unity.MatToTexture(image, output);
					return true;
			}

			/// <summary>
			/// Frees object tracker
			/// </summary>
			protected void DropTracking()
			{
					if (tracker != null)
					{
							tracker.Dispose();
							tracker = null;

							// Destroy the tracked object instance
							if (trackedObjectInstance != null)
							{
									Destroy(trackedObjectInstance);
									trackedObjectInstance = null;
							}
					}
			}

			/// <summary>
			/// Initializes tracking when the user clicks on the screen
			/// </summary>
			/// <param name="eventData"></param>
			public void OnPointerClick(PointerEventData eventData)
			{
					DropTracking();

					Vector2 clickPoint = ConvertToImageSpace(eventData.position, new Size(webCamTexture.width, webCamTexture.height));

					// Create a small area around the click point
					int areaSize = 5; // Size of the area to track
					Point location = new Point((int)clickPoint.x - areaSize / 2, (int)clickPoint.y - areaSize / 2);
					Size size = new Size(areaSize, areaSize);
					var areaRect = new OpenCvSharp.Rect(location, size);

					Mat downscaled = Unity.TextureToMat(webCamTexture, TextureParameters).Resize(Size.Zero, downScale, downScale);
					Rect2d obj = new Rect2d(areaRect.X, areaRect.Y, areaRect.Width, areaRect.Height);

					tracker = Tracker.Create(TrackerTypes.MedianFlow);
					tracker.Init(downscaled, obj);

					frameSize = downscaled.Size();

					// Create and position the 3D object
					if (objectToTrack != null)
					{
							trackedObjectInstance = Instantiate(objectToTrack, Camera.main.ScreenToWorldPoint(eventData.position + Vector2.down * (float)Camera.main.nearClipPlane), Quaternion.identity);
					}
			}
	}
}