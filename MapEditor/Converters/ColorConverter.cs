using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MapEditor.Converters;

public class ColorConverter:IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is not string s 
            ? new SolidColorBrush((Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFFFFF")) 
            : new SolidColorBrush((Color)System.Windows.Media.ColorConverter.ConvertFromString(s));
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}