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
        public override string Info
        {
            get
            {
                return "Priority AND gates fail " +
                    "if all their inputs fail in the specified order, i.e., input2 must fail after input1. " +
                    "By default, priority AND gates are inclusive, and fail when input1 and input2 fail simultaneously. " +
                    "If desired, inclusiveness can be explicitly specified by the pand - inc keyword. The pand-ex " +
                    "keyword specifies exclusive priority AND gates, which survive simultaneous failure. " +
                    "Inclusive and exclusive gates can be graphically distinguished with the ≤ and < symbols, respectively. " +
                    "Most commonly, priority AND gates represent failure avoidance strategies. If the failure avoidance " +
                    "strategy connected to input1 is working correctly, failure of input2 does not affect the rest " +
                    "of the system, but if input1 fails first, the failure of input2 are let through. Inclusive and " +
                    "exclusive variation can specify the response of the gates to common - mode failures of the " +
                    "avoidance strategy and the monitored component.";
            }
        }
        public override string GalileoFormat
        {
            get
            {
                return "Galileo: ‹name› pand ‹input1› ‹input2› . . . ;";
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
