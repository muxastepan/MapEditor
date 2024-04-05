using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace MapEditor.Models
{
    public class MapElement:ObservableObject
    {
        public bool IsSelected
        {
            get => GetOrCreate<bool>();
            set => SetAndNotify(value);
        }

        public bool IsVisible
        {
            get => GetOrCreate(true);
            set => SetAndNotify(value);
        }

    }
}
