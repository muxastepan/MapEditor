using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Core;
using WebApiNET;
using WebApiNET.Models;

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


        public List<RouteType> AllRouteTypes { get; set; } = [];

        private List<VisualRouteType>? _visualRouteTypes;
        public List<VisualRouteType> VisualRouteTypes
        {
            get => GetVisualRouteTypes();
            set => SetAndNotify(value);
        }

        private string SelectedColor => "Orange";

        private string LinkedColor => "LightGreen";

        public string RouteTypeColor=> Node.RouteTypes.Select(rt=>rt.Color).GetAverageColor();

        public override async Task<bool> Delete()
        {
            return await WebApi.DeleteData<Node>(Node.Id.ToString());
        }

        protected override void OnIsSelectedChanged(PropertyChangingArgs<bool> obj) => Color = obj.NewValue ? SelectedColor : RouteTypeColor;
        protected override void OnIsLinkedChanged(PropertyChangingArgs<bool> obj) =>
            Color = obj.NewValue ? LinkedColor : RouteTypeColor;

        private List<VisualRouteType> GetVisualRouteTypes()=>
            _visualRouteTypes ??= 
            AllRouteTypes
                .Select(rt => new VisualRouteType(this,rt,Node.RouteTypes.Any(nrt=>rt.Id==nrt.Id)))
                .ToList();
    }
}

public static class ColorStringExtensions
{
    public static string GetAverageColor(this IEnumerable<string> colors)
    {
        var avgColor = colors.Select(c => c.TrimStart('#').WholeChunks(2).GetHexIntegers())
            .GetAverageOfEnumerables().Select(a => a.ToString("X2")).ConcatToOneString();
        return string.IsNullOrEmpty(avgColor)?"#FFFFFF": '#' + avgColor;
    } 
    

    private static string ConcatToOneString(this IEnumerable<string> strings) => string.Concat(strings);

    private static IEnumerable<int> GetHexIntegers(this IEnumerable<string> hexStrings) =>
        hexStrings.Select(hs => hs.GetHexInt());

    private static int GetHexInt(this string hexString) => Convert.ToInt16(hexString,16);

    public static IEnumerable<string> WholeChunks(this string str, int chunkSize)
    {
        for (int i = 0; i < str.Length; i += chunkSize)
            yield return str.Substring(i, chunkSize);
    }

    public static IEnumerable<int> GetAverageOfEnumerables(this IEnumerable<IEnumerable<int>> source)
    {
        var s = source
            .Select(e => e.ToList()).ToList();

        var length = s.FirstOrDefault()?.Count ?? 0;
        return s.SelectMany(x => x)
            .Select((v, i) => new { Value = v, Index = i % length })
            .GroupBy(x => x.Index)
            .Select(y => (int)y.Average(z=>z.Value));
    }
}