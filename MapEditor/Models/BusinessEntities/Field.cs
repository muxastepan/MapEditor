using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace MapEditor.Models.BusinessEntities
{
    /// <summary>
    /// Поле из Web API.
    /// </summary>
    public class Field : ObservableObject
    {
        /// <summary>
        /// Значение поля.
        /// </summary>
        public string Value
        {
            get => GetOrCreate<string>();
            set => SetAndNotify(value);
        }

        /// <summary>
        /// Имя поля в Web API.
        /// </summary>
        public string Key
        {
            get => GetOrCreate<string>();
            set => SetAndNotify(value);
        }

        /// <summary>
        /// Имя поля в GUI.
        /// </summary>
        public string VerboseName
        {
            get => GetOrCreate<string>();
            set => SetAndNotify(value);
        }

        /// <summary>
        /// Является ли поле первичным ключом сущности.
        /// </summary>
        public bool IsPrimary
        {
            get => GetOrCreate<bool>();
            set => SetAndNotify(value);
        }

        /// <summary>
        /// Отностительный URL сущности.
        /// </summary>
        public string PrimaryGroupName
        {
            get => GetOrCreate<string>();
            set => SetAndNotify(value);
        }


        /// <summary>
        /// Отображать поле в GUI.
        /// </summary>
        public bool IsVisible
        {
            get => GetOrCreate<bool>();
            set => SetAndNotify(value);
        }

        /// <summary>
        /// Использовать поле в поиске.
        /// </summary>
        public bool IsIndex
        {
            get => GetOrCreate<bool>();
            set => SetAndNotify(value);
        }
    }
}
