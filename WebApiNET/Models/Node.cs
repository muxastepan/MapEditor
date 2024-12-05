using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Newtonsoft.Json;

namespace NavigationApp.Models
{
    /// <summary>
    /// Модель точки.
    /// </summary>
    public class Node
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("point")]
        public NaviPoint Point { get; set; }

        /// <summary>
        /// ID точек соседей.
        /// </summary>
        [JsonProperty("nodes")] 
        public List<int> NeighborsKeys { get; set; } = new();

        /// <summary>
        /// Точки соседи.
        /// </summary>
        [JsonIgnore]
        public List<Node> Neighbors { get; set; } = new();

        /// <summary>
        /// Преобразует ID точек соседей в модели точек.
        /// </summary>
        public void GetNeighbors(IEnumerable<Node> nodes)
        {
            Neighbors.AddRange(nodes.Where(node=>NeighborsKeys.Contains(node.Id)));
        }
    }
}
