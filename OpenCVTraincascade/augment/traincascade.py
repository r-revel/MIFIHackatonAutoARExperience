import cv2

# Define the parameters for training
params = {
    "data": "OpenCVTraincascade/augment/cascade.xml",
    "vec": "OpenCVTraincascade/augment/positive_samples.vec",
    "bg": "OpenCVTraincascade/augment/negative_samples.txt",
    "numPos": 1000,
    "numNeg": 500,
    "numStages": 20,
    "precalcValBufSize": 1024,
    "precalcIdxBufSize": 1024,
    "featureType": "HAAR",
    "w": 24,
    "h": 24
}

# Train the cascade classifier
cv2.createTrainCascadeClassifier(params)
