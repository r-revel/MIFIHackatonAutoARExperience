from ultralytics import YOLO
from fastapi import FastAPI
from fastapi.responses import HTMLResponse
from pydantic import BaseModel
import numpy as np
import cv2
from base64 import b64decode
import threading
import torch
import uvicorn

# Инициализация FastAPI сревиса
app = FastAPI()
# Инициализация модели нейронной сети.
# Так как мы рассматриваем упрощенный сервис,
# у нас будет только одна нейросетевая модель одновременно обрабатывающая запросы.
# Инициализация модели происходит при старте приложения,
# для того чтобы не производить это при каждом запросе.
# Что позволит уменьшить время выполнения запроса
model = YOLO("yolov8_Cars.pt")
# инициализация критической секции для блокировки доступа к модели из разных потоков
syncObj = threading.Lock()
# Определение доступных вычислительных устройств и выбор по производительности
device = torch.device("cuda" if torch.cuda.is_available() else "cpu")
model.to(device)


@app.get("/")
async def root():
    '''
    Определяем endpoint по умолчанию доступный по адресу: http://127.0.0.1:8000/
    Использует веб-метод GET
    Он возвращает HTML ответ с небольшой информацией о состоянии сервиса.
    Это позволяет просмотреть ее в браузере без дополнительных программных средств.
    '''
    return HTMLResponse(content=f'''
<h2>Detection service ready!</h2>
<a href="/docs" target="_blank">Swagger UI</a><br><br>
Model is running on: {next(model.model.parameters()).device}<br><br>
Cuda is available: {torch.cuda.is_available() }
''')


class DetectResponce(BaseModel):
    '''
    Класс содержащий ответ вебметода detect
    '''

    #: Тип данных: float
    #: Вероятность принадлежности изображения к детектируемому классу
    Probability: float

    #: Тип данных: str
    #: Название класса к которому может принадлежать детектируемое изображение
    ClassName: str


class DetectRequest(BaseModel):
    '''
    Класс содержащий параметры вебметода detect
    '''

    #: Тип данных: str
    #: строка в кодировке BASE64 содержащая бинарные данные изображения
    Data: str


@app.post("/detect/", response_model=DetectResponce)
async def detect(request: DetectRequest) -> DetectResponce:
    '''
    Определяем endpoint detect доступный по адресу http://127.0.0.1:8000/detect/
    Использует веб-метод POST
    Метод определяет как какому классу отностися детектируемое изображение.
    Выбирается класс с максимальным соотсвествием
    '''

    # преобразуем строку в бинарный вид и загружаем в обьект изображения
    img_bytes = b64decode(request.Data, altchars=None, validate=False)
    img_array = np.frombuffer(img_bytes, np.uint8)
    img = cv2.imdecode(img_array, cv2.IMREAD_COLOR)
    # блокируем доступ к модели из других котоков
    with syncObj:
        # производим оценку изображения и затем освобождаем доступ
        result = model(img)[0]
    # в цикле по результату оценки, находим класс обьекта с максимальным соотвествием
    max_conf = 0
    max_class = None
    for box in result.boxes:
        conf = box.conf[0]
        if conf > max_conf:
            max_conf = conf
            max_class = int(box.cls[0])
    # получаем название класса
    class_name = model.names[max_class]
    # из тензора получаем скалярное значение метрики соотвествия
    probability = max_conf.item()
    # формируем ответ который будет отправлен на клиент
    return DetectResponce(Probability=probability, ClassName=class_name)


if __name__ == "__main__":
    uvicorn.run(app, host="127.0.0.1", port=8000)


if __name__ == "__main__":
    uvicorn.run(app, host="127.0.0.1", port=8000)

if __name__ == '__main__':
    uvicorn.run(app, host="0.0.0.0", port=8000)