using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace MapEditor.Models.Settings
{
    public class VisualSettings:ObservableObject
    {
        public double NodePointWidth
        {
            get => GetOrCreate<double>();
            set => SetAndNotify(value);
        }

        public double NodePointHeight
        {
            get => GetOrCreate<double>();
            set => SetAndNotify(value);
        }

        public double AreaPointWidth
        {
            get => GetOrCreate<double>();
            set => SetAndNotify(value);
        }

        public double AreaPointHeight
        {
            get => GetOrCreate<double>();
            set => SetAndNotify(value);
        }

        public double LinkThickness
        {
            get => GetOrCreate<double>();
            set => SetAndNotify(value);
        }
    }
}
