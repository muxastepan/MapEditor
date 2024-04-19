using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NavigationApp.Models;

namespace MapEditor.Models.BusinessEntities
{
    public class FieldValue
    {
        public object Value { get; set; }

        public override string ToString()
        {
            return Value switch
            {
                string s => s,
                int i => i.ToString(),
                float f => f.ToString(),
                double d => d.ToString(),
                _ => string.Empty
            };
        }
    }
}
