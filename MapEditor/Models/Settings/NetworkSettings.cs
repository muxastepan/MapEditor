using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace MapEditor.Models.Settings
{
    public class NetworkSettings:ObservableObject
    {
        public string ApiUrl
        {
            get => GetOrCreate<string>();
            set => SetAndNotify(value);
        }

        public ObservableCollection<BusinessObject> BusinessObjects
        {
            get => GetOrCreate(new ObservableCollection<BusinessObject>());
            set => SetAndNotify(value);
        }

    }
}
