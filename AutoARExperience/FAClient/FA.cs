namespace FAClient
{
    /// <summary>
    /// Статический класс обеспечивающий инстанцирование клиента
    /// </summary>
    public static class FA
    {
        /// <summary>
        /// Метод позволяющий вернуть создать версию клиента, соотвествующую текущей версии сервера
        /// </summary>
        /// <param name="baseUrl">адрес сервера</param>
        /// <returns>Возвращаем экземпляр FastApi клиента</returns>
        public static IFAClient GetClient(string baseUrl)
        {
            // в текущей реализации у нас только одна версия ))
            return new FAClientV1(baseUrl);
        }
    }
}
