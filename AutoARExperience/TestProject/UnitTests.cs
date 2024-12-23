namespace TestProject;

[TestClass]
public class UnitTests
{
    const string baseUrl = "http://127.0.0.1:8000";
    const string redWolfPath = "..\\..\\..\\data\\redwolf.jpg";

    [TestMethod]
    public void Test1()
    {
        var bData = File.ReadAllBytes(redWolfPath);
        var client = FA.GetClient(baseUrl);
        var result = client.Detect(bData, "redwolf.jpg");
    }
}