using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NavigationApp.Models;

namespace MapEditor.Models.MapElements.Factories
{
    public class VisualAreaFactory:MapElementFactory
    {
        public override async Task<MapElement> Create(Point position, Settings settings, Floor selectedFloor)
        {
            
            var creatingArea = new VisualArea
            {
                Area = new Area
                {
                    Floor = selectedFloor.Id,
                    
                },
                IsEditing = true,
                IsVisible = true,
            };
            

            
            creatingArea.Area.NaviPoints.Add(new NaviPoint
            {
                Floor = selectedFloor.Id,
                X = position.X,
                Y = position.Y
            });
            return creatingArea;
        }
    }
}
