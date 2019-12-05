﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FaultTreeEditor.Core.Models
{
    public class FunctionalDependency : Gate
    {
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
                return "Functional dependency";
            }
        }
        public override string Info
        {
            get
            {
                return "Functional dependencies (Galileo: ‹name› fdep ‹trigger› ‹input1› ‹input2› . . . ;) cause their inputs " +
                    "to fail(provided they have not already failed) when their trigger event fails. Every input except the " +
                    "trigger must be a basic event. The outputs of functional dependencies are “dummy outputs”, because " +
                    "the event name will never actually fail.";
            }
        }
        public override string ToGalileo()
        {
            string builder = "";
            foreach (var v in Parents)
            {
                builder += $"{v.Title} fdep {getChildrenString()};\n";
            }
            return builder;
        }
    }
}