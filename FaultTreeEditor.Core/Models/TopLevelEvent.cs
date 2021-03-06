﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FaultTreeEditor.Core.Models
{
    public class TopLevelEvent : Event
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
                return "Top level event";
            }
        }
        public override string Info
        {
            get
            {
                return "Top level event info.";
            }
        }
        public override string GalileoFormat
        {
            get
            {
                return "Top level event Galileo format.";
            }
        }
        public override string ToGalileo()
        {
            return $"toplevel {Title};\n";
        }
    }
}
