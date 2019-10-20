using System;
using System.Collections.Generic;
using System.Text;

namespace FaultTreeEditor.Core.Models
{
    public class VoteGate : Gate
    {
        public int K { get; set; }
        public int M { get; set; }
        public override string ToString()
        {
            string builder = "";
            if (M == 0)
            {
                foreach (var v in Parents)
                {
                    builder += $"{v.Title} vot{K} {getChildrenString()};\n";
                }
            }
            else
            {
                foreach (var v in Parents)
                {
                    builder += $"{v.Title} {K}of{M} {getChildrenString()};\n";
                }
            }
            return builder;
        }
    }
}
