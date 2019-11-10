using System;
using System.Collections.Generic;
using System.Text;

namespace FaultTreeEditor.Core.Models
{
    public class PriorityOrGate : Gate
    {
        public override string ImageSource
        {
            get
            {
                return "/Assets/Images/Elements/priorityorgate.png";
            }
        }
        public override string DisplayTitle
        {
            get
            {
                return "Priority OR gate";
            }
        }
        public override string ToGalileo()
        {
            string builder = "";
            foreach (var v in Parents)
            {
                builder += $"{v.Title} por {getChildrenString()};\n";
            }
            return builder;
        }
    }
}
