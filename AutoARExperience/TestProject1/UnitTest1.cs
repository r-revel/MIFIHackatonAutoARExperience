using FAClient;
using FAClient.Dto;
using Newtonsoft.Json;
using System.Text;

namespace TestProject1;

[TestClass]
public class UnitTest1
{
    const string baseUrl = "http://127.0.0.1:8000";
    const string redWolfPath = "..\\..\\..\\data\\redwolf.jpg";


    [TestMethod]
    public void TestConcept()
    {
        string endpoint = "detect";
        var bData = File.ReadAllBytes(redWolfPath);
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


    [TestMethod]
    public void Test1()
    {
        var bData = File.ReadAllBytes(redWolfPath);
        var client = FA.GetClient(baseUrl);
        var result = client.Detect(bData, "redwolf.jpg");
    }
}