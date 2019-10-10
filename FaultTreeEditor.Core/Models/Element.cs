using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FaultTreeEditor.Core.Models
{
    public abstract class Element
    {
        public ObservableCollection<Element> Parents { get; set; }
        public ObservableCollection<Element> Children { get; set; }
        public string Title { get; set; }
        public string ImageSource { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public ElementType ElementType { get; set; }
    }
}
