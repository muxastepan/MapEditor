using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Newtonsoft.Json;

namespace NavigationApp.Models
{
    public class NaviPoint:ObservableObject
    {

        [JsonProperty("x")]
        public double X
        {
            get=> GetOrCreate<double>(); 
            set=> SetAndNotify(value);
        }

        [JsonProperty("y")]
        public double Y {
            get => GetOrCreate<double>();
            set => SetAndNotify(value);
        }

        [JsonProperty("floor")]
        public int Floor { get => GetOrCreate<int>(); set => SetAndNotify(value); }
    }
}
