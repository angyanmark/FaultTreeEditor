using System;
using System.Collections.Generic;
using System.Text;

namespace FaultTreeEditor.Core.Models
{
    public class BasicEvent : Event
    {
        private double lambda = 0;
        public double Lambda
        {
            get { return lambda; }
            set { Set(ref lambda, value); }
        }
        private double dormancy = 0;
        public double Dormancy
        {
            get { return dormancy; }
            set { Set(ref dormancy, value); }
        }
        public override string ImageSource
        {
            get
            {
                return "/Assets/Images/Elements/basicevent.png";
            }
        }
        public override string DisplayTitle
        {
            get
            {
                return "Basic event";
            }
        }
        public override string Info
        {
            get
            {
                return "Basic events describe the independent " +
                    "failures that may occur in the system. The time first failure since mission start is " +
                    "exponentially distributed with a rate of λ, i.e., P(T(fail) < t)  1 − e^(−λt). When the basic " +
                    "event is serving as a warm spare, its failure rate is reduced to dormancy·λ. The dormancy " +
                    "factor may be omitted if it is equal to 1.";
            }
        }
        public override string GalileoFormat
        {
            get
            {
                return "Galileo: ‹name› lambda=‹λ› dorm=‹dormancy›;";
            }
        }
        public override string ToGalileo()
        {
            return $"{Title} lambda={Lambda} dorm={Dormancy};\n";
        }
    }
}
