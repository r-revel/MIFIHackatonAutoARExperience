using System.Net.Http;
using System.Text;
using System;
using System.Text.Json;

namespace FAClient
{

    internal class BaseClient
    {
        internal BaseClient(string baseUrl)
        {
            this.baseUrl = baseUrl;
        }

        private string baseUrl;

        public string BaseUrl => baseUrl;

        internal T Post<T>(string endPoint, object data) where T : new()
        {
            
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            using HttpClient client = new HttpClient();
            var response = client.PostAsync($"{baseUrl}/{endPoint}/", content).GetAwaiter().GetResult();
            var responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return response.IsSuccessStatusCode && !string.IsNullOrWhiteSpace(responseContent) ?
                (T)JsonSerializer.Deserialize(responseContent!, typeof(T))! :
                throw new Exception(@$"Ошибка выполнения запроса: {response.StatusCode}");
        }
    }
}