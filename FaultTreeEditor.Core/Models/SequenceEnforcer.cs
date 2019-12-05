﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FaultTreeEditor.Core.Models
{
    public class SequenceEnforcer : Gate
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
                return "Sequence enforcer";
            }
        }
        public override string Info
        {
            get
            {
                return "Sequence enforcers (Galileo: ‹name› seq ‹input1› ‹input2› . . . ;) ensure that their inputs " +
                    "only fail in the specified order, i.e., input2 will never fail unless input1 has already failed. " +
                    "Similarly to the dependencies, their outputs are “dummy”.";
            }
        }
        public override string ToGalileo()
        {
            string builder = "";
            foreach (var v in Parents)
            {
                builder += $"{v.Title} seq {getChildrenString()};\n";
            }
            return builder;
        }
    }
}