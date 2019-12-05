using System;
using System.Collections.Generic;
using System.Text;

namespace FaultTreeEditor.Core.Models
{
    public class WarmSpareGate : Gate
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
                return "Warm spare gate";
            }
        }
        public override string Info
        {
            get
            {
                return "Warm spare gates (Galileo: ‹name› wsp ‹primary› ‹spare1› ‹spare2› . . . ;) maintain a pool of spares. " +
                    "When the primary input fails, its first free spare is claimed. When the claimed spare fails, the gate " +
                    "claims a new one(from left to right) until the pool of spares is exhausted. If the last spare fails, " +
                    "the gate itself fails. Spares assigned to a spare gate may not have any common events, but a spare " +
                    "can be assigned to multiple spare gates. However, a spare can only be claimed by at most one spare gate " +
                    "at a time. Thus we can model contention between subsystems for shares spares. Rates of basic events in " +
                    "unclaimed spares are multiplied by their dormancy factors. While Storm - DFT accepts the keywords csp and " +
                    "hsp for cold and hot spares, respectively, they are treated as synonyms for warm spares.";
            }
        }
        public override string ToGalileo()
        {
            string builder = "";
            foreach (var v in Parents)
            {
                builder += $"{v.Title} wsp {getChildrenString()};\n";
            }
            return builder;
        }
    }
}
