using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using MapEditor.Models.BusinessEntities;

namespace MapEditor.Models.Settings
{
    /// <summary>
    /// Модель настроек сети.
    /// </summary>
    public class NetworkSettings:ObservableObject
    {
        /// <summary>
        /// Ссылка на Web API.
        /// </summary>
        public string ApiUrl
        {
            get => GetOrCreate<string>();
            set => SetAndNotify(value);
        }

        /// <summary>
        /// Использовать суффикс /api в ссылке на Web API.
        /// </summary>
        public bool UseApiSuffix
        {
            get => GetOrCreate(true);
            set => SetAndNotify(value);
        }

        /// <summary>
        /// Сущности из Web API.
        /// </summary>
        public ObservableCollection<BusinessEntity> BusinessEntities
        {
            get => GetOrCreate(new ObservableCollection<BusinessEntity>());
            set => SetAndNotify(value);
        }

    }
}
