using NavigationApp.Models;
using Newtonsoft.Json;

namespace WebApiNET.Models;

public class NodeCreate
{
    [JsonProperty("point")]
    public NaviPoint Point { get; set; }

    [JsonProperty("nodes")]
    public List<int> NeighborsKeys { get; set; } = new();

    [JsonProperty("types")] public List<int> RouteTypes { get; set; } = new();
}