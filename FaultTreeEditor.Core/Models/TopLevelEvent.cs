using System;
using System.Collections.Generic;
using System.Text;

namespace FaultTreeEditor.Core.Models
{
    public class TopLevelEvent : Event
    {
        public override string ToString()
        {
            return $"toplevel {Title};\n";
        }
    }
}
