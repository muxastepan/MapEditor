using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MapEditor.Models.BusinessEntities;
using MapEditor.Models.Settings;
using WebApiNET;
using WebApiNET.Models;

namespace MapEditor.Helpers
{
    public class SettingsManager: JsonManager<Settings>
    {
        public SettingsManager(string path) : base(path)
        {
            DefaultValue = new Settings
            {
                NetworkSettings = new NetworkSettings
                {
                    ApiUrl = "http://127.0.0.1:8000",
                    BusinessEntities = []
                },
                VisualSettings = new VisualSettings
                {
                    LinkThickness = 10,
                    NodePointHeight = 100,
                    NodePointWidth = 100,
                    AreaPointWidth = 50,
                    AreaPointHeight = 50,
                    TransitiveNodeFontSize = 20,
                    RouteTypes = [new RouteType()]
                }
            };
        }

        public async Task<Settings> UpdateFromApi()
        {
            var settings = Read();
            var routeTypes = await WebApi.GetData<ObservableCollection<RouteType>>();
            if (routeTypes is null || routeTypes.Count==0)
            {
                settings.VisualSettings.RouteTypes.Clear();
                return settings;
            }
            foreach (var routeType in routeTypes)
            {
                if(settings.VisualSettings.RouteTypes.FirstOrDefault(rt=>rt.Id==routeType.Id) is not null)
                    continue;
                settings.VisualSettings.RouteTypes.Add(routeType);
            }

            settings.VisualSettings.SelectedRouteType = settings.VisualSettings.RouteTypes.FirstOrDefault()??new RouteType();
            return settings;
        }
    }
}
