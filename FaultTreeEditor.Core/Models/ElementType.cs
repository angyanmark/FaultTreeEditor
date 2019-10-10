using System;
using System.Collections.Generic;
using System.Text;

namespace FaultTreeEditor.Core.Models
{
    public enum ElementType
    {
        TopLevelEvent,
        Event,
        BasicEvent,
        OrGate,
        AndGate,
        VoteGate,
    }
}
