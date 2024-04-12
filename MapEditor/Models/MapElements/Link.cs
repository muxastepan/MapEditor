using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using NavigationApp.Models;
using WebApiNET;

namespace MapEditor.Models.MapElements
{
    public class Link:MapElement
    {
        public VisualNode? From { get; set; }
        public VisualNode? To { get; set; }


        public override async Task Delete()
        {
            var from = From.Node;
            var to = To.Node;

            from.Neighbors.Remove(to);
            to.Neighbors.Remove(from);

            from.NeighborsKeys.Remove(to.Id);
            to.NeighborsKeys.Remove(from.Id);

            await WebApi.UpdateData<Node>(from, from.Id.ToString());
            await WebApi.UpdateData<Node>(to, to.Id.ToString());
        }

        

        public static async Task LinkNodes(VisualNode firstNode, VisualNode secondNode)
        {
            firstNode.Node.Neighbors.Add(secondNode.Node);
            secondNode.Node.Neighbors.Add(firstNode.Node);
            firstNode.Node.NeighborsKeys.Add(secondNode.Node.Id);
            secondNode.Node.NeighborsKeys.Add(firstNode.Node.Id);

            await WebApi.UpdateData<Node>(firstNode.Node, firstNode.Node.Id.ToString());
            await WebApi.UpdateData<Node>(secondNode.Node, secondNode.Node.Id.ToString());
        }
    }
}
