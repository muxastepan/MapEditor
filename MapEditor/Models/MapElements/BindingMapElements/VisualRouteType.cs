using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;
using Core;
using MapEditor.Helpers;
using WebApiNET;
using WebApiNET.Models;

namespace MapEditor.Models.MapElements.BindingMapElements;

public class VisualRouteType(VisualNode parentNode, RouteType nodeRouteType,bool isSelected):ObservableObject
{
    public RouteType NodeRouteType { get; } = nodeRouteType;

    public ObservableCollection<RouteType> NodeRouteTypes { get; } = parentNode.Node.RouteTypes;

    public bool IsSelected
    {
        get => GetOrCreate(isSelected);
        set => SetAndNotify(value,callback:SelectedChanged);
    }

    private async void SelectedChanged(PropertyChangingArgs<bool> obj)
    {
        if(obj.NewValue==obj.OldValue) return;
        if (obj.NewValue)
        {
            NodeRouteTypes.Add(NodeRouteType);
        }
        else
        {
            var routeType = NodeRouteTypes.FirstOrDefault(nrt => nrt.Id == NodeRouteType.Id);
            if (routeType is not null) NodeRouteTypes.Remove(routeType);
        }
        parentNode.Color = parentNode.RouteTypeColor;
        await WebApi.UpdateData<NodeCreate>(parentNode.Node.ToCreate(), parentNode.Node.Id.ToString());
    }
}