namespace TestProject;

/// <summary>
/// Класс с юнит-тестами для проверки работоспособности приложения
/// </summary>
[TestClass]
public class UnitTests
{
    /// <summary>
    /// инициализация констант
    /// </summary>
    private const string baseUrl = "http://127.0.0.1:8000";
    private const string redWolfPath = "..\\..\\..\\data\\redwolf.jpg";
    private const string vaz   = "..\\..\\..\\..\\..\\Service\\DS\\data\\test\\images\\0_10.png";
    private const string uaz   = "..\\..\\..\\..\\..\\Service\\DS\\data\\test\\images\\1_10.png";
    private const string vesta = "..\\..\\..\\..\\..\\Service\\DS\\data\\test\\images\\2_10.png";
    /// <summary>
    /// инициализация экземпляра клиента
    /// </summary>
    private readonly IFAClient client = FA.GetClient(baseUrl);

    /// <summary>
    /// проверка ошибочного изображения
    /// </summary>
    /// <exception cref="Exception">Возникает в случае ошибки детекции</exception>
    [TestMethod]
    public void TestNonCar()
    {
        //проводим замер времени выполнения теста
        Stopwatch sw = Stopwatch.StartNew();
        // выполняем обращение на сервер
        var responce = client.Detect(File.ReadAllBytes(redWolfPath));
        //завершаем измерение времени
        sw.Stop();
        // выводим информацию в консоль
        Trace.WriteLine(@$"
{nameof(TestNonCar)}: {responce}
Duration: {sw.ElapsedMilliseconds} msec
");
        // проверяем результат и вызываем исключение в случае ошибки
        if (responce.Probability >= 0.9)
            throw new Exception("Ошибка предсказания!");
    }

    /// <summary>
    /// проверка изображения VAZ
    /// </summary>
    /// <exception cref="Exception">Возникает в случае ошибки детекции</exception>
    [TestMethod]
    public void TestVaz()
    {
        //проводим замер времени выполнения теста
        Stopwatch sw = Stopwatch.StartNew();
        // выполняем обращение на сервер
        var responce = client.Detect(File.ReadAllBytes(vaz));
        //завершаем измерение времени
        sw.Stop();
        // выводим информацию в консоль
        Trace.WriteLine(@$"
{nameof(TestVaz)}: {responce}
Duration: {sw.ElapsedMilliseconds} msec
");
        // проверяем результат и вызываем исключение в случае ошибки
        if (responce.Probability < 0.9 || responce.ClassName != "VAZ")
            throw new Exception("Ошибка предсказания!");
    }

    /// <summary>
    /// проверка изображения UAZ
    /// </summary>
    /// <exception cref="Exception">Возникает в случае ошибки детекции</exception>
    [TestMethod]
    public void TestUaz()
    {
        //проводим замер времени выполнения теста
        Stopwatch sw = Stopwatch.StartNew();
        // выполняем обращение на сервер
        var responce = client.Detect(File.ReadAllBytes(uaz));
        sw.Stop();
        // выводим информацию в консоль
        Trace.WriteLine(@$"
{nameof(TestUaz)}: {responce}
Duration: {sw.ElapsedMilliseconds} msec
");
        // проверяем результат и вызываем исключение в случае ошибки
        if (responce.Probability < 0.9 || responce.ClassName != "UAZ")
            throw new Exception("Ошибка предсказания!");
    }

    /// <summary>
    /// проверка изображения VESTA
    /// </summary>
    /// <exception cref="Exception">Возникает в случае ошибки детекции</exception>
    [TestMethod]
    public void TestVesta()
    {
        //проводим замер времени выполнения теста
        Stopwatch sw = Stopwatch.StartNew();
        // выполняем обращение на сервер
        var responce = client.Detect(File.ReadAllBytes(vesta));
        sw.Stop();
        // выводим информацию в консоль
        Trace.WriteLine(@$"
{nameof(TestVesta)}: {responce}
Duration: {sw.ElapsedMilliseconds} msec
");
        // проверяем результат и вызываем исключение в случае ошибки
        if (responce.Probability < 0.9 || responce.ClassName != "VESTA")
            throw new Exception("Ошибка предсказания!");
    }
}
