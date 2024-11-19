using Core;
using Newtonsoft.Json;

namespace WebApiNET.Models
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
