using FAClient.Dto;

namespace FAClient
{
    /// <summary>
    /// Интерфейс FastApi клиента 
    /// </summary>
    public interface IFAClient
    {
        /// <summary>
        /// Строка содержащия адрес и поционально порт сервера
        /// </summary>
        public string BaseUrl { get; }

        /// <summary>
        /// Метод осуществляющий классификацию изображения на сервер
        /// </summary>
        /// <param name="data">детектируемое изображения</param>
        /// <returns>Возвращает результат классификации изображения</returns>
        DetectResponce Detect(byte[] data);
    }
}
