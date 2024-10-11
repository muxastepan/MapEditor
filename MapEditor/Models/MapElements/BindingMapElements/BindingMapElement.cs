using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using MapEditor.Models.BusinessEntities;

namespace MapEditor.Models.MapElements.BindingMapElements
{
    public abstract class BindingMapElement : MapElement
    {
        public bool IsLinked
        {
            get => GetOrCreate<bool>();
            set => SetAndNotify(value, callback:OnIsLinkedChanged);
        }

        protected abstract void OnIsLinkedChanged(PropertyChangingArgs<bool> obj);

        public BusinessElement? BindedBusinessElement
        {
            get => GetOrCreate<BusinessElement>();
            set => SetAndNotify(value);
        }
    }
}
