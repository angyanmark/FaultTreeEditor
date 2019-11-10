using System;
using System.Collections.Generic;
using System.Text;

namespace FaultTreeEditor.Core.Models
{
    public class PriorityAndGate : Gate
    {
        public override string ImageSource
        {
            get
            {
                return "/Assets/Images/Elements/priorityandgate.png";
            }
        }
        public override string DisplayTitle
        {
            get
            {
                return "Priority AND gate";
            }
        }

        public override string ToGalileo()
        {
            string builder = "";
            foreach (var v in Parents)
            {
                builder += $"{v.Title} pand {getChildrenString()};\n";
            }
            return builder;
        }
    }
}
