from ultralytics import YOLO
from fastapi import FastAPI
from fastapi.responses import HTMLResponse
from pydantic import BaseModel
import numpy as np
import cv2
from base64 import b64decode
import threading
import torch


app = FastAPI()
model = YOLO("yolov8_Cars.pt")
device = torch.device("cuda" if torch.cuda.is_available() else "cpu")
model.to(device)
syncObj = threading.Lock()

# service url http://127.0.0.1:8000/
@app.get("/")
async def root():
    return HTMLResponse(content=f"""
<h2>Detection service ready!</h2>
<a href="/docs" target="_blank">Swagger UI</a><br><br>
Model is running on: {next(model.model.parameters()).device}<br><br>
Cuda is available: {torch.cuda.is_available() }
""")

class DetectResponce(BaseModel):
    Probability: float
    ClassName: str
    
class DetectRequest(BaseModel):
    Data: str

@app.post("/detect/", response_model=DetectResponce) 
async def detect(request: DetectRequest)-> DetectResponce:
    img_bytes  = b64decode(request.Data, altchars=None, validate=False)
    img_array = np.frombuffer(img_bytes, np.uint8)
    img = cv2.imdecode(img_array, cv2.IMREAD_COLOR)  
    with syncObj:
        result = model(img)[0]
    max_conf = 0
    max_class = None
    for box in result.boxes:
        conf = box.conf[0]
        if conf > max_conf:
            max_conf = conf
            max_class = int(box.cls[0])
    class_name = model.names[max_class]
    probability = max_conf.item()
    result = result.names
    return DetectResponce(Probability=probability, ClassName=class_name)
