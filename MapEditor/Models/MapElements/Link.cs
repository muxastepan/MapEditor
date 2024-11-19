using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using Core;
using MapEditor.Models.MapElements.BindingMapElements;
using WebApiNET;
using WebApiNET.Models;

namespace MapEditor.Models.MapElements
{
    public class Link:MapElement
    {
        public VisualNode? From { get; set; }
        public VisualNode? To { get; set; }


        protected override void OnIsSelectedChanged(PropertyChangingArgs<bool> obj){}

        public override async Task<bool> Delete()
        {
            var from = From.Node;
            var to = To.Node;

            from.Neighbors.Remove(to);
            to.Neighbors.Remove(from);

            from.NeighborsKeys.Remove(to.Id);
            to.NeighborsKeys.Remove(from.Id);

            return await WebApi.UpdateData<Node>(from, from.Id.ToString()) && 
                   await WebApi.UpdateData<Node>(to, to.Id.ToString());
        }

        

        public static async Task<bool> LinkNodes(VisualNode firstNode, VisualNode secondNode)
        {
            firstNode.Node.Neighbors.Add(secondNode.Node);
            secondNode.Node.Neighbors.Add(firstNode.Node);
            firstNode.Node.NeighborsKeys.Add(secondNode.Node.Id);
            secondNode.Node.NeighborsKeys.Add(firstNode.Node.Id);

            return await WebApi.UpdateData<NodeCreate>(firstNode.Node.ToCreate(), firstNode.Node.Id.ToString())&&
            await WebApi.UpdateData<NodeCreate>(secondNode.Node.ToCreate(), secondNode.Node.Id.ToString());
        }
    }
}
