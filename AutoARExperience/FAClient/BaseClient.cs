using Newtonsoft.Json;
using System.Text;

namespace FAClient;

internal class BaseClient
{
    internal BaseClient(string baseUrl)
    {
        this.baseUrl = baseUrl;
    }

    private string baseUrl;

    public string BaseUrl => baseUrl;

    internal T Post<T>(string endPoint, object data)
    {
        var json = JsonConvert.SerializeObject(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        using HttpClient client = new();
        var response = client.PostAsync($"{baseUrl}/{endPoint}/", content).GetAwaiter().GetResult();
        var responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        if (response.IsSuccessStatusCode && !string.IsNullOrWhiteSpace(responseContent))
        {
            var detectionResponce = (T)JsonConvert
                .DeserializeObject(responseContent!, typeof(T));
            return detectionResponce;
        }
        else
        {
            throw new Exception();
        }
    }
}
