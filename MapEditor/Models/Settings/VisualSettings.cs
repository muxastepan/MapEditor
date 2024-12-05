using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace MapEditor.Models.Settings
{
    /// <summary>
    /// Настройки внешнего вида.
    /// </summary>
    public class VisualSettings:ObservableObject
    {
        /// <summary>
        /// Ширина отображения точки.
        /// </summary>
        public double NodePointWidth
        {
            get => GetOrCreate<double>();
            set => SetAndNotify(value);
        }

        /// <summary>
        /// Высота отображения точки.
        /// </summary>
        public double NodePointHeight
        {
            get => GetOrCreate<double>();
            set => SetAndNotify(value);
        }

        /// <summary>
        /// Ширина отображения точки области при редактировании.
        /// </summary>
        public double AreaPointWidth
        {
            get => GetOrCreate<double>();
            set => SetAndNotify(value);
        }

        /// <summary>
        /// Высота отображения точки области при редактировании.
        /// </summary>
        public double AreaPointHeight
        {
            get => GetOrCreate<double>();
            set => SetAndNotify(value);
        }

        /// <summary>
        /// Толщина линии связи точек.
        /// </summary>
        public double LinkThickness
        {
            get => GetOrCreate<double>();
            set => SetAndNotify(value);
        }

        /// <summary>
        /// Размер шрифта имени следующего этажа при точке-переходе между этажами.
        /// </summary>
        public double TransitiveNodeFontSize
        {
            get=>GetOrCreate<double>();
            set => SetAndNotify(value);
        }
    }
}
