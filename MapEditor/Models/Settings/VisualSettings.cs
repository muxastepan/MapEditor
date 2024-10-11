using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using WebApiNET.Models;

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

        public double TransitiveNodeFontSize
        {
            get=>GetOrCreate<double>();
            set => SetAndNotify(value);
        }

        public RouteType SelectedRouteType
        {
            get => GetOrCreate(new RouteType());
            set => SetAndNotify(value);
        }

        public ObservableCollection<RouteType> RouteTypes
        {
            get => GetOrCreate(new ObservableCollection<RouteType>());
            set => SetAndNotify(value);
        }

    }
}
