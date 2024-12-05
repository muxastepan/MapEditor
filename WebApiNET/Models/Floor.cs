using Core;
using Newtonsoft.Json;
using WebApiNET.Utilities;

namespace NavigationApp.Models
{
    /// <summary>
    /// Модель этажа.
    /// </summary>
    public class Floor:ObservableObject
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("mapImage")]
        public string Image { get; set; }

        [JsonIgnore]
        public DisposableImage DisposableImage
        {
            get => GetOrCreate<DisposableImage>();
            set => SetAndNotify(value);
        } 

    }
}
