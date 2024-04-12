using NavigationApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MapEditor.Models.Settings;
using WebApiNET;

namespace MapEditor.Models.MapElements.Factories
{
    public class VisualNodeFactory:MapElementFactory
    {
        public override async Task<MapElement> Create(Point position, VisualSettings settings, Floor selectedFloor)
        {
            var newNode = new VisualNode
            {
                Height = settings.NodePointHeight,
                Width = settings.NodePointWidth,
                Node = new Node
                {
                    Point = new NaviPoint
                    {
                        Floor = selectedFloor.Id,
                        X = position.X - settings.NodePointWidth / 2,
                        Y = position.Y - settings.NodePointHeight / 2
                    }
                },
                VisualCoordinates = position
            };
            var (response, result) = await WebApi.SendData<Node>(newNode.Node);
            newNode.Node.Id = result.Id;
            return newNode;
        }
    }
}
