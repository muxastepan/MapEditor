using System.Threading.Tasks;
using System.Windows;
using NavigationApp.Models;
using WebApiNET;

namespace MapEditor.Models.MapElements.BindingMapElements
{
    /// <summary>
    /// Модель точки для отображения.
    /// </summary>
    public class VisualNode : BindingMapElement
    {

        public Node Node { get; set; }

        /// <summary>
        /// Ширина точки.
        /// </summary>
        public double Width
        {
            get => GetOrCreate<double>();
            set => SetAndNotify(value);
        }

        /// <summary>
        /// Высота точки.
        /// </summary>
        public double Height
        {
            get => GetOrCreate<double>();
            set => SetAndNotify(value);
        }

        /// <summary>
        /// Имя следующего этажа при переходе.
        /// </summary>
        public string LinkedFloor
        {
            get => GetOrCreate<string>();
            set => SetAndNotify(value);
        }

        /// <summary>
        /// Координаты точки для экрана.
        /// </summary>
        public Point VisualCoordinates
        {
            get => GetOrCreate<Point>();
            set => SetAndNotify(value);
        }

        /// <summary>
        /// Удаляет точку на сервере.
        /// </summary>
        public override async Task<bool> Delete()
        {
            return await WebApi.DeleteData<Node>(Node.Id.ToString());
        }
    }
}
