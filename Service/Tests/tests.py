import unittest
import base64
import requests


class TestService(unittest.TestCase):
    '''
    Класс содержащий юнит-тесты используемые для тестирования проекта
    '''
    def test_service(self):
        '''
        тест-кейс для проверки работоспособности сервисного метода методами питона
        '''
        url = 'http://127.0.0.1:8000/detect/'
        # имя файла
        file_name = "redwolf.jpg"
        # переменная для хранения изображения в виде BASE64 строки
        b64_image = ''
        # считываем файл файл
        with open(file_name, "rb") as image_file:
            b64_image  = base64.b64encode(image_file.read()).decode("utf-8")
        # формируем запрос на сервис
        data = {
            "Data": b64_image,
        }
        response = requests.post(url, json=data)
        # отправляем запрос и получаем ответ
        data = response.content.decode("utf-8")
        print(data)

if __name__ == '__main__':
    unittest.main()