using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using NavigationApp.Models;
using WebApiNET;

namespace MapEditor.Models.MapElements
{
    public class VisualArea:MapElement
    {
        public Area Area { get; set; }
        public bool IsEditing
        {
            get => GetOrCreate<bool>();
            set => SetAndNotify(value);
        }

        public double PointWidth
        {
            get => GetOrCreate<double>();
            set => SetAndNotify(value);
        }

        public double PointHeight
        {
            get=>GetOrCreate<double>();
            set => SetAndNotify(value);
        }

        public override async Task Delete()
        {
            await WebApi.DeleteData<Area>(Area.Id.ToString());
        }

        public void AddPoint(Point position, Floor floor)
        {
            Area.NaviPoints.Add(new NaviPoint
            {
                Floor = floor.Id,
                X = position.X,
                Y = position.Y
            });
        }

        public async Task StopEditing()
        {
            IsEditing = false;
            var (resp, result) = await WebApi.SendData<Area>(Area);
            Area.Id = result.Id;
        }
    }
}
