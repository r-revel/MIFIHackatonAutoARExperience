namespace TestProject;

/// <summary>
/// ����� � ����-������� ��� �������� ����������������� ����������
/// </summary>
[TestClass]
public class UnitTests
{
    /// <summary>
    /// ������������� ��������
    /// </summary>
    private const string baseUrl = "http://127.0.0.1:8000";
    private const string redWolfPath = "..\\..\\..\\data\\redwolf.jpg";
    private const string vaz   = "..\\..\\..\\..\\..\\Service\\DS\\data\\test\\images\\0_10.png";
    private const string uaz   = "..\\..\\..\\..\\..\\Service\\DS\\data\\test\\images\\1_10.png";
    private const string vesta = "..\\..\\..\\..\\..\\Service\\DS\\data\\test\\images\\2_10.png";
    /// <summary>
    /// ������������� ���������� �������
    /// </summary>
    private readonly IFAClient client = FA.GetClient(baseUrl);

    /// <summary>
    /// �������� ���������� �����������
    /// </summary>
    /// <exception cref="Exception">��������� � ������ ������ ��������</exception>
    [TestMethod]
    public void TestNonCar()
    {
        //�������� ����� ������� ���������� �����
        Stopwatch sw = Stopwatch.StartNew();
        // ��������� ��������� �� ������
        var responce = client.Detect(File.ReadAllBytes(redWolfPath));
        //��������� ��������� �������
        sw.Stop();
        // ������� ���������� � �������
        Trace.WriteLine(@$"
{nameof(TestNonCar)}: {responce}
Duration: {sw.ElapsedMilliseconds} msec
");
        // ��������� ��������� � �������� ���������� � ������ ������
        if (responce.Probability >= 0.9)
            throw new Exception("������ ������������!");
    }

    /// <summary>
    /// �������� ����������� VAZ
    /// </summary>
    /// <exception cref="Exception">��������� � ������ ������ ��������</exception>
    [TestMethod]
    public void TestVaz()
    {
        //�������� ����� ������� ���������� �����
        Stopwatch sw = Stopwatch.StartNew();
        // ��������� ��������� �� ������
        var responce = client.Detect(File.ReadAllBytes(vaz));
        //��������� ��������� �������
        sw.Stop();
        // ������� ���������� � �������
        Trace.WriteLine(@$"
{nameof(TestVaz)}: {responce}
Duration: {sw.ElapsedMilliseconds} msec
");
        // ��������� ��������� � �������� ���������� � ������ ������
        if (responce.Probability < 0.9 || responce.ClassName != "VAZ")
            throw new Exception("������ ������������!");
    }

    /// <summary>
    /// �������� ����������� UAZ
    /// </summary>
    /// <exception cref="Exception">��������� � ������ ������ ��������</exception>
    [TestMethod]
    public void TestUaz()
    {
        //�������� ����� ������� ���������� �����
        Stopwatch sw = Stopwatch.StartNew();
        // ��������� ��������� �� ������
        var responce = client.Detect(File.ReadAllBytes(uaz));
        sw.Stop();
        // ������� ���������� � �������
        Trace.WriteLine(@$"
{nameof(TestUaz)}: {responce}
Duration: {sw.ElapsedMilliseconds} msec
");
        // ��������� ��������� � �������� ���������� � ������ ������
        if (responce.Probability < 0.9 || responce.ClassName != "UAZ")
            throw new Exception("������ ������������!");
    }

    /// <summary>
    /// �������� ����������� VESTA
    /// </summary>
    /// <exception cref="Exception">��������� � ������ ������ ��������</exception>
    [TestMethod]
    public void TestVesta()
    {
        //�������� ����� ������� ���������� �����
        Stopwatch sw = Stopwatch.StartNew();
        // ��������� ��������� �� ������
        var responce = client.Detect(File.ReadAllBytes(vesta));
        sw.Stop();
        // ������� ���������� � �������
        Trace.WriteLine(@$"
{nameof(TestVesta)}: {responce}
Duration: {sw.ElapsedMilliseconds} msec
");
        // ��������� ��������� � �������� ���������� � ������ ������
        if (responce.Probability < 0.9 || responce.ClassName != "VESTA")
            throw new Exception("������ ������������!");
    }
}
