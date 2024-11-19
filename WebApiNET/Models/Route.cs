using Newtonsoft.Json;

namespace WebApiNET.Models;

public class Route
{
    [JsonProperty("points")] public List<NaviPoint> Points { get; set; } = new();
    [JsonProperty("distance")] public double Distance { get; set; }

    private DateTime _time;
    [JsonProperty("time")]
    public DateTime Time
    {
        get=>_time;
        set => _time = TimeZoneInfo.ConvertTime(value, TimeZoneInfo.Utc);
    }
}
    