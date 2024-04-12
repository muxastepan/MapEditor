﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MapEditor.Models.Settings;
using NavigationApp.Models;

namespace MapEditor.Models.MapElements.Factories
{
    public class VisualAreaFactory:MapElementFactory
    {
        public override async Task<MapElement> Create(Point position, VisualSettings settings, Floor selectedFloor)
        {
            
            var creatingArea = new VisualArea
            {
                Area = new Area
                {
                    Floor = selectedFloor.Id,
                    
                },
                PointWidth = settings.AreaPointWidth,
                PointHeight = settings.AreaPointHeight,
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
