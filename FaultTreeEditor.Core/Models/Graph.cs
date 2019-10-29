using System;
using System.Collections.Generic;
using System.Text;

namespace FaultTreeEditor.Core.Models
{
    public class Graph
    {
        public List<Element> Elements { get; set; }
        public List<Connection> Connections { get; set; }
    }
}
