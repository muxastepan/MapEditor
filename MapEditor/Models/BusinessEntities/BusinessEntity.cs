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
    public class BusinessEntity : ObservableObject
    {
        public string Name
        {
            get => GetOrCreate<string>();
            set => SetAndNotify(value);
        }

        public string Url
        {
            get => GetOrCreate<string>();
            set => SetAndNotify(value);
        }

        public ObservableCollection<Field> FieldNames
        {
            get => GetOrCreate(new ObservableCollection<Field>());
            set => SetAndNotify(value);
        }

        public ObservableCollection<BusinessElement> BusinessElements
        {
            get => GetOrCreate(new ObservableCollection<BusinessElement>());
            set => SetAndNotify(value);
        }
    }
}
