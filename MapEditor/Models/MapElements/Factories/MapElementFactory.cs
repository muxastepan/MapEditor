using System.Threading.Tasks;
using System.Windows;
using MapEditor.Models.Settings;
using WebApiNET.Models;

namespace MapEditor.Models.MapElements.Factories
{
    public abstract class MapElementFactory
    {
        public abstract Task<MapElement> Create(Point position, VisualSettings settings, Floor selectedFloor);
    }
}
