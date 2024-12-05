using System.IO;
using Newtonsoft.Json;

namespace MapEditor.Helpers
{
    /// <summary>
    /// Менеджер JSON файлов.
    /// </summary>
    public class JsonManager<T> where T : new()
    {
        /// <summary>
        /// Конструктор менеджера JSON файлов.
        /// </summary>
        /// <param name="path">Относительный путь до файла</param>
        public JsonManager(string path)
        {
            Path = path;
        }
        public string Path;

        /// <summary>
        /// Значение по умолчанию.
        /// </summary>
        protected T DefaultValue = new T();

        /// <summary>
        /// Читает значение из JSON файла, если файла нет, то возвращает значение по умолчанию.
        /// </summary>
        public T Read()
        {
            if (File.Exists(Path))
            {
                return JsonConvert.DeserializeObject<T>(File.ReadAllText(Path));
            }
            File.WriteAllText(Path, JsonConvert.SerializeObject(DefaultValue, Formatting.Indented));
            return DefaultValue;
        }

        /// <summary>
        /// Записывает значение в JSON файл.
        /// </summary>
        public void Write(T value)
        {
            File.WriteAllText(Path, JsonConvert.SerializeObject(value, Formatting.Indented));
        }
    }
}
