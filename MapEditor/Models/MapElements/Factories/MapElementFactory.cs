using NavigationApp.Models;
using System.Threading.Tasks;
using System.Windows;
using MapEditor.Models.Settings;

namespace MapEditor.Models.MapElements.Factories
{
    /// <summary>
    /// Базовый класс для создания объектов карты.
    /// </summary>
    public abstract class MapElementFactory
    {
        /// <summary>
        /// Создает объект на карте.
        /// </summary>
        /// <param name="position">Точка центра создаваемого объекта на карте</param>
        /// <param name="settings">Настройки внешнего вида</param>
        /// <param name="selectedFloor">Этаж, на котором будет создан объект</param>
        public abstract Task<MapElement> Create(Point position, VisualSettings settings, Floor selectedFloor);
    }
}
