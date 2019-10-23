using System;
using System.Collections.Generic;
using System.Text;

namespace FaultTreeEditor.Core.Models
{
    public class OrGate : Gate
    {
        public override string ImageSource
        {
            get
            {
                return "/Assets/Images/Elements/orgate.png";
            }
        }
        public override string DisplayTitle
        {
            get
            {
                return "OR gate";
            }
        }
        public override string ToGalileo()
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
