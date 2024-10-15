using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Newtonsoft.Json;
using WebApiNET.Models;

namespace NavigationApp.Models
{
    public class Node
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("point")]
        public NaviPoint Point { get; set; }

        [JsonProperty("nodes")] 
        public List<int> NeighborsKeys { get; set; } = new();

        [JsonProperty("types")] public ObservableCollection<RouteType> RouteTypes { get; set; } = new();


        [JsonIgnore]
        public List<Node> Neighbors { get; set; } = new();

        public void GetNeighbors(IEnumerable<Node> nodes)
        {
            Neighbors.AddRange(nodes.Where(node=>NeighborsKeys.Contains(node.Id)));
        }
    }

    public static class NodeMappers
    {
        public static NodeCreate ToCreate(this Node node) => new()
        {
            NeighborsKeys = node.NeighborsKeys, Point = node.Point,
            RouteTypes = node.RouteTypes.Select(rt => rt.Id).ToList()
        };
    }
}
