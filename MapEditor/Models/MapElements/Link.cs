﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using MapEditor.Models.MapElements.BindingMapElements;
using NavigationApp.Models;
using WebApiNET;

namespace MapEditor.Models.MapElements
{
    /// <summary>
    /// Связь двух точек.
    /// </summary>
    public class Link:MapElement
    {
        public VisualNode? From { get; set; }
        public VisualNode? To { get; set; }

        /// <summary>
        /// Удаляет связь на сервере.
        /// </summary>
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


        /// <summary>
        /// Связывает две точки и отправляет изменения на сервер.
        /// </summary>
        public static async Task<bool> LinkNodes(VisualNode firstNode, VisualNode secondNode)
        {
            firstNode.Node.Neighbors.Add(secondNode.Node);
            secondNode.Node.Neighbors.Add(firstNode.Node);
            firstNode.Node.NeighborsKeys.Add(secondNode.Node.Id);
            secondNode.Node.NeighborsKeys.Add(firstNode.Node.Id);

            return await WebApi.UpdateData<Node>(firstNode.Node, firstNode.Node.Id.ToString())&&
            await WebApi.UpdateData<Node>(secondNode.Node, secondNode.Node.Id.ToString());
        }
    }
}
