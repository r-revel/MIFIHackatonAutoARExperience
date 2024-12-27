using FAClient.Dto;
using System;

namespace FAClient
{
    /// <summary>
    /// Реализация FastApi клиента версии 1
    /// </summary>
    internal class FAClientV1 : BaseClient, IFAClient
    {
        /// <summary>
        /// конструктор клиента
        /// </summary>
        /// <param name="baseUrl">адрес сервера</param>
        public FAClientV1(string baseUrl) : base(baseUrl) { }

        /// <summary>
        /// Метод осуществляющий классификацию изображения на сервер
        /// </summary>
        /// <param name="data">детектируемое изображения</param>
        /// <returns>Возвращает результат классификации изображения</returns>
        public DetectResponce Detect(byte[] data) =>
            Post<DetectResponce>("detect", new DetectRequest() { Data = Convert.ToBase64String(data), });
    }
}
