using System;
using System.Collections.Generic;
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

        public string NaviUrl
        {
            get => GetOrCreate<string>();
            set=> SetAndNotify(value);
        }

    }
}
