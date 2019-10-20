using System;
using System.Collections.Generic;
using System.Text;

namespace FaultTreeEditor.Core.Models
{
    public class BasicEvent : Event
    {
        public double Probability { get; set; }
        public override string ToString()
        {
            return $"{Title} lambda={Probability};\n";
        }
    }
}
