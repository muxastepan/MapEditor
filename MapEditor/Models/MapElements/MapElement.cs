using System.Threading.Tasks;
using System.Windows;
using Core;
using NavigationApp.Models;

namespace MapEditor.Models.MapElements
{
    public abstract class MapElement:ObservableObject
    {
        public bool IsSelected
        {
            get => GetOrCreate<bool>();
            set => SetAndNotify(value);
        }

        public bool IsVisible
        {
            get => GetOrCreate(true);
            set => SetAndNotify(value);
        }


        public abstract Task Delete();

    }
}
