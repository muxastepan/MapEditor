using Core;

namespace MapEditor.Models.Settings
{
    public class Settings:ObservableObject
    {

        public NetworkSettings NetworkSettings
        {
            get=>GetOrCreate<NetworkSettings>(); 
            set=>SetAndNotify(value);
        }

        public VisualSettings VisualSettings
        {
            get=> GetOrCreate<VisualSettings>();
            set=>SetAndNotify(value);
        }

        public BusinessObjectsSettings BusinessObjectsSettings
        {
            get => GetOrCreate<BusinessObjectsSettings>();
            set => SetAndNotify(value);
        }
       
    }
}
