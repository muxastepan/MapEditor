using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor.Models.Help
{
    /// <summary>
    /// Раздел справки с подразделами.
    /// </summary>
    public class HelpCompositeSection:BaseHelpSection
    {
        public List<BaseHelpSection>? InnerHelpSections { get; set; }
    }
}
