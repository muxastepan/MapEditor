using System.Windows;
using MapEditor.Models.MapElements;
using MapEditor.Models.MapElements.BindingMapElements;

namespace MapEditor.Helpers
{
    public class MapElementBindingProxy:Freezable
    {
        protected override Freezable CreateInstanceCore()=> new MapElementBindingProxy();

        public MapElement Data
        {
            get => (MapElement)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register(nameof(Data), typeof(MapElement), typeof(MapElementBindingProxy), new UIPropertyMetadata(null));
    }

    public class VisualNodeBindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore() => new VisualNodeBindingProxy();

        public VisualNode Data
        {
            get => (VisualNode)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register(nameof(Data), typeof(VisualNode), typeof(VisualNodeBindingProxy), new UIPropertyMetadata(null));
    }
}
