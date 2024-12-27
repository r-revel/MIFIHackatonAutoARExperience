import cv2
import numpy as np


def read_vec_file(filename, sample_width, sample_height):
    # Открываем файл
    with open(filename, 'rb') as f:
        # Читаем заголовок
        magic = f.read(4)
        if magic != b'\x00\x00\x00\x01':
            raise ValueError("Invalid .vec file format")
        num_samples = int.from_bytes(f.read(4), byteorder='little')
        # Читаем образцы
        samples = []
        for _ in range(num_samples):
            sample = f.read(sample_width * sample_height)
            sample = np.frombuffer(sample, dtype=np.uint8)
            sample = sample.reshape((sample_height, sample_width))
            samples.append(sample)
    return samples


def display_samples(samples, cols=10):
    # Определяем количество строк
    num_samples = len(samples)
    rows = (num_samples + cols - 1) // cols
    # Создаем пустое изображение для отображения всех образцов
    sample_height, sample_width = samples[0].shape
    result = np.zeros((rows * sample_height, cols *
                      sample_width), dtype=np.uint8)
    for i, sample in enumerate(samples):
        row = i // cols
        col = i % cols
        result[row * sample_height:(row + 1) * sample_height,
               col * sample_width:(col + 1) * sample_width] = sample
    # Отображаем результат
    cv2.imshow('Positive Samples', result)
    cv2.waitKey(0)
    cv2.destroyAllWindows()


# Путь к файлу .vec
vec_file = 'augment/positive_samples.vec'
# Размеры образцов (ширина и высота)
sample_width = 24
sample_height = 24

# Чтение образцов из файла
samples = read_vec_file(vec_file, sample_width, sample_height)
# Отображение образцов
display_samples(samples)
