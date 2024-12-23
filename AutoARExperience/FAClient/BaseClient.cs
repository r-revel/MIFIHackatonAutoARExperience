namespace FAClient;

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
        var json = JsonConvert.SerializeObject(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        using HttpClient client = new();
        var response = client.PostAsync($"{baseUrl}/{endPoint}/", content).GetAwaiter().GetResult();
        var responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        return response.IsSuccessStatusCode && !string.IsNullOrWhiteSpace(responseContent) ?
            (T)JsonConvert.DeserializeObject(responseContent!, typeof(T))! :
            throw new Exception(@$"Ошибка выполнения запроса: {response.StatusCode}");
    }
}
