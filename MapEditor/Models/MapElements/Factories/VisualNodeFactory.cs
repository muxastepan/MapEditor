using NavigationApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WebApiNET;

namespace MapEditor.Models.MapElements.Factories
{
    public class VisualNodeFactory:MapElementFactory
    {
        public override async Task<MapElement> Create(Point position, Settings settings, Floor selectedFloor)
        {
            var newNode = new VisualNode
            {
                Height = settings.PointHeight,
                Width = settings.PointWidth,
                Node = new Node
                {
                    Point = new NaviPoint
                    {
                        Floor = selectedFloor.Id,
                        X = position.X - settings.PointWidth / 2,
                        Y = position.Y - settings.PointHeight / 2
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
