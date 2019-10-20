using System;
using System.Collections.Generic;
using System.Text;

namespace FaultTreeEditor.Core.Models
{
    public class OrGate : Gate
    {
        public override string ToString()
        {
            string builder = "";
            foreach (var v in Parents)
            {
                builder += $"{v.Title} or {getChildrenString()};\n";
            }
            return builder;
        }
    }
}
