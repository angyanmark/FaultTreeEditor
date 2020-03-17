using System;
using System.Collections.Generic;
using System.Text;

namespace FaultTreeEditor.Core.Models
{
    public class Event : Element
    {
        public override string Source
        {
            get
            {
                return "event.png";
            }
        }
        public override string DisplayTitle
        {
            get
            {
                return "Event";
            }
        }
        public override string Info
        {
            // TODO info
            get
            {
                return "Event info.";
            }
        }
        public override string GalileoFormat
        {
            // TODO galileo format
            get
            {
                return "Event Galileo format.";
            }
        }
        public override string ToGalileo()
        {
            return "";
        }
    }
}
