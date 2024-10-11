using Core;
using Newtonsoft.Json;

namespace WebApiNET.Models;

public class RouteType:ObservableObject
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("type")] public string Name { get; set; } = "Не определен";

    [JsonProperty("color")]
    public string Color
    {
        get=>GetOrCreate("#FFFFFF"); 
        set=>SetAndNotify(value);
    }

}