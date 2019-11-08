using System;
using System.Collections.Generic;
using System.Text;

namespace FaultTreeEditor.Core.Models
{
    public class Project
    {
        public string Title { get; set; } = "New Project";
        public FaultTree FaultTree { get; set; } = new FaultTree();
    }
}
