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
                return "/Assets/Images/Elements/Dark/priorityorgate.png";
            }
        }
        public override string DisplayTitle
        {
            get
            {
                return "Priority OR gate";
            }
        }
        public override string Info
        {
            get
            {
                return "Priority OR gates only fail " +
                    "if input1 fails before any other input. By default, priority OR gates are " +
                    "inclusive, i.e., fail if input1 fails simultaneously with some other input. " +
                    "Inclusive and exclusive priority OR gates can be specified with the " +
                    "por-inc and por-ex keywords, respectively.";
            }
        }
        public override string GalileoFormat
        {
            get
            {
                return "Galileo: ‹name› por ‹input1› ‹input2› . . . ;";
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
