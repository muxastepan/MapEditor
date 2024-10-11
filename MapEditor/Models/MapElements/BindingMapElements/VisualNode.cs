using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Core;
using NavigationApp.Models;
using WebApiNET;

namespace MapEditor.Models.MapElements.BindingMapElements
{
    public class VisualNode : BindingMapElement
    {
        public Node Node { get; set; }

        public double Width
        {
            get => GetOrCreate<double>();
            set => SetAndNotify(value);
        }
        public double Height
        {
            get => GetOrCreate<double>();
            set => SetAndNotify(value);
        }


        public string LinkedFloor
        {
            get => GetOrCreate<string>();
            set => SetAndNotify(value);
        }

        public Point VisualCoordinates
        {
            get => GetOrCreate<Point>();
            set => SetAndNotify(value);
        }

        public string Color
        {
            get=>GetOrCreate("#FFFFFF");
            set => SetAndNotify(value);
        }

        private string SelectedColor => "Orange";

        private string LinkedColor => "LightGreen";

        public string RouteTypeColor=> "#" + Node.RouteTypes.Select(rt=> Convert.ToInt32(rt.Color.TrimStart('#'),16)).Average(c=>c).ToString("X");

        public override async Task<bool> Delete()
        {
            return await WebApi.DeleteData<Node>(Node.Id.ToString());
        }

        protected override void OnIsSelectedChanged(PropertyChangingArgs<bool> obj) => Color = obj.NewValue ? SelectedColor : RouteTypeColor;
        protected override void OnIsLinkedChanged(PropertyChangingArgs<bool> obj) =>
            Color = obj.NewValue ? LinkedColor : RouteTypeColor;
    }
}
