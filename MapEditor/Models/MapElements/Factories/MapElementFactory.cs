using NavigationApp.Models;
using System.Threading.Tasks;
using System.Windows;

namespace MapEditor.Models.MapElements.Factories
{
    public abstract class MapElementFactory
    {
        public abstract Task<MapElement> Create(Point position, Settings settings, Floor selectedFloor);
    }
}
