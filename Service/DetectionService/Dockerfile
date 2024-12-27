# Используем официальный Python образ в качестве базового
FROM python:3.10-slim

# Устанавливаем рабочую директорию
WORKDIR /app

# Копируем файлы проекта
COPY main.py requirements_docker.txt yolov8_Cars.pt ./

# Устанавливаем системные библиотеки
RUN apt-get update && apt-get install -y \
    libgl1-mesa-glx \
    libglib2.0-0 \
    && rm -rf /var/lib/apt/lists/*

# Обновляем pip и устанавливаем зависимости
RUN pip install --no-cache-dir --upgrade pip \
    && pip install --no-cache-dir -r requirements.txt

# Указываем команду запуска приложения
CMD ["python", "main.py"]
