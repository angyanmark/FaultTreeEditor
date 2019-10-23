using System;
using System.Collections.Generic;
using System.Text;

namespace FaultTreeEditor.Core.Models
{
    public class BasicEvent : Event
    {
        public double Probability { get; set; } = 0.0;
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
            return $"{Title} lambda={Probability};\n";
        }
    }
}
