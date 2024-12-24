namespace TestProject;

[TestClass]
public class UnitTests
{
    private const string baseUrl = "http://127.0.0.1:8000";
    private const string redWolfPath = "..\\..\\..\\data\\redwolf.png";
    private const string vaz   = "..\\..\\..\\..\\..\\Service\\DS\\data\\test\\images\\0_10.png";
    private const string uaz   = "..\\..\\..\\..\\..\\Service\\DS\\data\\test\\images\\1_10.png";
    private const string vesta = "..\\..\\..\\..\\..\\Service\\DS\\data\\test\\images\\2_10.png";
    private readonly IFAClient client = FA.GetClient(baseUrl);

    [TestMethod]
    public void TestNonCar()
    {
        var responce = client.Detect(File.ReadAllBytes(redWolfPath));
        Trace.WriteLine(@$"{nameof(TestNonCar)}: {responce}");
        if (responce.Probability >= 0.9)
            throw new Exception("Ошибка предсказания!");
    }

    [TestMethod]
    public void TestVaz()
    {
        var responce = client.Detect(File.ReadAllBytes(vaz));
        Trace.WriteLine(@$"{nameof(TestVaz)}: {responce}");
        if (responce.Probability < 0.9 || responce.ClassName != "VAZ")
            throw new Exception("Ошибка предсказания!");
    }

    [TestMethod]
    public void TestUaz()
    {
        var responce = client.Detect(File.ReadAllBytes(uaz));
        Trace.WriteLine(@$"{nameof(TestUaz)}: {responce}");
        if (responce.Probability < 0.9 || responce.ClassName != "UAZ")
            throw new Exception("Ошибка предсказания!");
    }

    [TestMethod]
    public void TestVesta()
    {
        var responce = client.Detect(File.ReadAllBytes(vesta));
        Trace.WriteLine(@$"{nameof(TestVesta)}: {responce}");
        if (responce.Probability < 0.9 || responce.ClassName != "VESTA")
            throw new Exception("Ошибка предсказания!");
    }
}
