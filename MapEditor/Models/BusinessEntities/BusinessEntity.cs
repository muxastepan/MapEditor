using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace MapEditor.Models.BusinessEntities
{
    /// <summary>
    /// Модель описания сущности из Web API.
    /// </summary>
    public class BusinessEntity : ObservableObject
    {
        /// <summary>
        /// Имя в GUI.
        /// </summary>
        public string Name
        {
            get => GetOrCreate<string>();
            set => SetAndNotify(value);
        }

        /// <summary>
        /// Относительный URL сущности.
        /// </summary>
        public string Url
        {
            get => GetOrCreate<string>();
            set => SetAndNotify(value);
        }

        /// <summary>
        /// Поля из Web API.
        /// </summary>
        public ObservableCollection<Field> FieldNames
        {
            get => GetOrCreate(new ObservableCollection<Field>());
            set => SetAndNotify(value);
        }

        /// <summary>
        /// Экземпляры сущности.
        /// </summary>
        public ObservableCollection<BusinessElement> BusinessElements
        {
            get => GetOrCreate(new ObservableCollection<BusinessElement>());
            set => SetAndNotify(value);
        }

        /// <summary>
        /// Отфильтрованные экземпляры сущности.
        /// </summary>
        public ObservableCollection<BusinessElement> FilteredBusinessElements
        {
            get => GetOrCreate(new ObservableCollection<BusinessElement>());
            set => SetAndNotify(value);
        }
    }
}
