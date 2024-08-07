﻿using System.Threading.Tasks;
using System.Windows;
using NavigationApp.Models;
using WebApiNET;

namespace MapEditor.Models.MapElements.BindingMapElements
{
    public class VisualNode : BindingMapElement
    {

        public Node Node { get; set; }

        public double Width
        {
            get => GetOrCreate<double>();
            set => SetAndNotify(value);
        }
        public double Height
        {
            get => GetOrCreate<double>();
            set => SetAndNotify(value);
        }


        public string LinkedFloor
        {
            get => GetOrCreate<string>();
            set => SetAndNotify(value);
        }

        public Point VisualCoordinates
        {
            get => GetOrCreate<Point>();
            set => SetAndNotify(value);
        }

        public override async Task<bool> Delete()
        {
            return await WebApi.DeleteData<Node>(Node.Id.ToString());
        }
    }
}
