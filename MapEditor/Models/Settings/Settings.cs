﻿using Core;

namespace MapEditor.Models.Settings
{
    /// <summary>
    /// Модель настроек.
    /// </summary>
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
       
    }
}
