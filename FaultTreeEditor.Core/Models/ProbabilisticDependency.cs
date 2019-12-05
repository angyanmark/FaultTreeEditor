using System;
using System.Collections.Generic;
using System.Text;

namespace FaultTreeEditor.Core.Models
{
    public class ProbabilisticDependency : Gate
    {
        private double probability = 0;
        public double Probability
        {
            get { return probability; }
            set { Set(ref probability, value); }
        }
        public override string ImageSource
        {
            get
            {
                // TODO image
                return "/Assets/Images/Elements/event.png";
            }
        }
        public override string DisplayTitle
        {
            get
            {
                return "Probabilistic dependency";
            }
        }
        public override string Info
        {
            get
            {
                return "Probabilistic dependencies (Galileo: ‹name› pdep=‹p› ‹trigger› ‹input1› ‹input2› . . . ;) act similarly " +
                    "to functional dependencies. However, their inputs only fail with probability p the failure of the trigger. " +
                    "The probabilistic dependency has a single underlying random choice. If it causes one of its inputs to fail, " +
                    "all of them will fail, too.";
            }
        }
        public override string ToGalileo()
        {
            string builder = "";
            foreach (var v in Parents)
            {
                builder += $"{v.Title} pdep={Probability} {getChildrenString()};\n";
            }
            return builder;
        }
    }
}
