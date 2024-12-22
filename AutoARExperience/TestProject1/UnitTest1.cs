
using FAClient.Dto;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace TestProject1;

[TestClass]
public class UnitTest1
{
    string baseUrl = "http://127.0.0.1:8000";
    string endpoint = "detect";
    [TestMethod]
    public void TestConcept()
    {
        var path = "..\\..\\..\\data\\redwolf.jpg";
        var bData = File.ReadAllBytes(path);
        var sData = Convert.ToBase64String(bData);
        var fd = new FileData { b64Value = sData, name = "redwolf.jpg" };
        var json = JsonConvert.SerializeObject(fd);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        using HttpClient client = new();
        var response = client.PostAsync($"{baseUrl}/{endpoint}/", content).GetAwaiter().GetResult();
        var responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        if (response.IsSuccessStatusCode && !string.IsNullOrWhiteSpace(responseContent))
        {
            var detectionResponce = JsonConvert
                .DeserializeObject(responseContent!, typeof(DetectionResponce)) as DetectionResponce;
        }
        else
        {
            throw new Exception();
        }

    }
}