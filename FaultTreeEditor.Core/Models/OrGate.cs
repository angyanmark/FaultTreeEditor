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
                return "/Assets/Images/Elements/Dark/orgate.png";
            }
        }
        public override string DisplayTitle
        {
            get
            {
                return "OR gate";
            }
        }
        public override string Info
        {
            get
            {
                return "OR gates fail if any of their inputs fail.";
            }
        }
        public override string GalileoFormat
        {
            get
            {
                return "Galileo: ‹name› or ‹input1› ‹input2› . . . ;";
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
