using System.Collections.ObjectModel;
using MapEditor.Models;

namespace MapEditor.Helpers
{
    public class SettingsManager: JsonManager<Settings>
    {
        public SettingsManager(string path) : base(path)
        {
            DefaultValue = new Settings
            {
                ApiUrl = "http://127.0.0.1:8000",
                LinkThickness = 10,
                PointHeight = 100,
                PointWidth = 100
            };
        }
    }
}
