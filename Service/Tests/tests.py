import unittest
import base64
import requests


class TestService(unittest.TestCase):

    def test_service(self):
        url = 'http://127.0.0.1:8000/detect/'
        file_name = "redwolf.jpg"
        b64_image = ''
        with open(file_name, "rb") as image_file:
            b64_image  = base64.b64encode(image_file.read()).decode("utf-8")
        data = {
            "Data": b64_image,
        }
        response = requests.post(url, json=data)
        data = response.content.decode("utf-8")
        print(data)

if __name__ == '__main__':
    unittest.main()