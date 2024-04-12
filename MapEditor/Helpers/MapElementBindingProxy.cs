using System.Windows;
using MapEditor.Models.MapElements;

namespace MapEditor.Helpers
{
    public class MapElementBindingProxy:Freezable
    {
        protected override Freezable CreateInstanceCore()
        {
            return new MapElementBindingProxy();
        }

        public MapElement Data
        {
            get { return (MapElement)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(MapElement), typeof(MapElementBindingProxy), new UIPropertyMetadata(null));
    }
}
