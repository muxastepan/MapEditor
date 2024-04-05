using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Core;
using MapEditor.Models;

namespace NavigationApp.Models
{
    public class Link:MapElement
    {
        public VisualNode From { get; set; }
        public VisualNode To { get; set; }

        public double Width
        {
            get => Math.Max(From.Node.Point.X, To.Node.Point.X) - Math.Min(From.Node.Point.X, To.Node.Point.X);
        }

        public double Height
        {
            get => Math.Max(From.Node.Point.Y, To.Node.Point.Y) - Math.Min(From.Node.Point.Y, To.Node.Point.Y);
        }
    }
}
