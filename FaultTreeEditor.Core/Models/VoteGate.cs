using System;
using System.Collections.Generic;
using System.Text;

namespace FaultTreeEditor.Core.Models
{
    public class VoteGate : Gate
    {
        public int K { get; set; }
        public int M
        {
            get
            {
                return Children.Count;
            }
            set { }
        }
        public override string ImageSource
        {
            get
            {
                return "/Assets/Images/Elements/votegate.png";
            }
        }
        public override string DisplayTitle
        {
            get
            {
                return "Vote gate";
            }
        }
        public override string Info
        {
            get
            {
                return "Vote gates (Galileo: ‹name› ‹k›of‹m› ‹input1› ‹input2› . . . ;) must have exactly m inputs. " +
                    "They fail if at least k of their m inputs fail. If one wishes to avoid specifying the number of " +
                    "inputs, alternative textual notation vot‹k› is available.";
            }
        }
        public override string ToGalileo()
        {
            string builder = "";
            if (M == 0)
            {
                foreach (var v in Parents)
                {
                    builder += $"{v.Title} vot{K} {getChildrenString()};\n";
                }
            }
            else
            {
                foreach (var v in Parents)
                {
                    builder += $"{v.Title} {K}of{M} {getChildrenString()};\n";
                }
            }
            return builder;
        }
    }
}
