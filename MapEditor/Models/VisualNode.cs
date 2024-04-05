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
        public double Width { get; set; }
        public double Height { get; set; }

        

        public Point VisualCoordinates
        {
            get => GetOrCreate<Point>();
            set => SetAndNotify(value);
        }
    }
}
