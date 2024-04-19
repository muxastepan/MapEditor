using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace MapEditor.Models.BusinessEntities
{
    public class Field : ObservableObject
    {
        public string Value
        {
            get => GetOrCreate<string>();
            set => SetAndNotify(value);
        }

        public string Key
        {
            get => GetOrCreate<string>();
            set => SetAndNotify(value);
        }

        public string VerboseName
        {
            get => GetOrCreate<string>();
            set => SetAndNotify(value);
        }

        public bool IsPrimary
        {
            get => GetOrCreate<bool>();
            set => SetAndNotify(value);
        }

        public string PrimaryGroupName
        {
            get => GetOrCreate<string>();
            set => SetAndNotify(value);
        }

        public bool IsVisible
        {
            get => GetOrCreate<bool>();
            set => SetAndNotify(value);
        }
    }
}
