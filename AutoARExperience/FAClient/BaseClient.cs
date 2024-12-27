using System.Net.Http;
using System.Text;
using System;
using System.Text.Json;

namespace FAClient
{
    /// <summary>
    /// Базовый класс FastApi клиента 
    /// </summary>
    internal class BaseClient
    {
        /// <summary>
        /// Конструктор клиента
        /// </summary>
        /// <param name="baseUrl">Строка содержащия адрес и поционально порт сервера</param>
        internal BaseClient(string baseUrl)
        {
            this.baseUrl = baseUrl;
        }

        private string baseUrl;

        /// <summary>
        /// Строка содержащия адрес и поционально порт сервера
        /// </summary>
        public string BaseUrl => baseUrl;

        /// <summary>
        /// Метод реализующий запрос на сервер
        /// </summary>
        /// <typeparam name="T">Параметр шаблона определяющий тип возвращаемого значения</typeparam>
        /// <param name="endPoint">Эндпоинт вебметода</param>
        /// <param name="data">обьект передаваемый на сервер</param>
        /// <returns>Обьект возвращаемый сервером</returns>
        /// <exception cref="Exception">Генерируется исключение в случае если от сервера нет возвращаемых данных</exception>
        internal T Post<T>(string endPoint, object data) where T : new()
        {
            // сериалищуем обьект в Json
            var json = JsonSerializer.Serialize(data);
            // формируем тело html запроса
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            // создаем клиент для обращения к серверу
            using HttpClient client = new HttpClient();
            // выполняем обращение к серверу
            var response = client.PostAsync($"{baseUrl}/{endPoint}/", content).GetAwaiter().GetResult();
            // получаем ответ сервера
            var responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            // сериализуем в обьект и возвращаем его
            return response.IsSuccessStatusCode && !string.IsNullOrWhiteSpace(responseContent) ?
                (T)JsonSerializer.Deserialize(responseContent!, typeof(T))! :
                throw new Exception(@$"Ошибка выполнения запроса: {response.StatusCode}");
        }
    }
}
