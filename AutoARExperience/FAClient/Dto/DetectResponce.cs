using System.Text.Json;

namespace FAClient.Dto
{
    /// <summary>
    /// Класс содержащий ответ вебметода detect
    /// </summary>
    public class DetectResponce
    {
        /// <summary>
        /// Вероятность принадлежности изображения к детектируемому классу 
        /// </summary>
        public float Probability { get; set; } = 0;

        /// <summary>
        /// Название класса к которому может принадлежать детектируемое изображение
        /// </summary>
        public string ClassName { get; set; } = "";

        /// <summary>
        /// Текстовое представление обьекта
        /// </summary>
        /// <returns>Возвращает текстовое представление обьекта</returns>

        public override string ToString() => JsonSerializer.Serialize(this);
    }
}
