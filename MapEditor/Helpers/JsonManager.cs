using System.IO;
using Newtonsoft.Json;

namespace MapEditor.Helpers
{
    public class JsonManager<T> where T : new()
    {
        public JsonManager(string path)
        {
            Path = path;
        }
        public string Path;
        protected T DefaultValue = new T();
        public T Read()
        {
            if (File.Exists(Path))
            {
                return JsonConvert.DeserializeObject<T>(File.ReadAllText(Path));
            }
            File.WriteAllText(Path, JsonConvert.SerializeObject(DefaultValue, Formatting.Indented));
            return DefaultValue;
        }

        public void Write(T value)
        {
            File.WriteAllText(Path, JsonConvert.SerializeObject(value, Formatting.Indented));
        }
    }
}
