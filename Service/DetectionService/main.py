from ultralytics import YOLO
from fastapi import FastAPI
from fastapi.responses import HTMLResponse
from pydantic import BaseModel
import numpy as np
import cv2

app = FastAPI()
model = YOLO("yolov8_Cars.pt")


# service url http://127.0.0.1:8000/
@app.get("/")
async def root():
    return HTMLResponse(content=f"""
<h2>Detection service ready!</h2>
<a href="/docs" target="_blank">Swagger UI</a>
""")

class DetectResponce(BaseModel):
    Result: bool
    Description: str
    
class DetectRequest(BaseModel):
    Data: bytes 
    Name: str

@app.post("/detect/", response_model=DetectResponce) 
async def detect(request: DetectRequest)-> DetectResponce:
    description = ''
    result = False
    img_array = np.frombuffer(request.Data, np.uint8)
    img = cv2.imdecode(img_array, cv2.IMREAD_COLOR)  
    results = model.predict(img) 
    return DetectResponce(Result=result, Description=description)
