using System.Collections.ObjectModel;
using MapEditor.Models.BusinessEntities;
using MapEditor.Models.Settings;

namespace MapEditor.Helpers
{
    /// <summary>
    /// Менеджер настроек.
    /// </summary>
    /// <inheritdoc cref="JsonManager{T}"/>
    public class SettingsManager: JsonManager<Settings>
    {
        /// <inheritdoc cref="JsonManager{T}"/>
        public SettingsManager(string path) : base(path)
        {
            DefaultValue = new Settings
            {
                NetworkSettings = new NetworkSettings
                {
                    ApiUrl = "http://127.0.0.1:8000",
                    BusinessEntities = new ObservableCollection<BusinessEntity>(),
                },
                VisualSettings = new VisualSettings
                {
                    LinkThickness = 10,
                    NodePointHeight = 100,
                    NodePointWidth = 100,
                    AreaPointWidth = 50,
                    AreaPointHeight = 50,
                    TransitiveNodeFontSize = 20
                },
            };
        }
    }
}
