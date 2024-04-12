using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace MapEditor.Models
{
    public class BusinessObject:ObservableObject
    {
        public string Url
        {
            get=>GetOrCreate<string>();
            set=>SetAndNotify(value);
        }

        public Dictionary<string,string> Fields
        {
            get => GetOrCreate(new Dictionary<string, string>());
            set => SetAndNotify(value);
        }
    }
}
