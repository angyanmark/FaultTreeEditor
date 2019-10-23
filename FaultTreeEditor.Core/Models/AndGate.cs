using System;
using System.Collections.Generic;
using System.Text;

namespace FaultTreeEditor.Core.Models
{
    public class AndGate : Gate
    {
        public override string ImageSource
        {
            get
            {
                return "/Assets/Images/Elements/andgate.png";
            }
        }
        public override string DisplayTitle
        {
            get
            {
                return "AND gate";
            }
        }

        public override string ToGalileo()
        {
            string builder = "";
            foreach(var v in Parents)
            {
                builder += $"{v.Title} and {getChildrenString()};\n";
            }
            return builder;
        }
    }
}
