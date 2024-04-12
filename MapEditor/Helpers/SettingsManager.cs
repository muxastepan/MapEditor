using System.Collections.ObjectModel;
using MapEditor.Models;
using MapEditor.Models.Settings;

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
                    BusinessObjects = new ObservableCollection<BusinessObject>(),
                },
                VisualSettings = new VisualSettings
                {
                    LinkThickness = 10,
                    NodePointHeight = 100,
                    NodePointWidth = 100,
                    AreaPointWidth = 50,
                    AreaPointHeight = 50,
                },
            };
        }
    }
}
