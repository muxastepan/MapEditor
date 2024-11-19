using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MapEditor.Models.MapElements.BindingMapElements;
using MapEditor.Models.Settings;
using WebApiNET;
using WebApiNET.Models;

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
                        X = position.X,
                        Y = position.Y
                    },
                    RouteTypes = [..settings.RouteTypes]
                },
                AllRouteTypes = settings.RouteTypes.ToList(),
                VisualCoordinates = new Point(position.X-settings.NodePointWidth/2,position.Y-settings.NodePointHeight/2)
            };
            newNode.Color = newNode.RouteTypeColor;
            var (response, result) = await WebApi.SendData<Node>(newNode.Node.ToCreate());
            newNode.Node.Id = result?.Id ?? 0;
            return newNode;
        }
    }
}
