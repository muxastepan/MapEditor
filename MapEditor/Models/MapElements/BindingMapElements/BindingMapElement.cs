using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor.Models.MapElements.BindingMapElements
{
    public abstract class BindingMapElement : MapElement
    {
        public bool IsLinked
        {
            get => GetOrCreate<bool>();
            set => SetAndNotify(value);
        }

        public BusinessElement? BindedBusinessElement
        {
            get => GetOrCreate<BusinessElement>();
            set => SetAndNotify(value);
        }
    }
}
