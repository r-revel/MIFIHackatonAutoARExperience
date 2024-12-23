namespace TestProject;

[TestClass]
public class UnitTests
{
    private const string baseUrl = "http://127.0.0.1:8000";
    private const string redWolfPath = "..\\..\\..\\data\\redwolf.jpg";
    private readonly IFAClient client = FA.GetClient(baseUrl);

    [TestMethod]
    public void Test1()
    {
        var responce = client.Detect(File.ReadAllBytes(redWolfPath), "redwolf.jpg");
    }
}