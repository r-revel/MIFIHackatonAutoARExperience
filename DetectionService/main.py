from ultralytics import YOLO
from fastapi import FastAPI
from fastapi.responses import HTMLResponse
from base64 import b64decode
from base64 import b64encode 
from pydantic import BaseModel, Field
import numpy as np
import cv2

app = FastAPI()
model = YOLO("yolov8s.pt")  

@app.get("/")
async def root():
    return HTMLResponse(content=f"""
<h2>Detection service ready!</h2>
<a href="/docs" target="_blank">Swagger UI</a>
""")

class DetectionResponce(BaseModel):
    result: bool
    description: str
    
class Image(BaseModel):
    b64Value: str
    name: str

@app.post("/detect/", response_model=DetectionResponce) 
async def detect(data: Image)-> DetectionResponce:
    description = ''
    result = False
    img_bytes  = b64decode(data.b64Value, altchars=None, validate=False)
    img_array = np.frombuffer(img_bytes, np.uint8)
    img = cv2.imdecode(img_array, cv2.IMREAD_COLOR)  
    results = model.predict(img) 
    return DetectionResponce(result=result, description=description)
