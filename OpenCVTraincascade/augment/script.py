import cv2
import numpy as np
import os

# Глобальные переменные для хранения координат и изображения
drawing = False
ix, iy = -1, -1
rects = []


def draw_rectangle(event, x, y, flags, param):
    global ix, iy, drawing, rects
    if event == cv2.EVENT_LBUTTONDOWN:
        drawing = True
        ix, iy = x, y
    elif event == cv2.EVENT_MOUSEMOVE:
        if drawing:
            img_copy = img.copy()
            cv2.rectangle(img_copy, (ix, iy), (x, y), (0, 255, 0), 2)
            cv2.imshow('Image', img_copy)
    elif event == cv2.EVENT_LBUTTONUP:
        drawing = False
        cv2.rectangle(img, (ix, iy), (x, y), (0, 255, 0), 2)
        rects.append((ix, iy, x - ix, y - iy))
        cv2.imshow('Image', img)


def augment_image(image, output_dir, base_name, positive=True):
    # Повороты
    augmented_images = []
    for angle in [0, 90, 180, 270]:
        rotated = cv2.rotate(image, cv2.ROTATE_90_CLOCKWISE * (angle // 90))
        filename = f'{output_dir}/{base_name}_rot{angle}.jpg'
        cv2.imwrite(filename, rotated)
        augmented_images.append(filename)
    # Отражения
    flipped_h = cv2.flip(image, 1)
    flipped_v = cv2.flip(image, 0)
    filename_h = f'{output_dir}/{base_name}_flip_h.jpg'
    filename_v = f'{output_dir}/{base_name}_flip_v.jpg'
    cv2.imwrite(filename_h, flipped_h)
    cv2.imwrite(filename_v, flipped_v)
    augmented_images.extend([filename_h, filename_v])
    # Масштабирование
    scale_factors = [0.5, 1.5]
    for scale in scale_factors:
        scaled = cv2.resize(image, None, fx=scale, fy=scale,
                            interpolation=cv2.INTER_LINEAR)
        filename = f'{output_dir}/{base_name}_scale{scale}.jpg'
        cv2.imwrite(filename, scaled)
        augmented_images.append(filename)
    # Изменение яркости и контраста
    brightness_factors = [0.5, 1.5]
    contrast_factors = [0.5, 1.5]
    for brightness in brightness_factors:
        for contrast in contrast_factors:
            adjusted = np.clip(image * contrast + brightness *
                               127, 0, 255).astype(np.uint8)
            filename = f'{output_dir}/{base_name}_brightness{brightness}_contrast{contrast}.jpg'
            cv2.imwrite(filename, adjusted)
            augmented_images.append(filename)
    return augmented_images


def check_img(img):
    # Проверка существования файла
    if not os.path.exists(img):
        print(f"Файл не найден: {img}")
    else:
        # Чтение изображения
        image = cv2.imread(img)
        # Проверка, успешно ли прочитано изображение
        if image is None:
            print("Ошибка: изображение не может быть прочитано.")
        else:
            print("Изображение успешно прочитано.")
            # Продолжение выполнения скрипта
            print("Скрипт продолжается после закрытия окна с изображением.")
            return image
    return None


def create_sample_files(positive_images, negative_images, positive_samples_file, negative_samples_file, positive_rects):
    # with open(positive_samples_file, 'w') as f:
    #     for img, rects in zip(positive_images, positive_rects):
    #         f.write(f"{img} {len(rects)}")
    #         for rect in rects:
    #             f.write(f" {rect[0]} {rect[1]} {rect[2]} {rect[3]}")
    #         f.write("\n")
    with open(positive_samples_file, 'w') as f:
        for img in positive_images:
            f.write(f"{img}\n")
    with open(negative_samples_file, 'w') as f:
        for img in negative_images:
            f.write(f"{img}\n")


def annotate_image(image, base_name):
    global img, rects
    img = image.copy()
    rects = []
    cv2.namedWindow('Image')
    cv2.setMouseCallback('Image', draw_rectangle)
    while True:
        cv2.imshow('Image', img)
        key = cv2.waitKey(1) & 0xFF
        if key == ord('q'):
            break
    cv2.destroyAllWindows()
    return rects


input_image_positive = f'{os.getcwd()}/OpenCVTraincascade/augment/input/positive.jpg'
output_dir_positive = f'{os.getcwd()}/OpenCVTraincascade/augment/augmented_positive'
base_name_positive = 'positive'
input_image_negative = f'{os.getcwd()}/OpenCVTraincascade/augment/input/negative.jpg'
output_dir_negative = f'{os.getcwd()}/OpenCVTraincascade/augment/augmented_negative'
base_name_negative = 'negative'
positive_samples_file = f'{os.getcwd()}/OpenCVTraincascade/augment/positive_samples.txt'
negative_samples_file = f'{os.getcwd()}/OpenCVTraincascade/augment/negative_samples.txt'

positive_rects = []

# Проверка и аугментация положительного изображения
image_positive = check_img(input_image_positive)
if image_positive is not None:
    # positive_rects = [annotate_image(image_positive, base_name_positive)]
    positive_images = augment_image(
        image_positive, output_dir_positive, base_name_positive, positive=True)

# Проверка и аугментация отрицательного изображения
image_negative = check_img(input_image_negative)
if image_negative is not None:
    negative_images = augment_image(
        image_negative, output_dir_negative, base_name_negative, positive=False)

# # Создание файлов с описанием образцов
if positive_images and negative_images:
    create_sample_files(positive_images, negative_images,
                        positive_samples_file, negative_samples_file, positive_rects)

# # Путь к выходному файлу с образцами
# vec_file = 'positive_samples.vec'
# # Размеры образцов
# width = 24
# height = 24
# # Чтение аннотаций
# with open(positive_samples_file, 'r') as f:
#     lines = f.readlines()

# # Создание списка образцов
# samples = []
# for line in lines:
#     parts = line.strip().split()
#     image_path = parts[0]
#     num_objects = int(parts[1])
#     for i in range(num_objects):
#         x, y, w, h = map(int, parts[2 + i * 4:2 + (i + 1) * 4])
#         image = cv2.imread(image_path)
#         sample = image[y:y + h, x:x + w]
#         sample = cv2.resize(sample, (width, height))
#         samples.append(sample)

# # Сохранение образцов в vec-файл
# with open(vec_file, 'wb') as f:
#     f.write(cv2.UMat.get(samples).get().tobytes())

# print(f"Создан файл с образцами: {vec_file}")
