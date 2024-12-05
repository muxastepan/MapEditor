using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using NavigationApp.Models;
using Newtonsoft.Json;

namespace MapEditor.Models.BusinessEntities
{
    /// <summary>
    /// Экземпляр сущности из Web API.
    /// </summary>
    public class BusinessElement : ObservableObject
    {
        /// <summary>
        /// Сущность, к которой принадлежит экземпляр.
        /// </summary>
        [JsonIgnore]
        public BusinessEntity ParentBusinessEntity
        {
            get => GetOrCreate<BusinessEntity>();
            set => SetAndNotify(value);
        }

        /// <summary>
        /// Поля экземпляра.
        /// </summary>
        [JsonIgnore]
        public ObservableCollection<Field> Fields
        {
            get => GetOrCreate(new ObservableCollection<Field>());
            set => SetAndNotify(value);
        }

        /// <summary>
        /// Поля точки.
        /// </summary>
        [JsonProperty("node")]
        public int? NodeField
        {
            get => GetOrCreate<int?>();
            set => SetAndNotify(value);
        }

        /// <summary>
        /// Поля области.
        /// </summary>
        [JsonProperty("area")]
        public int? AreaField
        {
            get => GetOrCreate<int?>();
            set => SetAndNotify(value);
        }
    }
}
