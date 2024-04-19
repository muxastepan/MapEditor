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
    public class NetworkSettings:ObservableObject
    {
        public string ApiUrl
        {
            get => GetOrCreate<string>();
            set => SetAndNotify(value);
        }

        public bool UseApiSuffix
        {
            get => GetOrCreate(true);
            set => SetAndNotify(value);
        }


        public ObservableCollection<BusinessEntity> BusinessEntities
        {
            get => GetOrCreate(new ObservableCollection<BusinessEntity>());
            set => SetAndNotify(value);
        }

    }
}
