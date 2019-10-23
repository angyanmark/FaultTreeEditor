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
        public override string ImageSource
        {
            get
            {
                return "https://www.w3schools.com/w3css/img_lights.jpg";
            }
        }
        public override string DisplayTitle
        {
            get
            {
                return "Basic event";
            }
        }
        public override string ToGalileo()
        {
            return $"{Title} lambda={Lambda};\n";
        }
    }
}
