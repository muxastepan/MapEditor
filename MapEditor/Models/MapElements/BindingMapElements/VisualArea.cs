using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using NavigationApp.Models;
using WebApiNET;

namespace MapEditor.Models.MapElements.BindingMapElements
{
    /// <summary>
    /// UI элемент области.
    /// </summary>
    public class VisualArea : BindingMapElement
    {
        public Area Area { get; set; }
        public bool IsEditing
        {
            get => GetOrCreate<bool>();
            set => SetAndNotify(value);
        }

        public bool IsFinished { get; set; }

        public double PointWidth
        {
            get => GetOrCreate<double>();
            set => SetAndNotify(value);
        }

        public double PointHeight
        {
            get => GetOrCreate<double>();
            set => SetAndNotify(value);
        }

        /// <summary>
        /// Удаляет область на сервере.
        /// </summary>
        public override async Task<bool> Delete()
        {
            return await WebApi.DeleteData<Area>(Area.Id.ToString());
        }

        /// <summary>
        /// Добавляет точку к области.
        /// </summary>
        public void AddPoint(Point position, Floor floor)
        {
            Area.NaviPoints.Add(new NaviPoint
            {
                Floor = floor.Id,
                X = position.X,
                Y = position.Y
            });
        }

        /// <summary>
        /// Прекратить редактирование области и отправить изменения на сервер.
        /// </summary>
        public async Task<bool> StopEditing()
        {
            IsEditing = false;
            if (IsFinished)
            {
                return await WebApi.UpdateData<Area>(Area, Area.Id.ToString());
            }
            IsFinished = true;
            var (resp, result) = await WebApi.SendData<Area>(Area);
            if(result?.Id is null) return false;
            if (resp is null) return false;
            Area.Id = result.Id;
            return resp.IsSuccessStatusCode;

        }

    }
}
