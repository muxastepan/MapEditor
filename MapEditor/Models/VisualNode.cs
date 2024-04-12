using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Core;
using MapEditor.Models;


namespace NavigationApp.Models
{
    public class VisualNode: MapElement
    {

        public Node Node { get; set; }

        public double Width
        {
            get=>GetOrCreate<double>(); 
            set=>SetAndNotify(value);
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
    }
}
