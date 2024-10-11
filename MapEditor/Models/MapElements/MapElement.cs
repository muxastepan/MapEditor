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
            set => SetAndNotify(value,callback:OnIsSelectedChanged);
        }

        protected abstract void OnIsSelectedChanged(PropertyChangingArgs<bool> obj);

        public bool IsVisible
        {
            get => GetOrCreate(true);
            set => SetAndNotify(value);
        }


        public abstract Task<bool> Delete();

    }
}
