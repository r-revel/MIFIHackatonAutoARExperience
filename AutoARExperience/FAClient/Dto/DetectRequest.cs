namespace FAClient.Dto
{
    /// <summary>
    /// Класс содержащий параметры вебметода detect
    /// </summary>
    public class DetectRequest
    {
        /// <summary>
        /// строка в кодировке BASE64 содержащая бинарные данные изображения
        /// </summary>
        public string Data { get; set; } = "";
    }
}
