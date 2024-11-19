using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using WebApiNET.Models;

namespace MapEditor.Converters
{
    public class PointConverter:IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not IEnumerable<NaviPoint> points) return null;
            var pointsCollection = new PointCollection();
            foreach (var naviPoint in points)
            {
                pointsCollection.Add(new Point(naviPoint.X,naviPoint.Y));
            }
            return pointsCollection;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
