using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace MapEditor.Models
{
    public class Settings:ObservableObject
    {
        public string ApiUrl
        {
            get=>GetOrCreate<string>(); 
            set=> SetAndNotify(value);
        }

        public double PointWidth
        {
            get=>GetOrCreate<double>(); 
            set=>SetAndNotify(value);
        }

        public double PointHeight
        {
            get => GetOrCreate<double>();
            set => SetAndNotify(value);
        }

        public double LinkThickness
        {
            get => GetOrCreate<double>(); 
            set => SetAndNotify(value);
        }
    }
}
